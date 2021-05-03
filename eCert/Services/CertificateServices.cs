﻿using eCert.Controllers;
using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static eCert.Utilities.Constants;

namespace eCert.Services
{
    public class CertificateServices
    {
        private readonly CertificateDAO _certificateDAO;
        private readonly UserDAO _userDAO;
        private readonly EmailServices _emailServices;
        public CertificateServices()
        {
            _certificateDAO = new CertificateDAO();
            _userDAO = new UserDAO();
            _emailServices = new EmailServices();
        }
        //Get list certificates of user pagination
        public Pagination<CertificateViewModel> GetCertificatesPagination(string rollNumber, int pageSize, int pageNumber, string keyword)
        {
            keyword = NormalizeSearchedKeyWord(keyword);
            Pagination<Certificate> certificates = _certificateDAO.GetCertificatesPagination(rollNumber, pageSize, pageNumber, keyword);
            Pagination<CertificateViewModel> certificatesViewModel = AutoMapper.Mapper.Map<Pagination<Certificate>, Pagination<CertificateViewModel>>(certificates);

            //Populate certificate content
            foreach (CertificateViewModel item in certificatesViewModel.PagingData)
            {
                item.Links = _certificateDAO.GetCertificateContent(item.CertificateId);
            }
            return certificatesViewModel;
        }
        public string ConvertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public List<CertificateViewModel> GetAllCertificatesByKeyword(string rollNumber, string keyword)
        {
            
            List<Certificate> certificates = _certificateDAO.GetAllCertificates(rollNumber, keyword);
            return AutoMapper.Mapper.Map<List<Certificate>, List<CertificateViewModel>>(certificates);
        }

        //get list report by user id
        public Pagination<ReportViewModel> GetReportPagination(int userId, int pageSize, int pageNumber)
        {
            
            Pagination<Report> reports = _certificateDAO.GetReportPagination(userId, pageSize, pageNumber);
            Pagination<ReportViewModel> reportViewModel = AutoMapper.Mapper.Map<Pagination<Report>, Pagination<ReportViewModel>>(reports);
            foreach (ReportViewModel report in reportViewModel.PagingData)
            {
                Certificate certificate = _certificateDAO.GetCertificateById(report.CertificateId);
                report.CertificateName = certificate.CertificateName;
            }
                return reportViewModel;
        }
        //get all report
        public Pagination<ReportViewModel> GetReportByUserIdPagination(int userId, int pageSize, int pageNumber)
        {
            Pagination<Report> reports = _certificateDAO.GetReportByUserIdPagination(userId, pageSize, pageNumber);
            Pagination<ReportViewModel> reportViewModel = AutoMapper.Mapper.Map<Pagination<Report>, Pagination<ReportViewModel>>(reports);
            foreach (ReportViewModel report in reportViewModel.PagingData)
            {
                Certificate certificate = _certificateDAO.GetCertificateById(report.CertificateId);
                User user = _userDAO.GetUserByUserId(report.UserId);
                report.CertificateName = certificate.CertificateName;
                report.RollNumber = user.RollNumber;
            }
            return reportViewModel;
        }
        public ReportViewModel GetReportByReportId(int reportID)
        {
            Report report = _certificateDAO.GetReportByReportId(reportID);
            return AutoMapper.Mapper.Map<Report, ReportViewModel>(report);
        }
        public CertificateViewModel GetCertificateDetail(int certId)
        {
            Certificate certificate = _certificateDAO.GetCertificateById(certId);
            return AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(certificate);
        }
        public CertificateViewModel GetCertificateByUrl(string url)
        {
            Certificate certificate = _certificateDAO.GetCertificateByUrl(url);
            return AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(certificate);
        }
        public CertificateViewModel GetCertificateByRollNumberAndSubjectCode(string rollNumber, string subjectCode)
        {
            return AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(_certificateDAO.GetCertificateByRollNumberAndSubjectCode(rollNumber, subjectCode));
        }
        //public CertificateViewModel GetFUCertificateDetail(int certId, string razorView = "")
        //{

        //}
        public void GeneratePdfFuCert(CertificateViewModel cert, string razorString)
        {
            string vituralPdfPath = GenerateCertificateSaveFolder(cert, CertificateFormat.PDF);
            string pdfSaveFolder = SaveLocation.BaseCertificateFolder + vituralPdfPath;
            if (!Directory.Exists(pdfSaveFolder))
            {
                Directory.CreateDirectory(pdfSaveFolder);
            }
            string pdffileName = Guid.NewGuid().ToString() + ".pdf";
            string pdfSavePath = Path.Combine(pdfSaveFolder, pdffileName);
            //Generate and save pdf
            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.CssMediaType = IronPdf.PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.PaperSize = IronPdf.PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;
            Renderer.PrintOptions.Title = cert.CertificateName;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.DPI = 300;
            Renderer.PrintOptions.FitToPaperWidth = true;
            Renderer.PrintOptions.JpegQuality = 100;
            Renderer.PrintOptions.GrayScale = false;
            Renderer.PrintOptions.FitToPaperWidth = true;
            Renderer.PrintOptions.InputEncoding = Encoding.UTF8;
            Renderer.PrintOptions.Zoom = 100;
            Renderer.PrintOptions.MarginTop = 0;  //millimeters
            Renderer.PrintOptions.MarginLeft = 0;  //millimeters
            Renderer.PrintOptions.MarginRight = 0;  //millimeters
            Renderer.PrintOptions.MarginBottom = 0;  //millimeters
            Renderer.PrintOptions.CreatePdfFormsFromHtml = true;
            var PDF = Renderer.RenderHtmlAsPdf(razorString);
            PDF.SaveAs(pdfSavePath);
            //Save certificate img
            string vituralImgPath = GenerateCertificateSaveFolder(cert, CertificateFormat.PNG);
            string imgSaveFolder = SaveLocation.BaseCertificateFolder + vituralImgPath;
            string imgFile = Guid.NewGuid().ToString() + ".png";
            string imgSavePath = Path.Combine(imgSaveFolder, imgFile);
            PDF.RasterizeToImageFiles(imgSavePath, ImageType.Png, 300);
            
            //Insert to [Certificate_Content] pdf file and img file location
            List<CertificateContents> contents = new List<CertificateContents>()
            {
                new CertificateContents()
                {
                    CertificateFormat = Constants.CertificateFormat.PNG,
                    CertificateId = cert.CertificateId,
                    Content = Path.Combine(vituralImgPath, imgFile)
                },
                new CertificateContents()
                {
                    CertificateFormat = Constants.CertificateFormat.PDF,
                    CertificateId = cert.CertificateId,
                    Content = Path.Combine(vituralPdfPath, pdffileName)
                }

            };
            _certificateDAO.AddCertificateContent(contents);
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
            //Issuer name
            //if (string.IsNullOrEmpty(certificate.IssuerName))
            //{
            //    return new Result()
            //    {
            //        IsSuccess = false,
            //        Message = "The issuer name is required."
            //    };
            //}

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
            

            //Certificate content (link / file)
            if (string.IsNullOrEmpty(certificate.Links) && certificate.CertificateFile[0] == null)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "Certificate link or certificate file is required."
                };
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
                string[] supportedTypes = { "pdf", "jpg", "jpeg", "png", "PDF", "JPG", "JPEG", "PNG"};
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
        public string GetFileExtensionConstants(string fileName)
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
        public void AddCertificateLinks(CertificateViewModel certViewModel)
        {
            List<CertificateContentsViewModel> contents = new List<CertificateContentsViewModel>();
            //Link in certificate
            if (!string.IsNullOrEmpty(certViewModel.Links))
            {
                //Multiple links
                string[] lines = certViewModel.Links.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string link in lines)
                {
                    certViewModel.CertificateContents.Add(new CertificateContentsViewModel()
                    {
                        Content = link,
                        CertificateFormat = Constants.CertificateFormat.LINK,
                    });
                }
            }
        }
        //Add report to DB
        public void AddReport(ReportViewModel reportViewModel, string rollNumber)
        {
            Report report = AutoMapper.Mapper.Map<ReportViewModel, Report>(reportViewModel);
            //get current user
            User user = _userDAO.GetUserByRollNumber(rollNumber);
            report.UserId = user.UserId;
            report.Status = StatusReport.PENDING;
            report.CreateTime = DateTime.Now;
            //add report to DB
            _certificateDAO.AddReport(report);
        }
        //Add new certificate to database
        public void AddCertificate(CertificateViewModel certificateViewModel, string issuerType)
        {
            Certificate certificate = AutoMapper.Mapper.Map<CertificateViewModel, Certificate>(certificateViewModel);
            if(issuerType == CertificateIssuer.PERSONAL)
            {
                certificate.IssuerType = CertificateIssuer.PERSONAL;
            }else if(issuerType == CertificateIssuer.FPT)
            {
                certificate.IssuerType = CertificateIssuer.FPT;
            }
            
            //Insert to Certificates & CertificateContents table
            _certificateDAO.AddCertificate(certificate);
        }
        //Add multiple certificates
        public int AddMultipleCertificates(List<Certificate> certificates, int typeImport)
        {
            //Insert to Certificates & CertificateContents table
            int result = _certificateDAO.AddMultipleCertificates(certificates, typeImport);
            //Send email to user
            string mailTitle = "Congratulations, Your Certificate is Ready!";
            Thread sendMailThread = new Thread(delegate ()
            {
                foreach (Certificate cert in certificates)
                {
                    User user = _userDAO.GetUserByRollNumber(cert.RollNumber);
                    
                    if (user != null)
                    {
                        string toMail = String.IsNullOrEmpty(user.PersonalEmail) ? user.AcademicEmail : user.PersonalEmail;
                        string mailContentEnglish = "Dear " + cert.FullName + ",\n" + "Congratulations! You’ve successfully completed " + cert.CertificateName + ". On behalf of FPT Education System we are pleased to issue your official certificate. Once again, congratulations on your achievement.";
                        _emailServices.SendEmail(toMail, mailTitle, mailContentEnglish);
                    }
                }
            });
            sendMailThread.Start();
            return result;
        }
        public void UploadCertificatesFile(CertificateViewModel certViewModel)
        {

            string uploadedPath = string.Empty;

            foreach (HttpPostedFileBase file in certViewModel.CertificateFile)
            {
                string fileExtension = GetFileExtensionConstants(file.FileName).ToLower();
                string newFileName = Guid.NewGuid().ToString() + "." + fileExtension;
                string vituralPath = GenerateCertificateSaveFolder(certViewModel, fileExtension.ToUpper());
                string saveFolder = Path.Combine(SaveLocation.BaseCertificateFolder, vituralPath);
                //Check if save folder exist
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }
                string savePath = Path.Combine(saveFolder, newFileName);
                file.SaveAs(savePath);

                //CertificateContentsViewModel
                certViewModel.CertificateContents.Add(new CertificateContentsViewModel()
                {
                    CertificateFormat = fileExtension.ToUpper(),
                    Content = Path.Combine(vituralPath, newFileName)
                });

            }
        } 
        //Generate save folder for a certificate (Based on certificate Type)
        public string GenerateCertificateSaveFolder(CertificateViewModel certViewModel, string certFormat)
        {
            string folderLocation = string.Empty;
            //FU Education Certificate
            if (certViewModel.IssuerType == CertificateIssuer.FPT)
            {
                //PDF
                if (certFormat == CertificateFormat.PDF)
                {
                    return certViewModel.RollNumber + @"\FU_EDU\" + certViewModel.Url + @"\PDFs\";
                }
                //Img (Generated from PDF file)
                else if (certFormat == CertificateFormat.PNG)
                {
                    return certViewModel.RollNumber + @"\FU_EDU\" + certViewModel.Url + @"\Imgs\";
                }
            }
            //Personal certificate
            else
            {
                if (certFormat == CertificateFormat.PDF)
                {
                    return certViewModel.RollNumber + @"\Personal\" + certViewModel.Url + @"\PDFs\";
                }
                //Img (Generated from PDF file)
                else if (certFormat == CertificateFormat.PNG
                    || certFormat == CertificateFormat.JPG
                    || certFormat == CertificateFormat.JPEG
                )
                {
                    return certViewModel.RollNumber + @"\Personal\" + certViewModel.Url + @"\Imgs\";
                }
            }
            return folderLocation;
        }
        //Remove certificate & certificate_content from database
        public void DeleteCertificate(string url)
        {
            Certificate deleteCertificate = _certificateDAO.GetCertificateByUrl(url);

            //Get files of delete certificate
            List<CertificateContents> files = deleteCertificate.CertificateContents
                .Where(cert => cert.CertificateFormat == Constants.CertificateFormat.JPEG
                || cert.CertificateFormat == Constants.CertificateFormat.JPG
                || cert.CertificateFormat == Constants.CertificateFormat.PNG
                || cert.CertificateFormat == Constants.CertificateFormat.PDF)
                .ToList();
            string[] fileLocations = files.Select(content => content.Content).ToArray<string>();
            //Delete certificate files on computer

            string deleteFolder = Directory.GetDirectories(SaveLocation.BaseCertificateFolder, deleteCertificate.Url, SearchOption.AllDirectories).FirstOrDefault();
            if (Directory.Exists(deleteFolder))
            {
                Directory.Delete(deleteFolder, true);
            }
            //Delete in database
            _certificateDAO.DeletePersonalCertificate(deleteCertificate.CertificateId);
        }
        public string DownloadPersonalCertificate(string url, string rollNumber)
        {
            string fileLocation = string.Empty;
            //Get certificate
            Certificate cert = _certificateDAO.GetCertificateByUrl(url);
            //Download personal certificate
            if(cert.IssuerType == CertificateIssuer.PERSONAL)
            {
                string certificateFolder = Directory.GetDirectories(SaveLocation.BaseCertificateFolder, cert.Url, SearchOption.AllDirectories).FirstOrDefault();
                
                //Write all certificate link to file
                List<string> links = cert.CertificateContents.Where(content => content.CertificateFormat == CertificateFormat.LINK).Select(certContent => certContent.Content).ToList();
                if(links.Count > 0)
                {
                    //Create certificate folder
                    if (string.IsNullOrEmpty(certificateFolder))
                    {
                        certificateFolder = SaveLocation.BaseCertificateFolder + rollNumber + @"\Personal\" + cert.Url;
                        Directory.CreateDirectory(certificateFolder);
                    }
                    string linkStr = string.Empty;
                    links.ForEach(str => linkStr += str + Environment.NewLine);
                    string fileName = cert.CertificateName + "_links.txt";
                    string linksSavePath = Path.Combine(certificateFolder, fileName);
                    File.WriteAllText(linksSavePath, linkStr);
                }
                //Download certificate files
                List<string> files = cert.CertificateContents.Where(content => content.CertificateFormat != CertificateFormat.LINK).Select(certContent => certContent.Content).ToList();

                //Save zip to temp folder
                if (!Directory.Exists(SaveLocation.BaseTempFolder))
                {
                    Directory.CreateDirectory(SaveLocation.BaseTempFolder);
                }
                string zipPath = SaveLocation.BaseTempFolder + @"\" + cert.CertificateName + ".zip";
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                ZipFile.CreateFromDirectory(certificateFolder, zipPath);
                fileLocation = zipPath;
            }
            return fileLocation;
        }
        public string DownloadFPTCertificate(string url, string type)
        {
            string fileLocation = string.Empty;
            //Get certificate
            Certificate cert = _certificateDAO.GetCertificateByUrl(url);
            if(cert.IssuerType != CertificateIssuer.PERSONAL)
            {
                CertificateContents content = cert.CertificateContents.Where(x => x.CertificateFormat == type).FirstOrDefault();
                fileLocation = Path.Combine(SaveLocation.BaseCertificateFolder + content.Content);
            }
            return fileLocation;
        }
        public string DownloadSearchedCertificate(string rollNumber, string keyword)
        {
            keyword = NormalizeSearchedKeyWord(keyword);
            string zipPath = SaveLocation.BaseTempFolder + Guid.NewGuid().ToString() + ".zip";
            if (!Directory.Exists(SaveLocation.BaseTempFolder))
            {
                Directory.CreateDirectory(SaveLocation.BaseTempFolder);
            }

            List<Certificate> certificates = _certificateDAO.GetAllCertificates(rollNumber, keyword);
            //Trí refactor
            foreach (Certificate certificate in certificates)
            {
                if(certificate.IssuerType == CertificateIssuer.PERSONAL)
                {
                    //Get all links of certificate
                    List<string> links = certificate.CertificateContents.Where(content => content.CertificateFormat == CertificateFormat.LINK).Select(certContent => certContent.Content).ToList();
                    if (links.Count > 0)
                    {
                        string linkStr = string.Empty;
                        links.ForEach(str => linkStr += str + Environment.NewLine);
                        string fileName = certificate.CertificateName + "_links.txt";
                        string linksSavePath = Path.Combine(SaveLocation.BaseTempFolder, fileName);
                        File.WriteAllText(linksSavePath, linkStr);
                        using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                        {
                            archive.CreateEntryFromFile(linksSavePath, certificate.CertificateName + ".txt");
                        }

                    }
                }
                List<CertificateContents> contents = certificate.CertificateContents;
                foreach (CertificateContents content in contents)
                {
                    if (content.CertificateFormat != CertificateFormat.LINK)
                    {
                        using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                        {
                            string[] strArr = content.Content.Split('\\');
                            string fileExtension = "." + strArr[strArr.Length - 1];
                            archive.CreateEntryFromFile(Path.Combine(SaveLocation.BaseCertificateFolder, content.Content), certificate.CertificateName + fileExtension);
                        }
                    }
                }
            }
            //End trí refactor
            return zipPath;
        }
        public string NormalizeSearchedKeyWord(string keyword)
        {
            //Remove tone in Vietnamse
            string result = keyword.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");

            //Remove first and last space
            result = result.Trim();

            //Remove space between
            result = Regex.Replace(result, @"\s+", " ");
            return result;
        }
        public bool IsOwnerOfCertificate(string rollNumber, string certUrl)
        {
            if(string.IsNullOrEmpty(rollNumber) || string.IsNullOrEmpty(certUrl))
            {
                return false;
            }
            return _certificateDAO.IsOwnerOfCertificate(rollNumber, certUrl);
        }
        
        public void UpdateReport(ReportViewModel reportViewModel)
        {
            Report report = AutoMapper.Mapper.Map<ReportViewModel, Report>(reportViewModel);
            _certificateDAO.UpdateReport(report);
        }
    }
}