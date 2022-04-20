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
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        
        ICategoryRepository _repo = new CategoryRepository();

        [HttpGet]
        //[Authorize]
        [Route("GetAllCategories/{Id}/{IsActive}")]
        public ListDataResponse<GetAllCategories_Result> GetAllCategories(int? Id,bool? IsActive)
        {
            return _repo.GetAllCategories(Id,IsActive);
        }
        [HttpGet]
        //[Authorize]
        [Route("GetCategoriesByParentCategory/{ParentCategoryId}")]
        public ListDataResponse<GetCategoriesByParentCategoryId_Result> GetCategoriesByParentCategory(int? ParentCategoryId)
        {
            return _repo.GetCategoriesByParentCategoryId(ParentCategoryId); 
        }

        [HttpPost]
        [Route("AddUpdateCategory")]
        public ValueDataResponse<Category> AddUpdateCategory(AddUpdateCategoryReq req)
        {
            return _repo.AddUpdateCategory(req);
        }

        [HttpPost]
        [Route("DeleteCategory")]
        public ValueDataResponse<Category> DeleteCategory(DeleteReq req)
        {
            return _repo.DeleteCategory(req);
        }
    }
}
