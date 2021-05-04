using eCert.Daos;
using eCert.Models.Entity;
using eCert.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static eCert.Utilities.Constants;

namespace eCert.Services
{
    public class TranscriptServices
    {
        private readonly CertificateServices _certificateServices;
        private readonly CertificateDAO _certificateDAO;
        public TranscriptServices()
        {
            _certificateServices = new CertificateServices();
            _certificateDAO = new CertificateDAO();
        }
        //Convert subjects of Fap_Service to subject ViewModel
        public List<SubjectViewModel> ConvertToListSubjectViewModel(FAP_Service.Subject[] fapSubjects)
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>();
            foreach (FAP_Service.Subject fapSubject in fapSubjects)
            {
                SubjectViewModel subject = new SubjectViewModel()
                {
                    SubjectName = fapSubject.SubjectName,
                    SubjectCode = fapSubject.SubjectCode,
                    Mark = fapSubject.Mark,
                    Semester = fapSubject.Semester,
                    StudentFullName = fapSubject.StudentFullName
                };
                subjects.Add(subject);
            }
            return subjects;
        }
        public SubjectViewModel ConvertToSubjectViewModel(FAP_Service.Subject fapSubject)
        {
            return new SubjectViewModel()
            {
                SubjectName = fapSubject.SubjectName,
                SubjectCode = fapSubject.SubjectCode,
                Mark = fapSubject.Mark,
                Semester = fapSubject.Semester,
                StudentFullName = fapSubject.StudentFullName
            };
        }

        public void GenerateCertificateForSubject(SubjectViewModel subject, string rollNumber)
        {
            CertificateViewModel certViewModel = new CertificateViewModel()
            {
                CertificateName = subject.SubjectName,
                SubjectCode = subject.SubjectCode,
                FullName = _certificateServices.ConvertToUnSign3(subject.StudentFullName),
                RollNumber = rollNumber,
                Url = Guid.NewGuid().ToString(),
                SignatureId = GetSignatureById(subject.SignatureId).SignatureId
            };
            _certificateServices.AddCertificate(certViewModel, CertificateIssuer.FPT);
        }

        public EducationSystemViewModel GetEducationNameById(int eduId)
        {
            return AutoMapper.Mapper.Map<EducationSystem, EducationSystemViewModel>(_certificateDAO.GetEducationNameById(eduId));
        }
        public CampusViewModel GetCampusNameById(int campusId)
        {
            return AutoMapper.Mapper.Map<Campus, CampusViewModel>(_certificateDAO.GetCampusNameById(campusId));
        }
        public EducationSystemViewModel GetEduSystemByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<EducationSystem, EducationSystemViewModel>(_certificateDAO.GetEduSystemByCampusId(campusId));
        }
        public SignatureViewModel GetSignatureOfPrincipalByCampusId(int campusId)
        {
            return AutoMapper.Mapper.Map<Signature, SignatureViewModel>(_certificateDAO.GetSignatureOfPrincipalByCampusId(campusId));
        }

        public SignatureViewModel GetSignatureById(int signatureId)
        {
            return AutoMapper.Mapper.Map<Signature, SignatureViewModel>(_certificateDAO.GetSignatureById(signatureId));
        }

    }

    
}