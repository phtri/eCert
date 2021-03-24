using System.Collections.Generic;

namespace eCert.Utilities
{
    public class Result
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class ResultExcel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int RowCountSuccess { get; set; }
        public List<RowExcel> ListRowError {get; set;}
    }
    public class RowExcel
    {
        public int TypeError { get; set; }
        public string ColumnName { get; set; }
        public List<int> Rows { get; set; }
    }
}