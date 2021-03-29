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
        public TranscriptServices()
        {
            _certificateServices = new CertificateServices();
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
                Url = Guid.NewGuid().ToString()
            };
            _certificateServices.AddCertificate(certViewModel, CertificateIssuer.FPT);
        }
    }

    
}