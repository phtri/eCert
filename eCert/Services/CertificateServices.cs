using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using static eCert.Utilities.Constants;

namespace eCert.Services
{
    public class CertificateServices
    {
        private readonly CertificateDAO _certificateDAO;
        public CertificateServices()
        {
            _certificateDAO = new CertificateDAO();
        }
        //Get list certificates of user pagination
        public Pagination<CertificateViewModel> GetCertificatesPagination(string rollNumber, int pageSize, int pageNumber)
        {
            Pagination<Certificate> certificates = _certificateDAO.GetCertificatesPagination(rollNumber, pageSize, pageNumber);
            Pagination<CertificateViewModel> certificatesViewModel = AutoMapper.Mapper.Map<Pagination<Certificate>, Pagination<CertificateViewModel>>(certificates);

            //Populate certificate content
            foreach (CertificateViewModel item in certificatesViewModel.PagingData)
            {
                item.Content = _certificateDAO.GetCertificateContent(item.CertificateId);
            }
            return certificatesViewModel;
        }
        public CertificateViewModel GetCertificateDetail(int certId)
        {
            Certificate certificate = _certificateDAO.GetCertificateById(certId);
            return AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(certificate);
        }
        //public CertificateViewModel GetFUCertificateDetail(int certId, string razorView = "")
        //{
            
        //}
        public void GeneratePdfFuCert(Certificate cert)
        {
            //string razorString = RenderRazorViewToString("~/Views/Shared/Certificate.cshtml", AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(certificate));
        }
        
        public Result ValidateCertificateInfor(CertificateViewModel certificate)
        {
            //Certificate name
            if (string.IsNullOrEmpty(certificate.CertificateName))
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "The certificate name is required."
                };
            }

            //Certificate content (link / file)
            if (string.IsNullOrEmpty(certificate.Content) && certificate.CertificateFile[0] == null)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "Certificate link or certificate file is required."
                };
            }

            //CertificateDate
            if (certificate.DateOfIssue != DateTime.MinValue && certificate.DateOfExpiry != DateTime.MinValue)
            {
                if (DateTime.Compare(certificate.DateOfIssue, certificate.DateOfExpiry) >= 0)
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Message = "Issue date have to be ealier than expiry date."
                    };
                }
                else if (DateTime.Compare(certificate.DateOfIssue, DateTime.Now) > 0)
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Message = "Issue Date can not be in the future."
                    };
                }
            }
            return new Result()
            {
                IsSuccess = true
            };
        }

        public Result ValidateCertificateFiles(HttpPostedFileBase[] files)
        {
            const int sizeLimit = 20; //20Mb

            int totalSize = 0;
            foreach (HttpPostedFileBase file in files)
            {
                string[] supportedTypes = { "pdf", "jpg", "jpeg", "png"};
                string fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();
                totalSize += file.ContentLength;
                if (Array.IndexOf(supportedTypes, fileExt) < 0)
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Message = "File Extension Is InValid - Only Upload PDF/PNG/JPG/JPEG file"
                    };
                }
                //Total files size > 20mb
                else if (totalSize > (sizeLimit * 1024 * 1024))
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Message = "Total size of files can not exceed " + sizeLimit + "Mb"
                    };
                }
            }
            return new Result()
            {
                IsSuccess = true
            };
        }

        private string GetFileExtensionConstants(string fileName)
        {
            if (Path.GetExtension(fileName).Substring(1).ToLower() == "pdf")
            {
                return Constants.CertificateFormat.PDF;
            }
            else if (Path.GetExtension(fileName).Substring(1).ToLower() == "png")
            {
                return Constants.CertificateFormat.PNG;
            }
            else if (Path.GetExtension(fileName).Substring(1).ToLower() == "jpg")
            {
                return Constants.CertificateFormat.JPG;
            }
            else if (Path.GetExtension(fileName).Substring(1).ToLower() == "jpeg")
            {
                return Constants.CertificateFormat.JPEG;
            }
            return null;
        }

        public List<CertificateContents> GetCertificateContents(string links, HttpPostedFileBase[] files, string studentCode, string certVerifyCode)
        {
            List<CertificateContents> contents = new List<CertificateContents>();
            //Link in certificate
            if (!string.IsNullOrEmpty(links))
            {
                //Multiple links
                string[] lines = links.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string link in lines)
                {
                    contents.Add(new CertificateContents()
                    {
                        Content = link,
                        CertificateFormat = Constants.CertificateFormat.LINK,
                        
                    });
                }
            }
            //File in certificate
            if (files != null && files[0] != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    string saveFolder = GenerateCertificateSaveFolder(studentCode, certVerifyCode, CertificateIssuer.PERSONAL, GetFileExtensionConstants(file.FileName));
                    CertificateContents certificatecontents = new CertificateContents()
                    {
                        Content = Path.Combine(saveFolder, file.FileName),
                        CertificateFormat = GetFileExtensionConstants(file.FileName),
                        
                    };
                    contents.Add(certificatecontents);
                }
            }

            return contents;

        }

        //Add new certificate to database
        public void AddCertificate(Certificate certificate)
        {
            //Insert to Certificates & CertificateContents table
            _certificateDAO.AddCertificate(certificate);
        }
        //Add multiple certificates
        public void AddMultipleCertificates(List<Certificate> certificates)
        {
            //Insert to Certificates & CertificateContents table
            _certificateDAO.AddMultipleCertificates(certificates);
        }
        public void UploadCertificatesFile(HttpPostedFileBase[] files, string studentCode, string certVerifyCode)
        {

            string uploadedPath = string.Empty;

            foreach (HttpPostedFileBase file in files)
            {
                //Get saved folder
                if (GetFileExtensionConstants(file.FileName) == CertificateFormat.PDF)
                {
                    uploadedPath = SaveCertificateLocation.BaseFolder + GenerateCertificateSaveFolder(studentCode, certVerifyCode, CertificateIssuer.PERSONAL, CertificateFormat.PDF);
                    SaveCertificate(file, uploadedPath);
                    
                }
                else if (GetFileExtensionConstants(file.FileName) == CertificateFormat.JPEG
                  || GetFileExtensionConstants(file.FileName) == CertificateFormat.PNG
                  || GetFileExtensionConstants(file.FileName) == CertificateFormat.JPG)
                {
                    uploadedPath = SaveCertificateLocation.BaseFolder + GenerateCertificateSaveFolder(studentCode, certVerifyCode, CertificateIssuer.PERSONAL, CertificateFormat.PNG);
                    SaveCertificate(file, uploadedPath);
                    
                }
            }
        }
        public void SaveCertificate(HttpPostedFileBase file, string uploadedPath) {
            if (!Directory.Exists(uploadedPath))
            {
                Directory.CreateDirectory(uploadedPath);
            }
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(uploadedPath, fileName);
                //Save file to server folder  
                file.SaveAs(path);
            }
        }
        //Generate save folder for a certificate (Based on certificate Type)
        public string GenerateCertificateSaveFolder(string studentCode, string certVerifyCode, string certificateIssuer, string certificateFormat)
        {
            string folderLocation = string.Empty;
            //FU Education Certificate
            if (certificateIssuer == CertificateIssuer.FPT)
            {
                //PDF
                if (certificateFormat == CertificateFormat.PDF)
                {
                    return studentCode + @"\FU_EDU\" + certVerifyCode + @"\PDFs";
                }
                //Img (Generated from PDF file)
                else if (certificateFormat == CertificateFormat.PNG)
                {
                    return studentCode + @"\FU_EDU\" + certVerifyCode + @"\Imgs";
                }
            }
            //Personal certificate
            else
            {
                if (certificateFormat == CertificateFormat.PDF)
                {
                    return studentCode + @"\Personal\" + certVerifyCode + @"\PDFs";
                }
                //Img (Generated from PDF file)
                else if (certificateFormat == CertificateFormat.PNG
                    || certificateFormat == CertificateFormat.JPG
                    || certificateFormat == CertificateFormat.JPEG
                )
                {
                    return studentCode + @"\Personal\" + certVerifyCode + @"\Imgs";
                }
            }
            return folderLocation;
        }
        //Generate PDF for a certificate
        public void GeneratePdfForCertificate(string certificateName, string certVerifyCode, string studentCode, string pdfHTMLTemplate)
        {
            var Renderer = new IronPdf.HtmlToPdf();
            //Get pdf file
            var PDF = Renderer.RenderHtmlAsPdf(pdfHTMLTemplate);
            //Save PDF file to folder
            string certificateFolder = GenerateCertificateSaveFolder(studentCode, certVerifyCode, CertificateIssuer.FPT, CertificateFormat.PDF);

            if (!Directory.Exists(certificateFolder))
            {
                Directory.CreateDirectory(certificateFolder);
            }
            string saveCertificateLocation = certificateFolder + certificateName + "_" + studentCode + ".pdf";

            PDF.SaveAs(saveCertificateLocation);
        }
        //Remove certificate & certificate_content from database
        public void DeleteCertificate(int certificateId)
        {
            Certificate deleteCertificate = _certificateDAO.GetCertificateById(certificateId);

            //Get files of delete certificate
            List<CertificateContents> files = deleteCertificate.CertificateContents
                .Where(cert => cert.CertificateFormat == Constants.CertificateFormat.JPEG
                || cert.CertificateFormat == Constants.CertificateFormat.JPG
                || cert.CertificateFormat == Constants.CertificateFormat.PNG
                || cert.CertificateFormat == Constants.CertificateFormat.PDF)
                .ToList();
            string[] fileLocations = files.Select(content => content.Content).ToArray<string>();
            //Delete certificate files on computer
            
            string deleteFolder = Directory.GetDirectories(SaveCertificateLocation.BaseFolder, deleteCertificate.VerifyCode, SearchOption.AllDirectories).FirstOrDefault();
            if (Directory.Exists(deleteFolder))
            {
                Directory.Delete(deleteFolder, true);
            }
            //Delete in database
            _certificateDAO.DeleteCertificate(certificateId);
        }
        public string DownloadPersonalCertificate(int certificateId)
        {
            string fileLocation = string.Empty;
            //Get certificate
            Certificate cert = _certificateDAO.GetCertificateById(certificateId);
            //Download personal certificate
            if(cert.Issuer == CertificateIssuer.PERSONAL)
            {
                string certificateFolder = Directory.GetDirectories(SaveCertificateLocation.BaseFolder, cert.VerifyCode, SearchOption.AllDirectories).FirstOrDefault();
                //Write all certificate link to file
                List<string> links = cert.CertificateContents.Where(content => content.CertificateFormat == CertificateFormat.LINK).Select(certContent => certContent.Content).ToList();
                if(links.Count > 0)
                {
                    string linkStr = string.Empty;
                    links.ForEach(str => linkStr += str + Environment.NewLine);
                    string fileName = cert.CertificateName + "_links.txt";
                    string linksSavePath = Path.Combine(certificateFolder, fileName);
                    File.WriteAllText(linksSavePath, linkStr);
                }
                //Download certificate files
                List<string> files = cert.CertificateContents.Where(content => content.CertificateFormat != CertificateFormat.LINK).Select(certContent => certContent.Content).ToList();

                //Save zip to temp folder
                if (!Directory.Exists(SaveCertificateLocation.BaseTempFolder))
                {
                    Directory.CreateDirectory(SaveCertificateLocation.BaseTempFolder);
                }
                string zipPath = SaveCertificateLocation.BaseTempFolder + @"\" + cert.CertificateName + ".zip";
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                ZipFile.CreateFromDirectory(certificateFolder, zipPath);

                fileLocation = zipPath;

                
            }
            else
            {
                //Download FU Certificate

            }
            return fileLocation;
        }
        
        public void Test()
        {
            _certificateDAO.GetCertificateById(20788);
        }
    }
}