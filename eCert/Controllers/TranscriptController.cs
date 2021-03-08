using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eCert.Controllers
{
    public class TranscriptController : Controller
    {
        // GET: Transcript
        public ActionResult ListTranscript()
        {
            return View();
        }
    }
}