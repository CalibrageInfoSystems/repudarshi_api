using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response
{
    public class ListDataResponse<T> : DataResponse
    {
        public IEnumerable<T> ListResult { get; set; }

    }
}
