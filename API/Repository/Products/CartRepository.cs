using log4net;
using Model;
using Repository.Interface.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface.Products;
using Model.Response;
using System.Configuration;

namespace Repository.Products
{
    public class CartRepository : ICartRepository
    {
        RupdarshiEntities _context = new RupdarshiEntities();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string FileRepositoryURL = ConfigurationManager.AppSettings["FileRepositoryUrl"].ToString();
        string FileRepositoryFolder = ConfigurationManager.AppSettings["FileRepositoryFolder"].ToString();
        string ServerRootPath = ConfigurationManager.AppSettings["ServerRootPath"].ToString();
        public ValueDataResponse<UserCart> AddUpdateCart(AddUpdateCartReq req)
        {
            ValueDataResponse<UserCart> response = new ValueDataResponse<UserCart>();
            try
            {
                UserCart cart = new UserCart();
                if (req.cart != null)
                {
                    if (req.cart.Id == 0)
                    {
                        var userCart = _context.UserCarts.Where(x => x.UserId == req.cart.UserId).FirstOrDefault();
                        if (userCart == null)
                        {
                            cart.UserId = req.cart.UserId;
                            cart.Name = req.cart.Name;
                            cart.CreatedDate = DateTime.UtcNow;
                            cart.UpdatedDate = DateTime.UtcNow;
                            _context.UserCarts.Add(cart);
                            var cartId = _context.SaveChanges();
                            if (req.productsList.Count > 0 && cartId > 0)
                            {
                                foreach (var prod in req.productsList)
                                {
                                    _context.UserCartProductXrefs.Add(new UserCartProductXref
                                    {
                                        UserCartId = cart.Id,
                                        ProductId = prod.ProductId,
                                        Quantity = prod.Quantity
                                    });
                                }
                            }
                            else
                            {
                                var cartDel = _context.UserCarts.Where(x => x.Id == cart.Id).FirstOrDefault();
                                if (cartDel != null)
                                {
                                    _context.UserCarts.Remove(cartDel);
                                }
                                response.AffectedRecords = 0;
                                response.IsSuccess = false;
                                response.EndUserMessage = "Cart Products Cannot be empty";
                            }
                            var result = _context.SaveChanges();

                            if ((int)result > 0)
                            {
                                response.Result = cart;
                                response.AffectedRecords = 1;
                                response.IsSuccess = true;
                                response.EndUserMessage = "Cart Added or Updated Successfully";
                            }
                            else
                            {
                                response.AffectedRecords = 0;
                                response.IsSuccess = false;
                                response.EndUserMessage = "Cart  Failed";
                            }
                        }
                        else
                        {
                            response.AffectedRecords = 0;
                            response.IsSuccess = false;
                            response.EndUserMessage = "cart already exists to you";
                        }
                    }
                    else
                    {
                        var cartDetails = _context.UserCarts.Where(x => x.Id == req.cart.Id).FirstOrDefault();
                        cartDetails.UpdatedDate = DateTime.UtcNow;
                        var removeCartProducts = _context.UserCartProductXrefs.Where(x => x.UserCartId == cartDetails.Id).ToList();
                        _context.UserCartProductXrefs.RemoveRange(removeCartProducts);
                        if (req.productsList.Count > 0 && cartDetails != null)
                        {
                            foreach (var prod in req.productsList)
                            {
                                _context.UserCartProductXrefs.Add(new UserCartProductXref
                                {
                                    UserCartId = cartDetails.Id,
                                    ProductId = prod.ProductId,
                                    Quantity = prod.Quantity
                                });
                            }
                            _context.SaveChanges();
                        }
                        response.Result = cartDetails;
                        response.AffectedRecords = 1;
                        response.IsSuccess = true;
                        response.EndUserMessage = "Cart Updated Successfully";
                    }
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "Cart Cannot be empty";
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
        public ValueDataResponse<GetCartByUserResponse> GetCartByUser(int UserId)
        {
            ValueDataResponse<GetCartByUserResponse> response = new ValueDataResponse<GetCartByUserResponse>();
            try
            {
                var result = _context.UserCarts.Where(x => x.UserId == UserId).FirstOrDefault();
                if (result != null)
                {
                    result.CreatedDate = result.CreatedDate.ToLocalTime();
                    result.UpdatedDate = result.UpdatedDate.ToLocalTime();
                    List<GetProductsByUserCartId_Result> cartProductDetails = _context.GetProductsByUserCartId(result.Id).ToList();
                    if (cartProductDetails.Count > 0)
                    {
                        foreach (var res in cartProductDetails)
                        {
                            res.Filepath = res.FileName != null ? string.Format("{0}{1}/{2}", FileRepositoryURL, FileRepositoryFolder, res.Filepath) : null;
                        }
                    }
                    response.Result = new GetCartByUserResponse
                    {
                        cart = result,
                        productsList = cartProductDetails
                    };
                    response.AffectedRecords = 1;
                    response.IsSuccess = true;
                    response.EndUserMessage = "User Cart Details Found";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Cart Details Found";
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
        public ValueDataResponse<UserCart> DeleteUserCartByUserId(int UserId)
        {
            ValueDataResponse<UserCart> response = new ValueDataResponse<UserCart>();
            try
            {
                var cartDetails = _context.UserCarts.Where(x => x.UserId == UserId).FirstOrDefault();

                if (cartDetails != null)
                {
                    var cartProductDetails = _context.UserCartProductXrefs.Where(x => x.UserCartId == cartDetails.Id).ToList();

                    _context.UserCartProductXrefs.RemoveRange(cartProductDetails);
                    _context.UserCarts.Remove(cartDetails);
                    var Id = _context.SaveChanges();
                    response.AffectedRecords = Id > 0 ? 1 : 0;
                    response.IsSuccess = Id > 0 ? true : false;
                    response.EndUserMessage = Id > 0 ? "Cart Deleted" : "Cart Deletion Failed";
                }
                else
                {
                    response.AffectedRecords = 0;
                    response.IsSuccess = false;
                    response.EndUserMessage = "No Cart Details Found";
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
