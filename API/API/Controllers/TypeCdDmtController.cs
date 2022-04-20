using Model;
using Model.Response;
using Repository.Interface.Settings;
using Repository.Settings;
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
    public class TypeCdDmtController : ApiController
    {
         
        ITypeCdDmtRepository _repo = new TypeCdDmtRepository();

        [HttpGet]
        //[Authorize]
        public ListDataResponse<TypeCdDmt> GetAllTypeCdDmts(int Id)
        {
            return _repo.GetAllTypeCdDmts(Id);
        }
    }
}
