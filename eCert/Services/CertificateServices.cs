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
        //Add new certificate to database
        public void AddCertificate(Certificate certificate, List<CertificateContents> contents)
        {
            //Insert to Certificates & CertificateContents table
            _certificateDAO.AddCertificate(certificate, contents);
        }
        //Remove certificate & certificate_content from database
        public void RemoveCertificate(int certificateId)
        {

        }
        
    }
}