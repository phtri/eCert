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
            //if (Session["RollNumber"] != null)
            //{
            //    if ((bool)Session["isUpdatedEmail"])
            //    {

            //    }
            //    else
            //    {
            //        //redirect to update personal email page
            //        return RedirectToAction("UpdatePersonalEmail", "Authentication");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Authentication");
            //}

            //Session["RollNumber"].ToString();
            string rollNumber = "HE139476";
            //Get passed subject 
            FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
            FAP_Service.Subject[] fapPassedSubject = client.GetPassedSubjects(rollNumber);
            List<SubjectViewModel> subjects = _transcriptServices.ConvertToListSubjectViewModel(fapPassedSubject);
            return View(subjects);
        }

        [HttpPost]
        public ActionResult GenerateCertificate(string subjectCode)
        {
            if (Session["RollNumber"] != null)
            {
                if ((bool)Session["isUpdatedEmail"])
                {
                    string rollNumber = Session["RollNumber"].ToString();
                    //Get passed subject detail from FU
                    FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
                    FAP_Service.Subject detailPassedSubject = client.GetDetailPassedSubject(rollNumber, subjectCode);
                    SubjectViewModel subject = _transcriptServices.ConvertToSubjectViewModel(detailPassedSubject);
                    _transcriptServices.GenerateCertificateForSubject(subject, rollNumber);


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