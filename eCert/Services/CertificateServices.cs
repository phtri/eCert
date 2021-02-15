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


        public void AddCertificate(Certificate certificate, List<CertificateContents> contents)
        {
            //Insert to Certificates table
            _certificateDAO.AddCertificate(certificate, contents);
        }

        



        //Check if CertificateViewModel is valid
        //public ValidationCheck VerifyCertificateViewModel(CertificateViewModel cert)
        //{
        //    if (String.IsNullOrEmpty(cert.CertificateName))
        //    {
        //        return new ValidationCheck()
        //        {
        //            IsValid = false,
        //            Message = "The certificate name is required."
        //        };
        //    }

        //    if (String.IsNullOrEmpty(cert.Description))
        //    {
        //        return new ValidationCheck()
        //        {
        //            IsValid = false,
        //            Message = "The description is required."
        //        };
        //    }

        //    //When certificate content format is link
        //    if(cert.Format == Constants.CertificateFormat.LINK)
        //    {
        //        if (String.IsNullOrEmpty(cert.Content))
        //        {
        //            return new ValidationCheck()
        //            {
        //                IsValid = false,
        //                Message = "The certificate link is required."
        //            };
        //        }
        //    }
        //    else
        //    {
        //        //When certificate content format is file
        //        //Check certificate file
        //        ValidationCheck certificateFileCheck = 
        //    }

        //}

        //private ValidationCheck VerifyCertificateFile(HttpPostedFileBase file)
        //{
        //    int limitFileSize = 20; //20mb

           
        //        string[] supportedTypes = { "pdf", "jpg", "jpeg", "png" };
        //        string fileExt = Path.GetExtension(file.FileName).Substring(1);

        //        if (Array.IndexOf(supportedTypes, fileExt) < 0)
        //        {
        //            return new ValidationCheck()
        //            {
        //                IsValid = false,
        //                Message = "File Extension Is InValid - Only Upload PDF/PNG/JPG/JPEG File"
        //            };
        //        }
        //        else if (file.ContentLength > (limitFileSize * 1024 * 1024))
        //        {
        //            return new ValidationCheck()
        //            {
        //                IsValid = false,
        //                Message = "File size Should Be UpTo " + limitFileSize + "MB"
        //            };
        //        }
        //        else
        //        {
        //            return new ValidationCheck()
        //            {
        //                IsValid = true,
        //            };
        //        }
        //    }
            
        //}



    }
}