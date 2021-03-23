using eCert.Models.ViewModel;
using eCert.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class TranscriptController : Controller
    {
        private readonly TranscriptServices _transcriptServices;
        public TranscriptController()
        {
            _transcriptServices = new TranscriptServices();
        }
        // GET: Transcript
        public ActionResult ListTranscript()
        {
            if (Session["RollNumber"] != null)
            {
                if ((bool)Session["isUpdatedEmail"])
                {
                    string rollNumber = Session["RollNumber"].ToString();
                    //Get passed subject 
                    FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
                    FAP_Service.Subject[] fapPassedSubject = client.GetPassedSubject(rollNumber);
                    List<SubjectViewModel> subjects = _transcriptServices.ConvertToListSubjectViewModel(fapPassedSubject);
                    return View(subjects);
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        [HttpPost]
        public ActionResult GenerateCertificate(string semester, string subjectCode, string name, string mark)
        {
            if (Session["RollNumber"] != null)
            {
                if ((bool)Session["isUpdatedEmail"])
                {
                    string rollNumber = Session["RollNumber"].ToString();
                    //subject = new SubjectViewModel()
                    //{
                    //    Semester = "Fall 2019",
                    //    SubjectCode = "SWQ391",
                    //    Name = "Software Quality Assurance and Testing",
                    //    Mark = 6.5f
                    //};

                    
                    return View();
                }
                else
                {
                    //redirect to update personal email page
                    return RedirectToAction("UpdatePersonalEmail", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
    }
}