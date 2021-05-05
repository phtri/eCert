using eCert.Models.ViewModel;
using eCert.Services;
using eCert.Utilities;
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
        private readonly CertificateServices _certificateServices;
        public TranscriptController()
        {
            _transcriptServices = new TranscriptServices();
            _certificateServices = new CertificateServices();
        }
        public ActionResult LoadListTranscript(int campusId)
        {
            EducationSystemViewModel educationSystemViewModel = _transcriptServices.GetEduSystemByCampusId(campusId);
            
            List<SubjectViewModel> subjects = null;
            //campus 

            if (educationSystemViewModel.EducationName.Equals("Đại học FPT"))
            {
                string rollNumber = Session["RollNumber"].ToString();
                //Get passed subject 
                FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
                FAP_Service.Subject[] fapPassedSubject = client.GetPassedSubjects(rollNumber);
                subjects = _transcriptServices.ConvertToListSubjectViewModel(fapPassedSubject);

                foreach (SubjectViewModel subject in subjects)
                {
                    CertificateViewModel transcriptCert = _certificateServices.GetCertificateByRollNumberAndSubjectCode(rollNumber, subject.SubjectCode);
                    //Check if transcript has already generated
                    if (transcriptCert != null)
                    {
                        subject.IsGenerated = true;
                        subject.Link = "/Certificate/FPTCertificateDetail?url=" + transcriptCert.Url;
                        subject.CampusId = campusId;
                    }
                }
            }
           
            return PartialView(subjects);
        }
        // GET: Transcript
        public ActionResult ListTranscript()
        {

            if (Session["RollNumber"] != null)
            {
                if ((bool)Session["isUpdatedEmail"])
                {
                    
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
            return View();

        }
        [HttpPost]
        public ActionResult ListTranscript(TranscriptViewModel transcriptViewModel)
        {
            //switch case to connect to service of education systems
            string rollNumber = Session["RollNumber"].ToString();
            //Get passed subject 
            FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
            FAP_Service.Subject[] fapPassedSubject = client.GetPassedSubjects(rollNumber);
            List<SubjectViewModel> subjects = _transcriptServices.ConvertToListSubjectViewModel(fapPassedSubject);

            foreach (SubjectViewModel subject in subjects)
            {
                CertificateViewModel transcriptCert = _certificateServices.GetCertificateByRollNumberAndSubjectCode(rollNumber, subject.SubjectCode);
                //Check if transcript has already generated
                if (transcriptCert != null)
                {
                    subject.IsGenerated = true;
                    subject.Link = "/Certificate/FPTCertificateDetail?url=" + transcriptCert.Url;
                }
            }
            EducationSystemViewModel educationSystemViewModel = _transcriptServices.GetEducationNameById(transcriptViewModel.EducationSystemId);
            CampusViewModel campusViewModel = _transcriptServices.GetCampusNameById(transcriptViewModel.CampusId);
            ViewBag.EduName = educationSystemViewModel.EducationName;
            ViewBag.CampusName = campusViewModel.CampusName;
            ViewBag.Subject = subjects;
            return View();
        }

      
        public JsonResult GenerateCertificate(string subjectCode, int campusId)
        {
            string rollNumber = Session["RollNumber"].ToString();
            SignatureViewModel signatureViewModel = _transcriptServices.GetSignatureOfPrincipalByCampusId(campusId);
            Result result = new Result();
            FAP_Service.UserWebServiceSoapClient client = new FAP_Service.UserWebServiceSoapClient();
            //Check passed subject with subject code and student rollnumber
            FAP_Service.Subject detailPassedSubject = client.GetDetailPassedSubject(rollNumber, subjectCode);
            SubjectViewModel subject = _transcriptServices.ConvertToSubjectViewModel(detailPassedSubject);
            subject.SignatureId = signatureViewModel.SignatureId;
            //Does not have that subject
            if (subject == null)
            {
                result.IsSuccess = false;
                result.Message = "Generate error.";
                //Error message
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            CertificateViewModel transcriptViewModel = _certificateServices.GetCertificateByRollNumberAndSubjectCode(rollNumber, subject.SubjectCode);
            //If regenerated
            if (transcriptViewModel != null)
            {
                //Delete old transcript certificate
                _certificateServices.DeleteCertificate(transcriptViewModel.Url);
            }
            _transcriptServices.GenerateCertificateForSubject(subject, rollNumber);
            //Message success
            result.IsSuccess = true;
            result.Message = "Generate certificate successfully.";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}