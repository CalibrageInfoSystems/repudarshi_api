using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response
{
    public class DataResponse
    {
        public bool IsSuccess { get; set; }
        public int AffectedRecords { get; set; }
        public string EndUserMessage { get; set; }
        public List<ValidationItem> ValidationErrors { get; set; }
        public Exception Exception { get; set; }

        public DataResponse()
        {
            IsSuccess = false;
            AffectedRecords = 0;
            ValidationErrors = new List<ValidationItem>();
        }
    }
}
