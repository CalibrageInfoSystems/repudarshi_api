using Model;
using Model.Response;
using Repository.Interface.Products;
using Repository.Products;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        public IOrderRepository _repo = new OrderRepository();

        [HttpPost]
        [Route("GetOrdersByStore")]
        public ListDataResponse<GetOrdersByStoreId_Result> GetOrdersByStore(GetOrdersByStoreReq req)
        {
            return _repo.GetOrdersByStore(req);
        }
        [HttpPost]
        [Route("GetUserOrders")]
        public ListDataResponse<GetUserOrdersByUserId_Result> GetUserOrdersByUserId(GetUserOrdersReq req)
        {
            return _repo.GetUserOrdersByUserId(req);
        }
        [HttpPost]
        [Route("PlaceOrder")]
        public ValueDataResponse<Order> PlaceOrder(PlaceOrderReq req)
        {
            return _repo.PlaceOrder(req);
        }

        [HttpPost]
        [Route("AssignOrdertoDeliveryAgent")]
        public ValueDataResponse<Order> AssignOrdertoDeliveryAgent(AssignOrdertoDeliveryAgentReq req)
        {
            return _repo.AssignOrdertoDeliveryAgent(req);
        }
        [HttpPost]
        [Route("UpdateOrderStatus")]
        public ValueDataResponse<Order> UpdateOrderStatus(UpdateOrderStatusReq req)
        {
            return _repo.UpdateOrderStatus(req);
        }
        [HttpPost]
        [Route("GetOrdersByAgentIdStoreId")]
        public ListDataResponse<GetOrdersByAgentIdStoreId_Result> GetOrdersByAgentIdStoreId(GetOrdersByAgentIdStoreIdReq req)
        {
            return _repo.GetOrdersByAgentIdStoreId(req);
        }
        [HttpGet]
        [Route("GetDeliverySlot")]
        public ListDataResponse<GetDeliverySlotResponse> GetDeliverySlot()
        {
            return _repo.GetDeliverySlot();
        }
        [HttpGet]
        [Route("GetOrderStatusCountById/{AgentId}")]
        public ValueDataResponse<OrderStatusCountResponse> GetOrderStatusCountById(int AgentId)
        {
            return _repo.GetOrderStatusCountByAgentId(AgentId);
        }

        //[HttpGet]
        //[Route("CreateNotificaation")]
        //public ValueDataResponse<Order> CreateNotificaation()
        //{
        //    return _repo.CreateNotificaation();
        //}

    }
}
