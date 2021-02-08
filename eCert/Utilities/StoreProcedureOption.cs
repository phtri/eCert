using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace eCert.Utilities
{
    public class StoreProcedureOption
    {
        public string ProcedureName { get; set; }
        public List<SqlParameter> Parameters { get; set; }
    }
}