using log4net;
using Model;
using Model.Response;
using Repository.Interface.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Settings
{
    public class GeoLocationRepository : IGeoLocationRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ListDataResponse<GetAllCountries_Result> GetAllCountries(int? Id, bool? IsActive)
        {
            ListDataResponse<GetAllCountries_Result> response = new ListDataResponse<GetAllCountries_Result>();
            try
            {
                var result = _context.GetAllCountries(Id, IsActive).ToList();
                if (result != null)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Countries Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Country Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ListDataResponse<GetAllCities_Result> GetAllCities(int? CountryId, int? Id, bool? IsActive)
        {
            ListDataResponse<GetAllCities_Result> response = new ListDataResponse<GetAllCities_Result>();
            try
            {
                var result = _context.GetAllCities(CountryId, Id, IsActive).ToList();
                if (result != null)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Cities Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No City Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ListDataResponse<GetAllAreas_Result> GetAllAreas(string CityName)
        {
            ListDataResponse<GetAllAreas_Result> response = new ListDataResponse<GetAllAreas_Result>();
            try
            {
                var result = _context.GetAllAreas(CityName).ToList();
                if (result != null)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Areas Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Area Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }

        public ValueDataResponse<Country> AddUpdateCountry(Country req)
        {
            ValueDataResponse<Country> response = new ValueDataResponse<Country>();
            try
            {
                DateTime CreatedDate = req.CreatedDate;

                CreatedDate = TimeZoneInfo.ConvertTimeToUtc(CreatedDate);
                CreatedDate = new DateTime(CreatedDate.Ticks, DateTimeKind.Utc);
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                DateTime UpdatedDate = req.UpdatedDate;

                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                var result = _context.AddUpdateCountry(req.Id, req.Name1, req.Name2, req.Code, req.IsActive, req.CreatedByUserId, req.CreatedDate, req.UpdatedByUserId, req.UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);

                if (sc > 0)
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = statusMessage.Value.ToString();
                    return response;
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = statusMessage.Value.ToString(); 
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<City> AddUpdateCity(City req)
        {
            ValueDataResponse<City> response = new ValueDataResponse<City>();
            try
            {
                DateTime CreatedDate = req.CreatedDate;

                CreatedDate = TimeZoneInfo.ConvertTimeToUtc(CreatedDate);
                CreatedDate = new DateTime(CreatedDate.Ticks, DateTimeKind.Utc);
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                DateTime UpdatedDate = req.UpdatedDate;

                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                var result = _context.AddUpdateCity(req.Id, req.Name1, req.Name2, req.Code,req.CountryId, req.IsActive, req.CreatedByUserId, req.CreatedDate, req.UpdatedByUserId, req.UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);

                if (sc > 0)
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = statusMessage.Value.ToString();
                    return response;
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = statusMessage.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }

        public ListDataResponse<GetAllLocations_Result> GetAllLocations(int? CityId, int? Id, bool? IsActive)
        {
            ListDataResponse<GetAllLocations_Result> response = new ListDataResponse<GetAllLocations_Result>();
            try
            {
                var result = _context.GetAllLocations(CityId, Id, IsActive).ToList();
                if (result != null)
                {
                    response.ListResult = result;
                    response.AffectedRecords = result.Count();
                    response.IsSuccess = true;
                    response.EndUserMessage = "Locations Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Location Found";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }

        public ValueDataResponse<Location> AddUpdateLocation(Location req)
        {
            ValueDataResponse<Location> response = new ValueDataResponse<Location>();
            try
            {
                DateTime CreatedDate = req.CreatedDate;

                CreatedDate = TimeZoneInfo.ConvertTimeToUtc(CreatedDate);
                CreatedDate = new DateTime(CreatedDate.Ticks, DateTimeKind.Utc);
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                DateTime UpdatedDate = req.UpdatedDate;

                UpdatedDate = TimeZoneInfo.ConvertTimeToUtc(UpdatedDate);
                UpdatedDate = new DateTime(UpdatedDate.Ticks, DateTimeKind.Utc);
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(UpdatedDate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                ObjectParameter statusCode = new ObjectParameter("StatusCode", typeof(int));
                ObjectParameter statusMessage = new ObjectParameter("StatusMessage", typeof(string));

                var result = _context.AddUpdateLocation(req.Id, req.Name1, req.Name2, req.Code, req.CityId, req.IsActive, req.CreatedByUserId, req.CreatedDate, req.UpdatedByUserId, req.UpdatedDate, statusCode, statusMessage);
                var sc = Convert.ToInt32(statusCode.Value);

                if (sc > 0)
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = statusMessage.Value.ToString();
                    return response;
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = statusMessage.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<Country> DeleteCountry(DeleteReq req)
        {
            ValueDataResponse<Country> response = new ValueDataResponse<Country>();
            try
            {
                int sc = 0;
                var result = _context.Countries.Where(x => x.Id == req.Id).FirstOrDefault();
                if (result != null)
                {
                    result.IsActive = false;
                    result.UpdatedByUserId = req.UserId;
                    result.UpdatedDate = DateTime.UtcNow;
                    sc = _context.SaveChanges();
                }

                if (sc > 0)
                {
                    response.Result = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Country Deleted Successfully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Country Found";
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<City> DeleteCity(DeleteReq req)
        {
            ValueDataResponse<City> response = new ValueDataResponse<City>();
            try
            {
                int sc = 0;
                var result = _context.Cities.Where(x => x.Id == req.Id).FirstOrDefault();
                if (result != null)
                {
                    result.IsActive = false;
                    result.UpdatedByUserId = req.UserId;
                    result.UpdatedDate = DateTime.UtcNow;
                    sc = _context.SaveChanges();
                }

                if (sc > 0)
                {
                    response.Result = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "City Deleted Successfully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No City Found";
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
        public ValueDataResponse<Location> DeleteLocation(DeleteReq req)
        {
            ValueDataResponse<Location> response = new ValueDataResponse<Location>();
            try
            {
                int sc = 0;
                var result = _context.Locations.Where(x => x.Id == req.Id).FirstOrDefault();
                if (result != null)
                {
                    result.IsActive = false;
                    result.UpdatedByUserId = req.UserId;
                    result.UpdatedDate = DateTime.UtcNow;
                    sc = _context.SaveChanges();
                }

                if (sc > 0)
                {
                    response.Result = result;
                    response.IsSuccess = true;
                    response.AffectedRecords = 1;
                    response.EndUserMessage = "Location Deleted Successfully";
                }
                else
                {
                    response.IsSuccess = true;
                    response.AffectedRecords = 0;
                    response.EndUserMessage = "No Location Found";
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message, ex);
                response.AffectedRecords = 0;
                response.IsSuccess = false;
                response.EndUserMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                response.Exception = ex;
            }
            return response;
        }
    }
}
