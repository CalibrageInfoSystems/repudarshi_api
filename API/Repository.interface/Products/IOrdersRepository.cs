using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Products
{
    public interface IOrderRepository
    {
        ListDataResponse<GetOrdersByStoreId_Result> GetOrdersByStore(GetOrdersByStoreReq req);
        ListDataResponse<GetUserOrdersByUserId_Result> GetUserOrdersByUserId(GetUserOrdersReq req);
        ValueDataResponse<Order> PlaceOrder(PlaceOrderReq req);
        ValueDataResponse<Order> AssignOrdertoDeliveryAgent(AssignOrdertoDeliveryAgentReq req);
        ValueDataResponse<Order> UpdateOrderStatus(UpdateOrderStatusReq req);
        ListDataResponse<GetOrdersByAgentIdStoreId_Result> GetOrdersByAgentIdStoreId(GetOrdersByAgentIdStoreIdReq req);
        ListDataResponse<GetDeliverySlotResponse> GetDeliverySlot();

        ValueDataResponse<OrderStatusCountResponse> GetOrderStatusCountByAgentId(int AgentId);
        //ValueDataResponse<Order> CreateNotificaation();
    }
}
