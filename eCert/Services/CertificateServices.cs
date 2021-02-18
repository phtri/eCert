using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using eCert.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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
        public Pagination<CertificateViewModel> GetCertificatesPagination(int userId, int pageSize, int pageNumber)
        {
           Pagination<Certificate> certificates = _certificateDAO.GetCertificatesPagination(userId, pageSize, pageNumber);
            Pagination<CertificateViewModel> certificatesViewModel = AutoMapper.Mapper.Map<Pagination<Certificate>, Pagination<CertificateViewModel>>(certificates);

            //Populate certificate content
            foreach (CertificateViewModel item in certificatesViewModel.PagingData)
            {
                item.Content = _certificateDAO.GetCertificateContent(item.CertificateId);
            }
            return certificatesViewModel;
        }
        public CertificateViewModel GetCertificateById(int certId)
        {
            Certificate certificate = _certificateDAO.GetCertificateById(certId);
            return AutoMapper.Mapper.Map<Certificate, CertificateViewModel>(certificate);
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
            if(string.IsNullOrEmpty(certificate.Content) && certificate.CertificateFile[0] == null)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = "Certificate link or certificate file is required."
                };
            }

            //CertificateDate
            if(certificate.DateOfIssue != DateTime.MinValue && certificate.DateOfExpiry != DateTime.MinValue)
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
                string[] supportedTypes = { "pdf", "jpg", "jpeg", "png" };
                string fileExt = Path.GetExtension(file.FileName).Substring(1);
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
            if (Path.GetExtension(fileName).Substring(1) == "pdf")
            {
                return Constants.CertificateFormat.PDF;
            }
            else if (Path.GetExtension(fileName).Substring(1) == "png")
            {
                return Constants.CertificateFormat.PNG;
            }
            else if (Path.GetExtension(fileName).Substring(1) == "jpg")
            {
                return Constants.CertificateFormat.JPG;
            }
            else if (Path.GetExtension(fileName).Substring(1) == "jpeg")
            {
                return Constants.CertificateFormat.JPEG;
            }
            return null;
        }

        public List<CertificateContents> GetCertificateContents(string links, HttpPostedFileBase[] files)
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
                        Format = Constants.CertificateFormat.LINK,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    });
                }
            }
            //File in certificate
            if(files != null && files[0] != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    CertificateContents certificatecontents = new CertificateContents()
                    {
                        Content = Path.GetFileName(file.FileName),
                        Format = GetFileExtensionConstants(file.FileName),
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
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
        //Remove certificate & certificate_content from database
        public void DeleteCertificate(int certificateId)
        {
            _certificateDAO.DeleteCertificate(certificateId);
        }

        
        public void Test()
        {
            _certificateDAO.GetCertificateById(20788);
        }
    }
}