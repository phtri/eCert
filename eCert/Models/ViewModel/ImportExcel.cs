using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eCert.Models.ViewModel
{
    public class ImportExcel
    {
        [Required(ErrorMessage = "Please select file")]
        [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file")]
        public HttpPostedFileBase File { get; set; }
        [CampusValidation(ErrorMessage = "Please select Campus")]
        public int CampusId { get; set; }
        [CampusValidation(ErrorMessage = "Please select Signature")]
        public int SignatureId { get; set; }
    }
}