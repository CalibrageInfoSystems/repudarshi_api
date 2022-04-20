using log4net;
using Model;
using Model.Response;
using Repository.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Settings
{
    public class TypeCdDmtRepository : ITypeCdDmtRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ListDataResponse<TypeCdDmt> GetAllTypeCdDmts(int ClassTypeId)
    {
        ListDataResponse<TypeCdDmt> response = new ListDataResponse<TypeCdDmt>();

        try
        {
            var result = _context.TypeCdDmts.Where(x=>x.ClassTypeId== ClassTypeId).ToList();

            if (result.Count() > 0)
            {
                response.ListResult = result;
                response.IsSuccess = true;
                response.AffectedRecords = result.Count();
                response.EndUserMessage = "Get all typeCdDmts successfull";
                return response;
            }
            else
            {
                response.ListResult = null;
                response.IsSuccess = true;
                response.AffectedRecords = 0;
                response.EndUserMessage = "No data found";
                return response;
            }
        }
        catch (Exception ex)
        {
                //Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.IsSuccess = false;
            response.AffectedRecords = 0;
            response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            return response;
        }
    }
}
}
