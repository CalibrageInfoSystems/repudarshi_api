using Model;
using Model.Response;
using Repository.Interface.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Notification")]
    public class NotificationController : ApiController
    {
        
        INotificationRepository _repo = new Repository.Products.NotificationRepository();
        [HttpGet]
        [Route("GetCount/{userId}")]
        public ListDataResponse<Notification> GetCount(int userId)
        {//return count of latest unread notifications
            return _repo.GetNotificationsCountByUserId(userId);
        }
        [HttpGet]
        [Route("ReadAllNotifications/{userId}")]
        public ValueDataResponse<string> ReadAllNotifications(int userId)
        {//Make all unread notification to read 
            return _repo.ReadAllNotifications(userId);
        }
        [HttpGet]
        [Route("ReadNotificationById/{UserId}/{Id}")]
        public ValueDataResponse<string> ReadNotificationById(int UserId,int Id)
        {//Make all unread notification to read 
            return _repo.ReadNotificationById(UserId,Id);
        }
        [HttpPost]
        [Route("AllNotifications")]
        public ListDataResponse<Notification> AllNotifications(ViewAllNotificationsRequest request)
        {//return all notifications to mobile user based on userId and requested pagesize
            return _repo.GetAllNotificationsByUserId(request);
        }
    }
}
