using log4net;
using Model;
using Model.Response;
using Repository.Interface.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Products
{
    public class ReportRepository: IReportRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ListDataResponse<OrdersSummaryReport_Result> OrdersSummaryReport(OrdersSummaryReportReq req)
        {
            ListDataResponse<OrdersSummaryReport_Result> response = new ListDataResponse<OrdersSummaryReport_Result>();
            try
            {
                DateTime FromDate = req.FromDate.Value;
                FromDate = TimeZoneInfo.ConvertTimeToUtc(FromDate);
                FromDate = new DateTime(FromDate.Ticks, DateTimeKind.Utc);
                FromDate = TimeZoneInfo.ConvertTimeFromUtc(FromDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                DateTime ToDate = req.ToDate.Value;
                ToDate = TimeZoneInfo.ConvertTimeToUtc(ToDate);
                ToDate = new DateTime(ToDate.Ticks, DateTimeKind.Utc);
                ToDate = TimeZoneInfo.ConvertTimeFromUtc(ToDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                var result = _context.OrdersSummaryReport(req.AgentId,req.CustomerId, req.StoreId, req.StatusTypeId, FromDate, ToDate).ToList();
                if (result.Count > 0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime(); 
                    }
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Orders Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = true;
                    response.EndUserMessage = "No Orders Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<GetOrderDetailsResponse> GetOrderDetails(string OrderCode)
        {
            ValueDataResponse<GetOrderDetailsResponse> response = new ValueDataResponse<GetOrderDetailsResponse>();
            try
            {
              
                var result = _context.GetOrderDetailsReport(OrderCode).FirstOrDefault();
                if (result!=null)
                {

                    response.Result = new GetOrderDetailsResponse
                    {
                        OrdeDetails = result,
                        ProductDetails = _context.GetProductsByOrderId(result.Id).ToList()
                    };
                    response.AffectedRecords =1;
                    response.IsSuccess = true;
                    response.EndUserMessage = "Orders Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = true;
                    response.EndUserMessage = "No Orders Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
    }
}
