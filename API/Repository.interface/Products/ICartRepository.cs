using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Products
{
    public interface ICartRepository
    {
        ValueDataResponse<UserCart> AddUpdateCart(AddUpdateCartReq req);
        ValueDataResponse<GetCartByUserResponse> GetCartByUser(int UserId);
        ValueDataResponse<UserCart> DeleteUserCartByUserId(int UserId);
    }
}
