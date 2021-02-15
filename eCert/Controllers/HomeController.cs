using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
namespace eCert.Controllers
{
    public class HomeController : Controller
    {
        private string errorMessage = "";
        private readonly CertificateServices _certificateServices;
        private readonly CertificateContentServices _certificateContentServices;
        public HomeController()
        {
            _certificateServices = new CertificateServices();
            _certificateContentServices = new CertificateContentServices();
        }
        public ActionResult Index(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            int userId = 1;
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(userId, pageSize, pageNumber);
            ViewBag.message = mesage;
            return View();
        }
        public ActionResult LoadListOfCert(string mesage, int pageSize = 5, int pageNumber = 1)
        {
            int userId = 1;
            //Get all certiificates of a user
            ViewBag.Pagination = _certificateServices.GetCertificatesPagination(userId, pageSize, pageNumber);
            return PartialView();
        }

        [HttpPost]
        public ActionResult Delete(int certId)
        {
            //_certificateDao.DeleteCertificate(certId);
            //TempData["Msg"] = "Delete certificate successfully";

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult AddCertificate(CertificateViewModel cert)
        {
            try
            { 
                if (string.IsNullOrEmpty(cert.CertificateName))
                {
                    TempData["Msg"] = "The certificate name is required.";
                }
                else if (string.IsNullOrEmpty(cert.Description))
                {
                    TempData["Msg"] = "The description is required.";
                }
                else if (string.IsNullOrEmpty(cert.Content) && cert.CertificateFile[0] == null)
                {
                    TempData["Msg"] = "Certificate link or certificate file is required.";
                    //Certificate file
                }
                else
                {
                    Certificate addCertificate = new Certificate();
                    List<CertificateContents> contents = new List<CertificateContents>();
                    if (cert.DateOfIssue != DateTime.MinValue && cert.DateOfExpiry != DateTime.MinValue)
                    {
                        if (DateTime.Compare(cert.DateOfIssue, cert.DateOfExpiry) >= 0)
                        {
                            TempData["Msg"] = "Issue Date have to be ealier than Expiry Date.";
                            return RedirectToAction("Index");
                        }
                        else if (DateTime.Compare(cert.DateOfIssue, DateTime.Now) > 0)
                        {
                            TempData["Msg"] = "Issue Date can not be in the future.";
                            return RedirectToAction("Index");
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(cert.Content))
                    {
                        //xu ly nhap nhieu link
                        string[] lines = cert.Content.Split(
                            new[] { "\r\n", "\r", "\n" },
                            StringSplitOptions.None
                        );
                        //Add certificate (with link) to database
                        addCertificate = new Certificate()
                        {
                            OrganizationId = 1,
                            UserId = 1,
                            CertificateName = cert.CertificateName,
                            Description = cert.Description,
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                            Issuer = Constants.CertificateType.PERSONAL,
                            ViewCount = 100,
                            VerifyCode = "XYZ"
                        };
                        
                        foreach (string link in lines)
                        {
                            CertificateContents certificatecontents = new CertificateContents()
                            {
                                Content = link,
                                Format = Constants.CertificateFormat.LINK,
                                created_at = DateTime.Now,
                                updated_at = DateTime.Now
                            };

                            contents.Add(certificatecontents);
                        }
                        
                    
                    }
                    if(cert.CertificateFile[0] != null)
                    {
                        bool result = validateUploadFile(cert.CertificateFile);
                        //If check file success
                        if (result)
                        {
                            try
                            {
                                uploadFile(cert.CertificateFile);
                                addCertificate = new Certificate()
                                {
                                    OrganizationId = 1,
                                    UserId = 1,
                                    CertificateName = cert.CertificateName,
                                    Description = cert.Description,
                                    created_at = DateTime.Now,
                                    updated_at = DateTime.Now,
                                    Issuer = Constants.CertificateType.PERSONAL,
                                    ViewCount = 100,
                                    VerifyCode = "XYZ"
                                };
                                
                                foreach (HttpPostedFileBase file in cert.CertificateFile)
                                {
                                    CertificateContents certificatecontents = new CertificateContents()
                                    {
                                        Content = Path.GetFileName(file.FileName),
                                        Format = _certificateContentServices.GetFileExtension(file),
                                        created_at = DateTime.Now,
                                        updated_at = DateTime.Now
                                    };
                                    contents.Add(certificatecontents);
                                }
                                
                                    
                            }
                            catch
                            {
                                TempData["Msg"] = "Upload file failed";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            TempData["Msg"] = errorMessage;
                            return RedirectToAction("Index");
                        }
                    }
                    _certificateServices.AddCertificate(addCertificate, contents);
                    TempData["Msg"] = "Added Successfully.";
                }
                return RedirectToAction("Index");


                ////case cert is link
                //else if (cert.CertificateFile == null)
                //{
                //    if (cert.Content == null)
                //    {
                //        TempData["Msg"] = "The certificate link is required."; 
                //    }
                //    else
                //    {

                //    }
                //    //Certificate file
                //}
                //else if (cert.CertificateFile != null)
                //{
                //    bool result = validateUploadFile(cert.CertificateFile);
                //    //If check file success
                //    if (result)
                //    {
                //        try
                //        {
                //            uploadFile(cert.CertificateFile);
                //            //_certificateDao.CreateACertificate(new Certificate() { OrganizationId = 1, UserId = 18, CertificateName = cert.CertificateName, Description = cert.Description, Content = Path.GetFileName(cert.CertificateFile.FileName), created_at = DateTime.Now, updated_at = DateTime.Now, Format = Path.GetExtension(cert.CertificateFile.FileName).Substring(1).ToUpper(), Type = Constants.CertificateType.PERSONAL });
                //        }
                //        catch
                //        {
                //            TempData["Msg"] = "Upload file failed";
                //        }
                //    }
                //    else
                //    {
                //        TempData["Msg"] = errorMessage;
                //    }

                //}
                //else
                //{
                //    TempData["Msg"] = "Added Successfully.";
                //}
                //return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Msg"] = "Something went wrong.";
                return RedirectToAction("Index");
            }
            
        }

        public void DownloadCertificate(int certificateId)
        {
            //string fileName = _certificateDao.GetCertificateContent(certificateId);


            //FileInfo file = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles/" + fileName);
            //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            //Response.Clear();
            //Response.ClearHeaders();
            //Response.ClearContent();
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            //Response.AddHeader("Content-Length", file.Length.ToString());
            //Response.ContentType = "text/plain";
            //Response.Flush();
            //Response.TransmitFile(file.FullName);
            //Response.End();

        }

        private bool validateUploadFile(HttpPostedFileBase[] files)
        {
            
            int limitFileSize = 20;
            try
            {
                int totalSize = 0;
                foreach (HttpPostedFileBase file in files)
                {
                    string[] supportedTypes = { "pdf", "jpg", "jpeg", "png" };
                    string fileExt = Path.GetExtension(file.FileName).Substring(1);
                    totalSize += file.ContentLength;
                    if (Array.IndexOf(supportedTypes, fileExt) < 0)
                    {
                        errorMessage = "File Extension Is InValid - Only Upload PDF/PNG/JPG/JPEG File";
                        return false;
                    }
                    else if (totalSize > (limitFileSize * 1024 * 1024))
                    {
                        errorMessage = "Total size of files dose not have to exceed " + limitFileSize + "MB";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong";
                return false;
            }
        }

        public void uploadFile(HttpPostedFileBase[] files)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(folderPath, InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);                       
                    }
                }
            }
        }


        //public ActionResult EditCertificate(int certId)
        //{
        //    Certificate cert = _certificateDao.GetCertificateByID(certId);
        //    return Json(cert, JsonRequestBehavior.AllowGet);
        //}

        //For testing purpose
        public ActionResult Test()
        {
            Certificate addCertificate = new Certificate()
            {
                OrganizationId = 1,
                UserId = 1,
                CertificateName = "TEST SQL TRANSACTION 2",
                Description = "THIS IS A LONG DESCRIPTION 2",
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                Issuer = Constants.CertificateType.PERSONAL,
                ViewCount = 100,
                VerifyCode = "XYZ",
                DateOfIssue = DateTime.Now,
                DateOfExpiry = DateTime.Now
            };
            List<CertificateContents> list = new List<CertificateContents>()
            {
                new CertificateContents(){Content = "Test mung 4 tet 1", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.PDF},
                new CertificateContents(){Content = "Test mung 4 tet 2", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.PNG},
                new CertificateContents(){Content = "Test mung 4 tet 3", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.LINK},
                new CertificateContents(){Content = "Test mung 4 tet 4", created_at = DateTime.Now, updated_at = DateTime.Now, Format = Constants.CertificateFormat.JPEG}
            };
            _certificateServices.AddCertificate(addCertificate, list);
            return RedirectToAction("Index");
        }

    }
}

