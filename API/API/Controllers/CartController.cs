
using Model;
using Model.Response;
using Repository.Interface.Products;
using Repository.Products;
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
    [RoutePrefix("api/Cart")]
    public class CartController : ApiController
    {
        

        ICartRepository _repo = new CartRepository();

        [HttpPost]
        //[Authorize]
        [Route("AddUpdateCart")]
        public ValueDataResponse<UserCart> AddUpdateCart(AddUpdateCartReq req)
        {
            return _repo.AddUpdateCart(req);
        }

        [HttpGet]
        //[Authorize]
        [Route("GetCartByUserId/{UserId}")]
        public ValueDataResponse<GetCartByUserResponse> GetCartByUser(int UserId)
        {
            return _repo.GetCartByUser(UserId);
        }

        [HttpGet]
        //[Authorize]
        [Route("DeleteCartByUserId/{UserId}")]
        public ValueDataResponse<UserCart> DeleteUserCart(int UserId)
        {
            return _repo.DeleteUserCartByUserId(UserId);
        }
    }
}
