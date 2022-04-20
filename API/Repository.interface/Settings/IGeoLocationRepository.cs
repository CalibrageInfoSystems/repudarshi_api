using Model;
using Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface.Settings
{
    public interface IGeoLocationRepository
{
        ListDataResponse<GetAllCountries_Result> GetAllCountries(int? Id, bool? IsActive);
        ListDataResponse<GetAllCities_Result> GetAllCities(int? CountryId, int? Id, bool? IsActive);
        ListDataResponse<GetAllAreas_Result> GetAllAreas(string CityName);
        ValueDataResponse<Country> AddUpdateCountry(Country req);
        ValueDataResponse<City> AddUpdateCity(City req);
        ListDataResponse<GetAllLocations_Result> GetAllLocations(int? CityId, int? Id, bool? IsActive);
        ValueDataResponse<Location> AddUpdateLocation(Location req);
        ValueDataResponse<Country> DeleteCountry(DeleteReq req);
        ValueDataResponse<City> DeleteCity(DeleteReq req);
        ValueDataResponse<Location> DeleteLocation(DeleteReq req);
}
}
