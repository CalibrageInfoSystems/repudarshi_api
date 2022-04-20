using Model;
using Model.Response;
using Repository.Interface.Settings;
using Repository.Settings;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/GeoLocation")]
    public class GeoLocationController : ApiController
    {
        
        IGeoLocationRepository _repo = new GeoLocationRepository();

        [HttpGet]
        //[Authorize]
        [Route("GetAllCountries/{Id}/{IsActive}")]
        public ListDataResponse<GetAllCountries_Result> GetAllCountries(int? Id, bool? IsActive)
        {
            return _repo.GetAllCountries(Id, IsActive);
        }
     
        [HttpGet]
        //[Authorize]
        [Route("GetAllCities/{CountryId}/{Id}/{IsActive}")]
        public ListDataResponse<GetAllCities_Result> GetAllCities(int? CountryId,int? Id, bool? IsActive)
        {
            return _repo.GetAllCities(CountryId,Id, IsActive);
        }
        [HttpGet]
        //[Authorize]
        [Route("GetAllAreas/{CityName}")]
        public ListDataResponse<GetAllAreas_Result> GetAllAreas(string CityName)
        {
            return _repo.GetAllAreas(CityName);
        }

        [HttpPost] 
        [Route("AddUpdateCountry")]
        public ValueDataResponse<Country> AddUpdateCountry(Country req)
        {
            return _repo.AddUpdateCountry(req);
        }
        [HttpPost]
        [Route("AddUpdateCity")]
        public ValueDataResponse<City> AddUpdateCity(City req)
        {
            return _repo.AddUpdateCity(req);
        }

        [HttpGet]
        //[Authorize]
        [Route("GetAllLocations/{CityId}/{Id}/{IsActive}")]
        public ListDataResponse<GetAllLocations_Result> GetAllLocations(int? CityId, int? Id, bool? IsActive)
        {
            return _repo.GetAllLocations(CityId, Id, IsActive);
        }
        [HttpPost]
        [Route("AddUpdateLocation")]
        public ValueDataResponse<Location> AddUpdateLocation(Location req)
        {
            return _repo.AddUpdateLocation(req);
        }

        [HttpPost]
        [Route("DeleteCountry")]
        public ValueDataResponse<Country> DeleteCountry(DeleteReq req)
        {
            return _repo.DeleteCountry(req);
        }
        [HttpPost]
        [Route("DeleteCity")]
        public ValueDataResponse<City> DeleteCity(DeleteReq req)
        {
            return _repo.DeleteCity(req);
        } 
        [HttpPost]
        [Route("DeleteLocation")]
        public ValueDataResponse<Location> DeleteLocation(DeleteReq req)
        {
            return _repo.DeleteLocation(req);
        }
    }
}
