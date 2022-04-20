using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Response;
using Repository.Interface.Products;
namespace Repository.Products
{
    public class CategoryRepository : ICategoryRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities.FileRepository repo = new Utilities.FileRepository();
        string FileRepositoryURL = ConfigurationManager.AppSettings["FileRepositoryUrl"].ToString();
        string FileRepositoryFolder = ConfigurationManager.AppSettings["FileRepositoryFolder"].ToString();
        string ServerRootPath = ConfigurationManager.AppSettings["ServerRootPath"].ToString();

        public ListDataResponse<GetAllCategories_Result> GetAllCategories(int? Id, bool? IsActive)
        {
            ListDataResponse<GetAllCategories_Result> response = new ListDataResponse<GetAllCategories_Result>();
            try
            {
                var result = _context.GetAllCategories(Id,IsActive).ToList();
                if (result.Count>0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
                        res.Filepath = res.FileName != null ? string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, res.Filepath) : null;
                    }
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Categories Found";
                }
                else
                { 
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Categories Found";
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

        public ListDataResponse<GetCategoriesByParentCategoryId_Result> GetCategoriesByParentCategoryId(int? ParentCategoryId)
        {
            ListDataResponse<GetCategoriesByParentCategoryId_Result> response = new ListDataResponse<GetCategoriesByParentCategoryId_Result>();
            try
            {
                var result = _context.GetCategoriesByParentCategoryId(ParentCategoryId).ToList();
                if (result.Count>0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
                        res.Filepath = res.FileName != null ? string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, res.Filepath) : null;
                    }
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Categories Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Categories Found";
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
        public ValueDataResponse<Category> AddUpdateCategory(AddUpdateCategoryReq req)
        {
            ValueDataResponse<Category> response = new ValueDataResponse<Category>();
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
                    string ModuleName = "Category";
                    var now = DateTime.UtcNow;
                    var yearName = now.ToString("yyyy");
                    var monthName = now.Month.ToString("d2");
                    var dayName = now.ToString("dd");
                    string Location = Path.Combine(ServerRootPath, FileRepositoryFolder, yearName, monthName, dayName);

                    byte[] FileBytes = req.FileBytes;
                    req.FileName = repo.UploadFile(ModuleName, FileBytes, req.FileExtention, Location);
                    req.FileLocation = Path.Combine(yearName, monthName, dayName, ModuleName);
                }
                var result = _context.AddUpdateCategory(req.Id, req.Name1, req.Name2, req.ParentCategoryId, req.CategoryLevel,
                   req.FileName, req.FileLocation, req.FileExtention,  req.IsActive,
                req.CreatedByUserId, CreatedDate, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);
                if (sc > 0)
                {
                    response.Result = _context.Categories.Where(x => x.Id == sc).FirstOrDefault();
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

        public ValueDataResponse<Category> DeleteCategory(DeleteReq req)
        {
            ValueDataResponse<Category> response = new ValueDataResponse<Category>();
            try
            {
                int sc = 0;
                var result = _context.Categories.Where(x => x.Id == req.Id).FirstOrDefault();
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
                    response.EndUserMessage = "Category Deleted Successfully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Category Found";
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
