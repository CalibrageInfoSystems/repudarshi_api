using log4net;
using Model;
using Model.Response;
using Repository.Interface.Products;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Products
{
   public class StoreRepository: IStoreRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities.FileRepository repo = new Utilities.FileRepository();
        string FileRepositoryURL = ConfigurationManager.AppSettings["FileRepositoryUrl"].ToString();
        string FileRepositoryFolder = ConfigurationManager.AppSettings["FileRepositoryFolder"].ToString();
        string ServerRootPath = ConfigurationManager.AppSettings["ServerRootPath"].ToString();

        public ListDataResponse<GetAllStores_Result> GetAllStores(int? Id, bool? IsActive)
        {
            ListDataResponse<GetAllStores_Result> response = new ListDataResponse<GetAllStores_Result>();
            try
            {
                var result = _context.GetAllStores(Id, IsActive).ToList();
                if (result != null)
                {
                    foreach (var res in result)
                    {
                        res.Filepath = res.FileName != null ? string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, res.Filepath) : null;
                    }
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Stores Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Store Found";
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
        public ListDataResponse<GetUserStoresByUserId_Result> GetUserStoresByUser(int  UserId)
        {
            ListDataResponse<GetUserStoresByUserId_Result> response = new ListDataResponse<GetUserStoresByUserId_Result>();
            try
            {
                var result = _context.GetUserStoresByUserId(UserId).ToList();
                if (result != null)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Stores Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Store Found";
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
        public ValueDataResponse<Store> AddUpdateStore(AddUpdateStoreReq req)
        {
            ValueDataResponse<Store> response = new ValueDataResponse<Store>();
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
                    string ModuleName = "Store";
                    var now = DateTime.UtcNow;
                    var yearName = now.ToString("yyyy");
                    var monthName = now.Month.ToString("d2");
                    var dayName = now.ToString("dd");
                    string Location = Path.Combine(ServerRootPath, FileRepositoryFolder, yearName, monthName, dayName);

                    byte[] FileBytes = req.FileBytes;
                    req.FileName =repo.UploadFile(ModuleName, FileBytes, req.FileExtension, Location);
                    req.FileLocation = Path.Combine(yearName, monthName, dayName, ModuleName);
                }
                var result = _context.AddUpdateStore(req.Id, req.Name1, req.Name2, req.FileName,req.FileLocation,req.FileExtension,
                    req.Address,req.LandMark,req.CityName,req.PostelCode, req.IsActive,
                req.CreatedByUserId, CreatedDate, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);
                if (sc > 0)
                {
                    response.Result = _context.Stores.Where(x => x.Id == sc).FirstOrDefault();
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
        public ValueDataResponse<Store> DeleteStore(DeleteReq req)
        {
            ValueDataResponse<Store> response = new ValueDataResponse<Store>();
            try
            {
                int sc = 0;
                var result = _context.Stores.Where(x => x.Id == req.Id).FirstOrDefault();
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
                    response.EndUserMessage = "Store Deleted Successfully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Store Found";
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
