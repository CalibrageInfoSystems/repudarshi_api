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
    [RoutePrefix("api/Store")]
    public class StoreController : ApiController
    {
        
        IStoreRepository _repo = new StoreRepository();

        [HttpGet]
        //[Authorize]
        [Route("GetAllStores/{Id}/{IsActive}")]
        public ListDataResponse<GetAllStores_Result> GetAllStores(int? Id, bool? IsActive)
        {
            return _repo.GetAllStores(Id, IsActive);
        }

        [HttpGet]
        //[Authorize]
        [Route("GetUserStoresByUser/{UserId}")]
        public ListDataResponse<GetUserStoresByUserId_Result> GetUserStoresByUser(int UserId)
        {
            return _repo.GetUserStoresByUser(UserId);
        }
        [HttpPost]
        [Route("AddUpdateStore")]
        public ValueDataResponse<Store> AddUpdateStore(AddUpdateStoreReq req)
        {
            return _repo.AddUpdateStore(req);
        }
        [HttpPost]
        [Route("DeleteStore")]
        public ValueDataResponse<Store> DeleteStore(DeleteReq req)
        {
            return _repo.DeleteStore(req);
        }
    }
}
