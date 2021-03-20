using eCert.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Services
{
    public class TranscriptServices
    {
        //Convert subjects of Fap_Service to subject ViewModel
        public List<SubjectViewModel> ConvertToListSubjectViewModel(FAP_Service.Subject[] fapSubjects)
        {
            List<SubjectViewModel> subjects = new List<SubjectViewModel>();
            foreach (FAP_Service.Subject fapSubject in fapSubjects)
            {
                SubjectViewModel subject = new SubjectViewModel()
                {
                    Name = fapSubject.Name,
                    SubjectCode = fapSubject.SubjectCode,
                    Mark = fapSubject.Mark,
                    Semester = fapSubject.Semester
                };
                subjects.Add(subject);
            }
            return subjects;
        }
    }
}