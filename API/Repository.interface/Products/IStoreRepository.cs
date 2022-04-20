using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Products
{
    public interface IStoreRepository
{
        ListDataResponse<GetAllStores_Result> GetAllStores(int? Id, bool? IsActive);
        ListDataResponse<GetUserStoresByUserId_Result> GetUserStoresByUser(int UserId);
        ValueDataResponse<Store> AddUpdateStore(AddUpdateStoreReq req);
        ValueDataResponse<Store> DeleteStore(DeleteReq req);
    }
}
