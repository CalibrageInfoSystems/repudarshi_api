using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Products
{
    public interface INotificationRepository
{
            ListDataResponse<Notification> GetNotificationsCountByUserId(int userId);
            ValueDataResponse<string> ReadAllNotifications(int userId);
            ValueDataResponse<string> ReadNotificationById(int userId, int Id);
            ListDataResponse<Notification> GetAllNotificationsByUserId(ViewAllNotificationsRequest request);
    }
}
