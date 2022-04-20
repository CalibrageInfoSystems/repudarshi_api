using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Repository.Interface.Products
{
    public interface IProductRepository
    {
        ListDataResponse<GetProductsByCategoryIds_Result> GetProductsByCategoryIds(GetProductsByCategoryIdReq req);
        ListDataResponse<GetProductsByName_Result> GetProductsByName(GetProductsByNameReq req);
        ListDataResponse<GetProductsByOrderId_Result> GetProductsByOrder(int OrderId);
        ValueDataResponse<Product> AddUpdateProduct(AddUpdateProductReq req);
        ValueDataResponse<dynamic> UpdateProductPrice(HttpFileCollection file);
        ValueDataResponse<string> GetProductTemplate();
        ValueDataResponse<Product> DeleteProduct(DeleteReq req);
    }
}
