using Model;
using Model.Response;
using Repository.Interface.Products;
using Repository.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
         
        IProductRepository _repo = new Repository.Products.ProductRepository();

        [HttpPost]
        //[Authorize]
        [Route("GetProductsByCategoryId")]
        public ListDataResponse<GetProductsByCategoryIds_Result> GetProductsByCategoryIds(GetProductsByCategoryIdReq req)
        {
            return _repo.GetProductsByCategoryIds(req);
        }
        [HttpPost] 
        [Route("GetProductsByName")]
        public ListDataResponse<GetProductsByName_Result> GetProductsByName(GetProductsByNameReq req)
        {
            return _repo.GetProductsByName(req);
        }
        [HttpGet]
        [Route("GetProductsByOrder/{OrderId}")]
        public ListDataResponse<GetProductsByOrderId_Result> GetProductsByOrder(int OrderId)
        {
            return _repo.GetProductsByOrder(OrderId);
        }
        [HttpPost]
        [Route("AddUpdateProduct")]
        public ValueDataResponse<Product> AddUpdateProduct(AddUpdateProductReq req)
        {
            return _repo.AddUpdateProduct(req);
        }
        [HttpPost]
        [Route("UpdateProductPrice")]
        public ValueDataResponse<dynamic> UpdateProductPrice()
        {
          
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            
            return _repo.UpdateProductPrice(hfc);
        }
        [HttpGet]
        [Route("GetProductTemplate")]
        public ValueDataResponse<string> GetProductTemplate()
        {  

            return _repo.GetProductTemplate();
        }
        [HttpPost]
        [Route("DeleteProduct")]
        public ValueDataResponse<Product> DeleteProduct(DeleteReq req)
        {
            return _repo.DeleteProduct(req);
        }
    }
}
