using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class AdminController : Controller
    {
        private readonly CertificateServices _certificateServices;
        public AdminController()
        {
            _certificateServices = new CertificateServices();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(ImportExcel importExcelFile)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = importExcelFile.File;
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }

                    DataTable dataTable = new DataTable();
                    conString = string.Format(conString, filePath);

                    //Fill data from excel to data table
                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dataTable);
                                connExcel.Close();
                            }
                        }
                    }

                    List<Certificate> certificateList = new List<Certificate>();
                    //Generate certificate from datatable
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Certificate certificate = new Certificate()
                        {
                            CertificateName = row["CertificateName"].ToString(),
                            VerifyCode = Guid.NewGuid().ToString(),
                            Issuer = row["Issuer"].ToString(),
                            Description = row["Description"].ToString(),
                            ViewCount = 0,
                            DateOfIssue = DateTime.Now,
                            DateOfExpiry = DateTime.Now,
                            UserId = 1,
                            OrganizationId = Int32.Parse(row["OrganizationId"].ToString()),
                        };

                        //Generate PDF for FU Certificate
                        string razorString = RenderRazorViewToString("~/Views/Shared/Certificate.cshtml", AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(certificate));
                        var Renderer = new IronPdf.HtmlToPdf();
                        var PDF = Renderer.RenderHtmlAsPdf(razorString);
                        string savedFolder = _certificateServices.GenerateCertificateSaveFolder("HE6969", certificate.VerifyCode, Constants.CertificateIssuer.FPT, Constants.CertificateFormat.PDF);
                        string savedLocation = savedFolder + "\\" + certificate.CertificateName + ".pdf";
                        if (!Directory.Exists(savedFolder))
                        {
                            Directory.CreateDirectory(savedFolder);
                        }
                        PDF.SaveAs(savedLocation);


                        certificate.CertificateContents = new List<CertificateContents>()
                        {
                            new CertificateContents()
                            {
                                Content = savedFolder,
                                CertificateFormat = Constants.CertificateFormat.PDF,

                            }
                        };

                        certificateList.Add(certificate);
                    }

                    //Add list certificates to database
                    _certificateServices.AddMultipleCertificates(certificateList);




                    //conString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                    //using (SqlConnection con = new SqlConnection(conString))
                    //{
                    //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    //    {
                    //        //Set the database table name.
                    //        sqlBulkCopy.DestinationTableName = "Certificates";

                    //        //[OPTIONAL]: Map the Excel columns with that of the database table
                    //        sqlBulkCopy.ColumnMappings.Add("CertificateName", "CertificateName");
                    //        sqlBulkCopy.ColumnMappings.Add("Issuer", "Issuer");
                    //        sqlBulkCopy.ColumnMappings.Add("RoleNumber", "UserId");
                    //        sqlBulkCopy.ColumnMappings.Add("OrganizationId", "OrganizationId");

                    //        con.Open();
                    //        sqlBulkCopy.WriteToServer(dataTable);
                    //        con.Close();
                    //    }
                    //}
                }

                return View();
            }
            return View();
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}