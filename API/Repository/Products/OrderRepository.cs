using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Model;
using Model.Response;
using Repository.Interface.Products;
using Nancy.Json; 

namespace Repository.Products
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        RupdarshiEntities _context = new RupdarshiEntities();
        string NotificationServerKey = ConfigurationManager.AppSettings["ServerKey"].ToString();
 
        public ListDataResponse<GetOrdersByStoreId_Result> GetOrdersByStore(GetOrdersByStoreReq req)
        {
            ListDataResponse<GetOrdersByStoreId_Result> response = new ListDataResponse<GetOrdersByStoreId_Result>();
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

                var result = _context.GetOrdersByStoreId(req.UserId, req.StoreId, req.StatusTypeId, FromDate, ToDate).ToList();
                if (result.Count > 0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
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
        public ListDataResponse<GetUserOrdersByUserId_Result> GetUserOrdersByUserId(GetUserOrdersReq req)
        {
            ListDataResponse<GetUserOrdersByUserId_Result> response = new ListDataResponse<GetUserOrdersByUserId_Result>();
            try
            {
                var result = _context.GetUserOrdersByUserId(req.UserId, req.PageNo, req.PageSize).ToList();
                if (result.Count > 0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
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
        public ValueDataResponse<Order> PlaceOrder(PlaceOrderReq req)
        {
            ValueDataResponse<Order> response = new ValueDataResponse<Order>();
            try
            {
                Order order = new Order();
                if (req.Order != null)
                {
                    order.Code = "ORD" + DateTime.UtcNow.ToString("ddMMyyhhmmssfff") + req.Order.UserId;
                    order.UserId = req.Order.UserId;
                    order.TotalPrice = req.Order.TotalPrice;
                    order.StoreId = req.Order.StoreId;
                    order.Address = req.Order.Address;
                    order.Landmark = req.Order.Landmark;
                    order.CityName = req.Order.CityName;
                    order.PostalCode = req.Order.PostalCode;
                    order.DeliveryDate = req.Order.DeliveryDate;
                    order.TimeSlot = req.Order.TimeSlot;
                    order.StatusTypeId = (int)OrderStatus.Picking;
                    order.CreatedByUserId = req.Order.UserId;
                    order.CreatedDate = DateTime.UtcNow;
                    order.UpdatedByUserId = req.Order.UserId;
                    order.UpdatedDate = DateTime.UtcNow;
                    _context.Orders.Add(order);
                    var OrderId = _context.SaveChanges();
                    if (req.Products.Count > 0 && OrderId > 0)
                    {
                        foreach (var prod in req.Products)
                        {
                            _context.OrderProductXrefs.Add(new OrderProductXref
                            {
                                OrderId = order.Id,
                                ProductId = prod.ProductId,
                                Price = prod.Price,
                                Quantity = prod.Quantity
                            });
                        }
                    }
                    else
                    {
                        var orderDel = _context.Orders.Where(x => x.Id == order.Id).FirstOrDefault();
                        if (orderDel != null)
                        {
                            _context.Orders.Remove(orderDel);
                        }
                        response.AffectedRecords = 0;
                        response.IsSuccess = false;
                        response.EndUserMessage = "Products Cannot be empty";
                    }
                    var result = _context.SaveChanges();

                    if ((int)result > 0)
                    {
                        sendNotifications(order);
                        response.Result = order;
                        response.AffectedRecords = 1;
                        response.IsSuccess = true;
                        response.EndUserMessage = "Orders Placed Successfully";
                    }
                    else
                    {
                        response.AffectedRecords = 0;
                        response.IsSuccess = false;
                        response.EndUserMessage = "Orders Failed";
                    }
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "Orders Cannot be empty";
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


        public ValueDataResponse<Order> AssignOrdertoDeliveryAgent(AssignOrdertoDeliveryAgentReq req)
        {
            ValueDataResponse<Order> response = new ValueDataResponse<Order>();
            try
            {

                DateTime UpdatedDate = req.UpdatedDate;

                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                var result = _context.AssignOrdertoDeliveryAgent(req.OrderId, req.DeliveryAgentId, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);

                if (sc > 0)
                {
                    response.Result = _context.Orders.Where(x => x.Id == req.OrderId).FirstOrDefault();
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = statusMessage.Value.ToString();
                    return response;
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
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

        public ValueDataResponse<Order> UpdateOrderStatus(UpdateOrderStatusReq req)
        {
            ValueDataResponse<Order> response = new ValueDataResponse<Order>();
            try
            {

                DateTime UpdatedDate = req.UpdatedDate;

                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                var result = _context.UpdateOrderStatus(req.OrderId, req.StatusTypeId, req.Comments, req.UpdatedByUserId, UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);

                if (sc > 0)
                {
                    response.Result = _context.Orders.Where(x => x.Id == req.OrderId).FirstOrDefault();
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = statusMessage.Value.ToString();
                    return response;
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
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

        public ListDataResponse<GetOrdersByAgentIdStoreId_Result> GetOrdersByAgentIdStoreId(GetOrdersByAgentIdStoreIdReq req)
        {
            ListDataResponse<GetOrdersByAgentIdStoreId_Result> response = new ListDataResponse<GetOrdersByAgentIdStoreId_Result>();
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
                var result = _context.GetOrdersByAgentIdStoreId(req.DeliveryAgentId, req.StoreId, req.StatusTypeId, FromDate, ToDate).ToList();
                if (result.Count > 0)
                {
                    foreach (var res in result)
                    {
                        res.CreatedDate = res.CreatedDate.ToLocalTime();
                        res.UpdatedDate = res.UpdatedDate.ToLocalTime();
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

        public ListDataResponse<GetDeliverySlotResponse> GetDeliverySlot()
        {
            ListDataResponse<GetDeliverySlotResponse> response = new ListDataResponse<GetDeliverySlotResponse>();
            try
            {
                List<GetDeliverySlotResponse> deliverySlot = new List<GetDeliverySlotResponse>();
                int days = 7;
                for (int i = 0; i < days; i++)
                {
                    var currentDay = DateTime.UtcNow.Date;
                    string deliverySlotDay = currentDay.AddDays(i).DayOfWeek.ToString().ToUpper();
                    deliverySlot.Add(new GetDeliverySlotResponse
                    {
                        Date = currentDay.AddDays(i).Date,
                        Day = deliverySlotDay,
                        DeliverSlot = _context.DeliverySlots.Where(x => x.DayName == deliverySlotDay).OrderBy(x => x.Id).ToList()
                    });
                }
                var currentDaySlot = deliverySlot[0];
                if (currentDaySlot != null)
                {
                    var Time = TimeSpan.Parse(DateTime.UtcNow.ToLocalTime().ToString("HH:ss"));
                    foreach (var res in currentDaySlot.DeliverSlot)
                    {
                        var slot = TimeSpan.Parse(res.Slot.Split('-').First());
                        if (Time >= slot)
                        {
                            res.IsActive = false;
                        }
                    }
                    deliverySlot[0] = currentDaySlot;
                }
                response.ListResult = deliverySlot;
                response.AffectedRecords = deliverySlot.Count();
                response.IsSuccess = true;
                response.EndUserMessage = "Delivery Slot Found";

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


        public ValueDataResponse<OrderStatusCountResponse> GetOrderStatusCountByAgentId(int AgentId)
        {
            ValueDataResponse<OrderStatusCountResponse> response = new ValueDataResponse<OrderStatusCountResponse>();
            try
            {

                var result = _context.Orders.Where(x => x.DeliveryAgentId == AgentId).ToList();
                
                if (result.Count > 0)
                {
                    response.Result = new OrderStatusCountResponse
                    {
                        InTransit=result.Where(x=>x.StatusTypeId==(int)OrderStatus.InTransit).ToList().Count(),
                        Delivered= result.Where(x => x.StatusTypeId == (int)OrderStatus.Delivered).ToList().Count(),
                        Cancelled= result.Where(x => x.StatusTypeId == (int)OrderStatus.Cancelled).ToList().Count(),
                        Received= result.Where(x => x.StatusTypeId == (int)OrderStatus.Received).ToList().Count(),
                        CollectedFromStore= result.Where(x => x.StatusTypeId == (int)OrderStatus.CollectedFromStore).ToList().Count(),
                    };
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Orders Found";
                    return response;
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
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
        //public ValueDataResponse<Order> CreateNotificaation()
        //{
        //    ValueDataResponse<Order> response = new ValueDataResponse<Order>();
        //    var Order = _context.Orders.Where(x => x.Id == 46).FirstOrDefault();
        //      sendNotifications(Order);
        //    response.Result = Order;
        //    return response;
        //}
        public async Task<string> sendNotifications(Order order)
        {
            try {
                var RoleId = 1;//(int) Roles.DeliveryBoy;
            var agentsList = _context.GetUsersByRoleIdStoreId(RoleId.ToString(), order.StoreId).ToList();
            if (agentsList.Count > 0)
            {
                var text = String.Format(@" <b> Order Received </b><br>
            Store:{0} <br>
            Order No: {1} <br>
            Order Total: {2} SR <br>
            Customer Area:{3} <br>", agentsList[0].StoreName1, order.Code, order.TotalPrice, order.Address + "," + order.Landmark + "," + order.CityName + "," + order.PostalCode);
                    var fireBaseText = String.Format(@" Order Received \n
            Store:{0} \n
            Order No: {1} \n
            Order Total: {2} SR\n
            Customer Area:{3} ", agentsList[0].StoreName1, order.Code, order.TotalPrice, order.Address + "," + order.Landmark + "," + order.CityName + "," + order.PostalCode);

                    //var result= await _context.Orders.ToList();
                    List<Notification> notifications = new List<Notification>();
                foreach (var agent in agentsList)
                {
                    notifications.Add(new Notification
                    {
                        OrderId = order.Id,
                        UserId = agent.Id,
                        Desc = text,
                        IsRead = false,
                        NotificationTypeId = 7,
                        IsActive = true,
                        CreatedByUserId = order.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedByUserId = order.CreatedByUserId,
                        UpdatedDate = DateTime.UtcNow
                    }); 
                }
                _context.Notifications.AddRange(notifications);
                    var Id = _context.SaveChanges();
                   var Text= text.Replace(System.Environment.NewLine, "<br>");
                    //           System.Text.RegularExpressions.Regex regex =
                    //new System.Text.RegularExpressions.Regex(@"(<br />|<br/>|</ br>|</br>)");
                    //           // Replace new line with <br/> tag   
                    //           //Replace Html Tag with empty
                    //           System.Text.RegularExpressions.Regex regexText =
                    //new System.Text.RegularExpressions.Regex(@"(<p>|<p/>|<div>|</div>|<u>|</u>|<b>|</b>)");
                    //           regexText.Replace(text, "\r\n");
                    //           var  newText = regex.Replace(text, "\r\n");
                    if (Id > 0)
                {
                    await sendFireBaseNotifications(agentsList.Where(x=>x.AccessToken!=null).Select(x=>x.AccessToken).ToList(), fireBaseText);
                }

            }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                var x = ex;
            }
            return "";
        }

        public async Task<FCMResponse> sendFireBaseNotifications(List<string> deviceTokens, string text)
        {
            ////ReqResponse result = new ReqResponse();
            FCMResponse response = new FCMResponse();

            try
            {

                var applicationID = NotificationServerKey; //"AAAAW6E0hLc:APA91bGWLVWcwEeqjvvcqdM47wqj2E-9zAddkbMZ8geDCHbUm2fMTkXH1AR1IZEe4LtzGuAQBoa_IJ6PdD-0gj3r0y5nJJQuCUG6xHNHmcDMcPpG-PBxQT4AFt-gaK2ao5lYyyCwPy36";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send"); 
                tRequest.Method = "post"; 
                tRequest.ContentType = "application/json"; 
                var data = new 
                { 
                    registration_ids = deviceTokens, 
                    notification = new 
                    { 
                        body = text,
                        title = text,
                        icon = "myicon",
                        click_action = "FLUTTER_NOTIFICATION_CLICK"
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                { 
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {                    
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                string str = ex.Message;
            }
            return response;
        }
    }
}
