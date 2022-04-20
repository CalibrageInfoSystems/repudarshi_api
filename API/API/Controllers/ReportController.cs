using log4net;
using Model;
using Model.Response;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Repository.Interface.Products;
using Repository.Products;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Reports")]
    public class ReportController : ApiController
    {
       
        IReportRepository _repo = new ReportRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        //[Authorize]
        [Route("OrdersSummaryReport")]
        public ListDataResponse<OrdersSummaryReport_Result> OrdersSummaryReport(OrdersSummaryReportReq req)
        {
            return _repo.OrdersSummaryReport(req);
        }

        [HttpGet]
        //[Authorize]
        [Route("GetOrderDetails/{OrderCode}")]
        public ValueDataResponse<GetOrderDetailsResponse> GetOrderDetails(string OrderCode)
        {
            return _repo.GetOrderDetails(OrderCode);
        }

        [Route("ExportOrdersSummaryReport")] 
        [HttpPost]
        public HttpResponseMessage ExportOrdersSummaryReport(List<OrdersSummaryReport_Result> order)
        {
            Stream stream = null;
            try
            {
                var iRowCnt = 2;
                using (ExcelPackage excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
                {
                    excelPackage.Workbook.Properties.Title = "Summary Report";

                    excelPackage.Workbook.Worksheets.Add("Summary Report");
                    var worksheet = excelPackage.Workbook.Worksheets[1];
                    using (var range = worksheet.Cells["A1:G1"])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Left.Color.SetColor(Color.Black);
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Right.Color.SetColor(Color.Black);
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Bottom.Color.SetColor(Color.Black);
                        range.Style.Font.Color.SetColor(Color.Black);
                        range.Style.Font.SetFromFont(new Font("Calibri", 12));
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.Font.Bold = true;
                    }
                    worksheet.Cells[iRowCnt - 1, 1].Value = "Store";
                    worksheet.Cells[iRowCnt - 1, 2].Value = "Order No";
                    worksheet.Cells[iRowCnt - 1, 3].Value = "Order Status";
                    worksheet.Cells[iRowCnt - 1, 4].Value = "Total Amount";
                    worksheet.Cells[iRowCnt - 1, 5].Value = "Order Date";
                    worksheet.Cells[iRowCnt - 1, 6].Value = "Delivery Agent";
                    worksheet.Cells[iRowCnt - 1, 7].Value = "Customer"; 
                    int i;
                    for (i = 0; i <= order.Count - 1; i++)
                    {
                        worksheet.Cells[iRowCnt, 1].Value = order[i].StoreName1;
                        worksheet.Cells[iRowCnt, 2].Value = order[i].Code;
                        worksheet.Cells[iRowCnt, 3].Value = order[i].OrderStatus;
                        worksheet.Cells[iRowCnt, 4].Value = order[i].TotalPrice;
                        worksheet.Cells[iRowCnt, 5].Value = order[i].CreatedDate;
                        worksheet.Cells[iRowCnt, 5].Style.Numberformat.Format = @"dd-MM-yyyy hh:mm"; 
                        worksheet.Cells[iRowCnt, 6].Value = order[i].AgentName;
                        worksheet.Cells[iRowCnt, 7].Value = order[i].CustomerName;  
                         

                        iRowCnt = iRowCnt + 1;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, excelPackage.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.InnerException.InnerException.Message);
            }
        }
    }
}
