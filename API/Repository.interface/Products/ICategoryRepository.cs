using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Products
{
    public interface ICategoryRepository
    {
        ListDataResponse<GetAllCategories_Result> GetAllCategories(int? Id, bool? IsActive);
        ListDataResponse<GetCategoriesByParentCategoryId_Result> GetCategoriesByParentCategoryId(int? ParentCategoryId);
        ValueDataResponse<Category> AddUpdateCategory(AddUpdateCategoryReq req);
        ValueDataResponse<Category> DeleteCategory(DeleteReq req);
    }
}
