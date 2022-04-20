using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Products
{
  public  interface IReportRepository
{
        ListDataResponse<OrdersSummaryReport_Result> OrdersSummaryReport(OrdersSummaryReportReq req);
        ValueDataResponse<GetOrderDetailsResponse> GetOrderDetails(string OrderCode);

    }
}
