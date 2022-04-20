using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    #region RequestModels
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class Filters : Pagination
    {

        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

    }
    public class GetProductsByCategoryIdReq : Filters
    {
        public string CategoryIds { get; set; }

    }
    public class ViewAllNotificationsRequest : PaginationWeb
    {
        public int UserId { get; set; }
        public bool IsPagination { get; set; }
    }
    public class GetUsersByRoleAndStoreReq
    {
        public string RoleId { get; set; }
        public int? StoreId { get; set; }
    }
    public class Pagination
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
    public class PaginationWeb
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class GetUserOrdersReq : Pagination
    {
        public int UserId { get; set; }

    }
    public class GetOrdersByStoreReq
    {
        public int UserId { get; set; }
        public int? StoreId { get; set; }
        public int? StatusTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class ProductList
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
    public class Orders
    {
        public int UserId { get; set; }
        public double TotalPrice { get; set; }
        public int StoreId { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public string TimeSlot { get; set; }
    }
    public class PlaceOrderReq
    {
        public Orders Order { get; set; }
        public List<ProductList> Products { get; set; }
    }
    public class AssignOrdertoDeliveryAgentReq
    {
        public int OrderId { get; set; }
        public int DeliveryAgentId { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class UpdateOrderStatusReq
    {
        public int OrderId { get; set; }
        public int StatusTypeId { get; set; }
        public string Comments { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class GetOrdersByAgentIdStoreIdReq
    {
        public int? DeliveryAgentId { get; set; }

        public int? StoreId { get; set; }
        public int? StatusTypeId { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

    }
    public class GetProductsByNameReq : Filters
    {
        public string SearchValue { get; set; }
        public bool? IsActive { get; set; }
    }
    public class FileRepository
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileBytes { get; set; }
    }
    public class AddUpdateProductReq : FileRepository
    {
        public int? Id { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Code { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public float Price { get; set; }
        public float? DiscountedPrice { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CategoryId { get; set; }

    }
    public class AddUpdateStoreReq : FileRepository
    {
        public int? Id { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        public string LandMark { get; set; }
        public string CityName { get; set; }
        public string PostelCode { get; set; }

        public bool IsActive { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
    public class AddUpdateCategoryReq : Category
    {
        public byte[] FileBytes { get; set; }
    }
    public class DeleteReq
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
    public class AddUpdateCartReq
    {
        public UserCartDetails cart { get; set; }
        public List<ProductDetails> productsList { get; set; }
    }
    public class OrdersSummaryReportReq
    {
        public int? AgentId { get; set; }
        public int? CustomerId { get; set; }
        public int? StoreId { get; set; }
        public int? StatusTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class GetUsersBySearchReq
    {
        public string Search { get; set; }
        public int RoleId { get; set; }
    }
    public class UpdateVendorStatus
    {
        public int VendorId { get; set; }
        public int StatusTypeId { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class VendorReq
    {
        public int?  VendorId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? StatusTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class UpdateDeviseToken
    {
        public int UserId { get; set; }
        public string DeviseToken { get; set; }

    }
    public class UserCartDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
    public class ProductDetails
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class AddUpdateRoleReq : Role
    {
        public string ActivityRightIds { get; set; }
    }
    #endregion
    #region ResponseModels


    public class LoginRes
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public GetUserInfoById_Result UserInfos { get; set; }
        public List<GetActivityRightsByRoleId_Result> activityRights { get; set; }

    }

    public class LoginResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public UserInfo UserInfos { get; set; }
    }
    public class GetDeliverySlotResponse
    {
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public List<DeliverySlot> DeliverSlot { get; set; }
    }
    public class OrderStatusCountResponse
    {
        public int? InTransit { get; set; }
        public int? Delivered { get; set; }
        public int? Cancelled { get; set; }
        public int? Received { get; set; }
        public int? CollectedFromStore { get; set; }
    }
    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        //public List<FCMResult> results { get; set; }
    }
    public class GetCartByUserResponse
    {
        public UserCart cart { get; set; }
        public List<GetProductsByUserCartId_Result> productsList { get; set; }
    }
    public class GetOrderDetailsResponse
    {
        public GetOrderDetailsReport_Result OrdeDetails { get; set; }
        public List<GetProductsByOrderId_Result> ProductDetails { get; set; }
    }

}
    #endregion

