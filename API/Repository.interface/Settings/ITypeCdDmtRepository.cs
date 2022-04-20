using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Settings
{
    public interface ITypeCdDmtRepository
    {
        ListDataResponse<TypeCdDmt> GetAllTypeCdDmts(int ClassTypeId);
    }
}
