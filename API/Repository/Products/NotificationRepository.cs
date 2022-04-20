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
    public class NotificationRepository: Interface.Products.INotificationRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ListDataResponse<Notification> GetNotificationsCountByUserId(int userId)
        {
            ListDataResponse<Notification> response = new ListDataResponse<Notification>();
            try
            {
                var result = _context.Notifications.Where(n => n.UserId == userId && n.IsRead == false).ToList();

                if (result.Count() > 0)
                {
                    response.ListResult = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Get All Notifications Successfull";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Notifications Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }
        public ValueDataResponse<string> ReadAllNotifications(int userId)
        {
            ValueDataResponse<string> response = new ValueDataResponse<string>();
            try
            {
                var result = _context.Notifications.Where(n => n.UserId == userId && n.IsRead == false).ToList();
                if (result.Count() > 0)
                {
                    result.ForEach(c => { c.IsRead = true; c.UpdatedByUserId = userId; c.UpdatedDate = DateTime.UtcNow; });
                    _context.SaveChanges();
                }
                if (result.Count() > 0)
                {
                    response.Result = null;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Read All Notifications Successfull";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Notifications Found To Update";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }

        public ValueDataResponse<string> ReadNotificationById(int userId,int Id)
        {
            ValueDataResponse<string> response = new ValueDataResponse<string>();
            try
            {
                var result = _context.Notifications.Where(n => n.Id == Id && n.IsRead == false).FirstOrDefault();
                if (result!=null)
                {
                    result.IsRead = true; result.UpdatedByUserId = userId; result.UpdatedDate = DateTime.UtcNow;  
                    _context.SaveChanges();
                }
                if (result!=null)
                {
                    response.Result = null;
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Read Notifications Successfull";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Notifications Found To Update";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }
        public ListDataResponse<Notification> GetAllNotificationsByUserId(ViewAllNotificationsRequest req)
        {
            ListDataResponse<Notification> response = new ListDataResponse<Notification>();
            int totalRecords = 0;
            try
            {

                var result = _context.Notifications.Where(n => n.UserId == req.UserId).OrderByDescending(x=>x.CreatedDate).ToList();
                
                if (result.Count() > 0)
                {
                    totalRecords = result.Count();
                    response.ListResult = req.IsPagination? result.Skip(req.PageIndex * req.PageSize).Take(req.PageSize): result;
                    response.IsSuccess = true;
                    response.AffectedRecords = result.Count();
                    response.EndUserMessage = "Get All Notifications Successfull";
                }
                else
                {
                    response.IsSuccess = false;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Notifications Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
                response.AffectedRecords = 0;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }

            return response;
        }
    }
}
