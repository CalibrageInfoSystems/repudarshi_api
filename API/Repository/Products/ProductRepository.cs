using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface.Products;
using Model.Response;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Web; 
using System.Data;
using ExcelDataReader;

namespace Repository.Products
{
    public class ProductRepository : IProductRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities.FileRepository repo = new Utilities.FileRepository();
        string FileRepositoryURL = ConfigurationManager.AppSettings["FileRepositoryUrl"].ToString();
        string FileRepositoryFolder = ConfigurationManager.AppSettings["FileRepositoryFolder"].ToString();
        string ServerRootPath = ConfigurationManager.AppSettings["ServerRootPath"].ToString();
        public ListDataResponse<GetProductsByCategoryIds_Result> GetProductsByCategoryIds(GetProductsByCategoryIdReq req)

        {
            ListDataResponse<GetProductsByCategoryIds_Result> response = new ListDataResponse<GetProductsByCategoryIds_Result>();
            try
            {
                var result = _context.GetProductsByCategoryIds(req.CategoryIds, req.PageNo, req.PageSize, req.SortColumn, req.SortOrder).ToList();
                if (result.Count > 0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
                        res.Filepath = res.FileName != null ? string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, res.Filepath) : null;
                    }
                }
                if (result.Count > 0)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();//(int) result.FirstOrDefault().MaxRows;
                    response.IsSuccess = true;
                    response.EndUserMessage = "Products Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Product Found";
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

        public ListDataResponse<GetProductsByName_Result> GetProductsByName(GetProductsByNameReq req)

        {
            ListDataResponse<GetProductsByName_Result> response = new ListDataResponse<GetProductsByName_Result>();
            try
            {
                var result = _context.GetProductsByName(req.SearchValue, req.PageNo, req.PageSize,req.IsActive, req.SortColumn, req.SortOrder).ToList();
                if (result.Count > 0)
                { 
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
                        res.Filepath = res.FileName != null ? string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, res.Filepath) : null;
                    }
                }
                if (result != null)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Products Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Product Found";
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
        public ListDataResponse<GetProductsByOrderId_Result> GetProductsByOrder(int OrderId)

        {
            ListDataResponse<GetProductsByOrderId_Result> response = new ListDataResponse<GetProductsByOrderId_Result>();
            try
            {
                var result = _context.GetProductsByOrderId(OrderId).ToList();

                if (result.Count>0)
                {  
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Products Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Product Found";
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


        public  ValueDataResponse<Product> AddUpdateProduct(AddUpdateProductReq req)
        {
            ValueDataResponse<Product> response = new ValueDataResponse<Product>();
            try
            {
                DateTime CreatedDate = req.CreatedDate;
                CreatedDate = TimeZoneInfo.ConvertTimeToUtc(CreatedDate);
                CreatedDate = new DateTime(CreatedDate.Ticks, DateTimeKind.Utc);
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                DateTime UpdatedDate = req.UpdatedDate;
                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));
                if (req.FileBytes != null)
                {
                    string ModuleName = "Products";
                    var now = DateTime.UtcNow;
                    var yearName = now.ToString("yyyy");
                    var monthName = now.Month.ToString("d2");
                    var dayName = now.ToString("dd");
                    string Location = Path.Combine(ServerRootPath, FileRepositoryFolder, yearName, monthName, dayName);

                    byte[] FileBytes = req.FileBytes;
                    req.FileName = repo.UploadFile(ModuleName, FileBytes, req.FileExtension, Location);
                    req.FileLocation = Path.Combine(yearName, monthName, dayName, ModuleName);
                }
                var result = _context.AddUpdateProduct(req.Id, req.Name1, req.Name2, req.Code, req.Description1, req.Description2, req.Price, req.DiscountedPrice, req.IsActive,
                req.CreatedByUserId, CreatedDate, req.UpdatedByUserId, UpdatedDate,
                req.CategoryId, req.FileName, req.FileLocation, req.FileExtension, statusCode, statusMessage);
                    var sc = Convert.ToInt32(statusCode.Value);
                    if (sc > 0)
                    {
                        response.Result = _context.Products.Where(x => x.Id == sc).FirstOrDefault();
                        response.IsSuccess = true;
                        response.AffectedRecords = 1;
                        response.EndUserMessage = statusMessage.Value.ToString();
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.AffectedRecords = 0;
                        response.EndUserMessage = statusMessage.Value.ToString();
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
        public ValueDataResponse<dynamic> UpdateProductPrice(HttpFileCollection fileCollection)
        {
            ValueDataResponse < dynamic > response= new ValueDataResponse<dynamic>();
            try {
                int size = 0;
                string message = null;
                byte[] filebytes = null;
                string filename = null;
                HttpPostedFile file = fileCollection[0];
                size = file.ContentLength;

                if (file.ContentLength > 0)
                {

                    Stream stream = file.InputStream;
                    int UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    IExcelDataReader reader = null;

                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        message = "This file format is not supported";
                    }

                    DataSet excelRecords = reader.AsDataSet();
                    reader.Close();

                    var finalRecords = excelRecords.Tables[0];
                    for (int i = 1; i < finalRecords.Rows.Count; i++)
                    {
                        string Code = finalRecords.Rows[i][0].ToString();
                        Product product = _context.Products.Where(p => p.Code == Code).FirstOrDefault();
                        if (product != null)
                        {
                            product.Price =Convert.ToDouble( finalRecords.Rows[i][1].ToString());
                            product.DiscountedPrice = Convert.ToDouble(finalRecords.Rows[i][2].ToString());
                            product.UpdatedByUserId = UserId;
                            product.UpdatedDate = DateTime.UtcNow; 
                        } 
                    }

                    int output = _context.SaveChanges();
                    if (output >= 0)
                    {
                        response.AffectedRecords = 0;
                        response.IsSuccess = true;
                        response.EndUserMessage = output==0? "product code doesn't match in the documents !" : "Excel file has been successfully uploaded"; 
                    }
                    else
                    {
                       
                        response.AffectedRecords = 0;
                        response.IsSuccess = false;
                        response.EndUserMessage = "Excel file uploaded has failed"; 
                    }
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

        public ValueDataResponse<string> GetProductTemplate()

        {
            ValueDataResponse<string> response = new ValueDataResponse<string>();
            try
            {
                var templateUrl = string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, "BulkUpload//ProductTemplate.xlsx");


                if (templateUrl != null)
                {
                    response.Result = templateUrl;
                    response.AffectedRecords = 1;
                    response.IsSuccess = true;
                    response.EndUserMessage = "Template URL Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "Template URL Not Found";
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
        public ValueDataResponse<Product> DeleteProduct(DeleteReq req)
        {
            ValueDataResponse<Product> response = new ValueDataResponse<Product>();
            try
            {
                int sc = 0;
                var result = _context.Products.Where(x => x.Id == req.Id).FirstOrDefault();
                if (result != null)
                {
                    result.IsActive = false;
                    result.UpdatedByUserId = req.UserId;
                    result.UpdatedDate = DateTime.UtcNow;
                    sc = _context.SaveChanges();
                }

                if (sc > 0)
                {
                    response.Result = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Product Deleted Successfully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Product Found";
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

        //public string UploadFile(string ModuleName, byte[] Bytes, string Extension, string FolderLocation)
        //{
        //    try
        //    {
        //        var now = DateTime.UtcNow;
        //        //var yearName = now.Year.ToString(); now.ToString("yyyy");
        //        //var monthName = now.Month.ToString();
        //        //var dayName = now.Day.ToString();
        //        string FileName = DateTime.UtcNow.ToString("yyyyMMddhhmmssfff");

        //        var FilePath = Path.Combine(FolderLocation, ModuleName);

        //        //FolderLocation += "FolderLocation/";
        //        if (!Directory.Exists(FilePath))
        //        {
        //            Directory.CreateDirectory(FilePath);
        //        }

        //        //byte[] ByteArray = null;

        //        if (Bytes == null)
        //            return null;
        //        else
        //        {
        //            var testFile = Path.Combine(FilePath, FileName + Extension);
        //            File.WriteAllBytes(testFile, Bytes);
        //            return FileName;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        // throw;
        //        return ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        //    }
        //}
    }
}
