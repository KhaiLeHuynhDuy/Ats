using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Member;
using Ats.Models.Occupation;
using Ats.Models.Product;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Models;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class ProductOrderController : BaseController
    {
        private IMembershipService _memberService;
        private IMemberWalletService _memberWallet;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IProductOrderService _productorderservice;
        private IProductCollectionItemService _productCollectionItemservice;
        private IProductCollectionService _productCollectionservice;
        private IProductService _productservice;
        private IMembershipService _memberservice;
        private IConfiguration _config;
        string DomainSendZalo = string.Empty;
        public ProductOrderController(UserManager<User> userManage, IProductOrderService productOrderService, IMembershipService memberService, IMemberWalletService memberWallet, IProductCollectionItemService productCollectionItemservice, IConfiguration config, IWebHostEnvironment hostEnvironment,
                          IProductService productservice, IMembershipService membership, IProductCollectionService productCollectionservice, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _productorderservice = productOrderService;
            _config = config;
            webHostEnvironment = hostEnvironment;
            _productCollectionItemservice = productCollectionItemservice;
            _productservice = productservice;
            _productCollectionservice = productCollectionservice;
            _memberservice = membership;
            _memberService = memberService;
            _memberWallet = memberWallet;
            DomainSendZalo = _config.GetValue<string>("DomainSendZalo");
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Create(Guid Id, Guid iditem, Guid member, Guid idmemberold, Guid iditemold, string search, int? province, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        { 
            #region List Category  
            ViewBag.ProductOrderStatus = Ultility.EnumToSelectList<PRODUCT_ORDER_STATUS>(false);
            #endregion List Category 
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderViewModel model = new ProductOrderViewModel(); 
            if (iditem != Guid.Empty)
            {
                var getitem = _productCollectionItemservice.Get(iditem);
                model.ProductItemId = getitem.Id;
                model.NameProduct = getitem.ProductName;
                model.Price = getitem.Price;
            }
            if (member != Guid.Empty)
            {
                var getmember = _memberservice.GetMemberDetail(member); 
                model.MemberId = getmember.Id;
                model.NameMember = getmember.FullName; 
                if (getmember != null)
                {
                    var point = _memberWallet.GetMemberWallet(getmember.Id);
                    model.CheckPoint = point.Point;
                }
            }
            if (member == Guid.Empty)
            {
                if (idmemberold != Guid.Empty)
                {
                    var getmember = _memberservice.GetMemberDetail(idmemberold);
                    if (getmember != null)
                    {
                        model.MemberId = getmember.Id;
                        model.NameMember = getmember.FullName;
                    }
                }
                else
                {
                    model.MemberId = idmemberold;
                    model.NameMember = " ";
                }
            }
            if (iditem == Guid.Empty)
            {
                if (iditemold != Guid.Empty)
                {
                    var getitem = _productCollectionItemservice.Get(iditemold);
                    if (getitem != null)
                    {
                        model.ProductItemId = getitem.Id;
                        model.NameProduct = getitem.ProductName;
                    }
                }
                else
                {
                    model.ProductItemId = iditemold;
                    model.NameProduct = " ";
                }
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            int pageSize = _config.GetValue<int>("PageSize");
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ProductOrderViewModel model, int pageIndex)
        {
            if (model.MemberId == Guid.Empty && model.ProductItemId == Guid.Empty)
            {
                return View(model);
            }
            if (model.MemberId == Guid.Empty)
            {
                return View(model);
            }
            if (model.ProductItemId == Guid.Empty)
            {
                return View(model);
            }

            bool create = _productorderservice.Create(model);

            if (create == true)
            {
                var getmember = _memberservice.GetMemberDetail(model.MemberId);
                Guid userId = GetUserIdLogin();
                if (model.OrderStatus == PRODUCT_ORDER_STATUS.NEW && getmember.ZaloId != null)
                {
                    var sendInTransit = new SendZaloViewModel()
                    {
                        user_id = userId.ToString(),
                        member_id = getmember.ZaloId,
                        text = ($"Chúc mừng bạn đã đổi quà ({model.NameProduct}) thành công!" +
                        $"\nNature's Way sẽ gói quà thật đẹp để gửi đến bạn /-heart" +
                        $"\nMọi thắc mắc hãy liên hệ với mình qua hotline 1900.636911 nhé!")
                    };
                    SendZaloOrders(sendInTransit);
                }
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            }
            else
            {
                TempData["ErrorMessage"] = Resource.Common_notify_addFailed;
                return RedirectToAction("Create", new { iditem = model.ProductItemId, member= model.MemberId });
            } 
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id)
        {
            _logger.LogDebug($"ProductOrder Detail(Id: {id})");
            if (id == null) { return NotFound(); }
            var model = new ProductOrderViewModel();
            var entity = _productorderservice.GetProductOrder(id);
            if (entity != null) model = entity;
            return View(model);
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Edit(Guid id, Guid idorder, Guid idmember, Guid iditem, MemberSearchView memberSearch, string search, int? province, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool valid, string field, bool sort, int page, int size = 0/*, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo*/)
        {
            if (id == null) { return NotFound(); }
            //if(idorder != Guid.Empty) { if (idorder != id) { return NotFound(); } } 
            var modelole = new ProductOrderViewModel();
            var productOrder = _productorderservice.GetProductOrder(id);
            if (productOrder == null) { return NotFound(); }
            else
            {
                #region List Category  
                ViewBag.ProductOrderStatus = Ultility.EnumToSelectList<PRODUCT_ORDER_STATUS>(false);
                #endregion List Category  
                ProductOrderSearchViewModel model = new ProductOrderSearchViewModel();

                if (idmember != Guid.Empty) // edit member
                {
                    if (idmember != productOrder.MemberId) // có thay đổi member
                    {
                        var getnewmember = _memberService.GetMember(idmember);
                        if (getnewmember != null)
                        {
                            productOrder.FirstName = getnewmember.FirstName;
                            productOrder.LastName = getnewmember.FirstName;
                            productOrder.NameMember = getnewmember.FirstName + getnewmember.LastName;
                            productOrder.MemberId = getnewmember.Id;
                        }
                    }
                }
                if (iditem != Guid.Empty) // edit product
                {
                    if (iditem != productOrder.ProductItemId)
                    {
                        var getProductItem = _productCollectionItemservice.Get(iditem);
                        if (getProductItem != null)
                        {
                            productOrder.Price = getProductItem.Price;
                            productOrder.NameProduct = getProductItem.ProductName;
                            productOrder.ProductItemId = getProductItem.Id;
                        }
                    }
                }
                model.ProductOrder = productOrder;
                return View(model);
            }
        }

        [HttpPost]
        public  ActionResult Edit(ProductOrderSearchViewModel model, int pageIndex)
        {
           
            _logger.LogDebug($"Edit ProductCollectionItem (Id: {model.ProductOrder.Id})"); 
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier);
            var entity = _productorderservice.GetProductOrder(model.ProductOrder.Id);
            if (entity != null)
            {
                bool update = _productorderservice.UpdateProductOrder(model.ProductOrder, userId);
                if (update == true)
                {
                  
                    if (model.ProductOrder.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY && entity.zaloId != null)
                    {
                        var sendInTransit = new SendZaloViewModel()
                        {
                            user_id = userId.Value,
                            member_id = entity.zaloId,
                            text = ($"Đơn đổi quà ({entity.NameProduct}) của bạn đang được giao đi. " +
                            $"\nMọi thắc mắc hãy liên hệ với mình qua hotline 1900.636911 nhé!")
                        };
                        SendZaloOrders(sendInTransit);
                    }

                  else if (model.ProductOrder.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL && entity.zaloId != null)
                    {
                        var sendCancel = new SendZaloViewModel()
                        {
                            user_id = userId.Value,
                            member_id = entity.zaloId,
                            text = ($"Rất tiếc! Đơn đổi quà ({entity.NameProduct}) của bạn đã bị hủy. " +
                            $"\nMọi thắc mắc hãy liên hệ với mình qua hotline 1900.636911 nhé!")
                        };
                        SendZaloOrders(sendCancel);
                    }


                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                }
                else
                {
                    TempData["addSuccess"] = Resource.Common_notify_addFailed;
                } 
            }
            if( model.ProductOrder.OrderStatus == PRODUCT_ORDER_STATUS.NEW)
            {    
                return RedirectToAction("Index", new { pageIndex = pageIndex });
            }
            else if(model.ProductOrder.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY)
            {
                return RedirectToAction("IndexTabDelivering", new { pageIndex = pageIndex });

            }
            else if (model.ProductOrder.OrderStatus == PRODUCT_ORDER_STATUS.COMPLETE)
            {
                return RedirectToAction("IndexTabReceived", new { pageIndex = pageIndex });

            }
            else if (model.ProductOrder.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL)
            {
                return RedirectToAction("IndexTabCancelled", new { pageIndex = pageIndex });
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });

        }


        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Product CollectionItem (Id: {id})");
            var entity = _productorderservice.GetProductOrder(id);
            if (entity != null)
            {
                _productorderservice.DeleteProductOrder(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        // ham search member - product - order
        #region Search Member - Product - Order
        public ActionResult Search(string keyword, int pageIndex, int idstatus)
        {
            if(idstatus ==0) { return RedirectToAction("Index", new { search = keyword, pageIndex = pageIndex }); }
            if(idstatus ==1) { return RedirectToAction("IndexTabBrowsing", new { search = keyword, pageIndex = pageIndex }); }
            if(idstatus ==2) { return RedirectToAction("IndexTabDelivering", new { search = keyword, pageIndex = pageIndex }); }
            if(idstatus ==3) { return RedirectToAction("IndexTabReceived", new { search = keyword, pageIndex = pageIndex }); }
            if(idstatus ==4) { return RedirectToAction("IndexTabCancelled", new { search = keyword, pageIndex = pageIndex }); }
            return RedirectToAction("Index", new { search = keyword, pageIndex = pageIndex });
        }

        [HttpPost]
        public ActionResult SearchProduct(string Keyword, int? productId, Guid? productCollectionItemId, Guid? idorder, Guid? idmember, Guid? iditemold, int? stockFrom, int? stockTo,
        string field, bool sort, int page, int size, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string searchproduct = "";
                if (idorder == null) { searchproduct = "SelectProductItem"; }
                else { searchproduct = "EditProduct"; }
                return RedirectToAction(searchproduct, new
                {
                    search = Keyword,
                    ProductId = productId,
                    ProductCollectionItemId = productCollectionItemId,
                    idorder,
                    id = idorder,
                    field,
                    idmember = idmember,
                    iditemold,
                    sort,
                    Page = page,
                    size
                });
            }
            else
            {
                return RedirectToAction("searchproduct", new
                {
                    field,
                    sort,
                    page,
                    size
                });
            }
        }

        [HttpPost]
        public ActionResult SearchMember(ProductOrderSearchViewModel Productd, MemberSearchView memberSearch, Guid? idorder, Guid? iditem, Guid? idmemberold, string Keyword,
         int? ProvinceId, int? OccupationId, int? DistrictId, bool? gender, int? StoreId, int? ChannelId, string field, bool sort, int page, int size, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string member;
                if (idorder == null)
                {
                    member = "SelectMember";
                }
                else
                {
                    member = "EditMember";
                }
                return RedirectToAction(member, new
                {
                    memberCode = memberSearch.MemberCode,
                    firstName = memberSearch.FirstName,
                    lastName = memberSearch.LastName,
                    email = memberSearch.Email,
                    phoneNumber = memberSearch.PhoneNumber,
                    search = Keyword,
                    gender,
                    field,
                    iditem = iditem,
                    idmemberold,
                    sort,
                    page,
                    idorder,
                    id = Productd.ProductOrder.ProductItemId,
                    size
                });
            }
            else
            {
                return RedirectToAction("SelectMember", new
                {
                    memberCode = "",
                    firstName = "",
                    province = "",
                    field,
                    sort,
                    page,
                    size
                });
            }

        }
        #endregion Search Member - Product - Order

        // hai view select member và product item 
        #region Select and Edit Product/Member 
        [HttpGet]
        public ActionResult SelectProductItem(Guid iditem, Guid idmember, Guid iditemold, Guid idmemberold, Guid idorder, string search, int? province, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        {
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel() // truyền dữ liệu
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("SelectProductItem", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int pageSize = _config.GetValue<int>("PageSize");

            if (iditem != Guid.Empty) // đã chọn product collection item
            {
                var product = _productCollectionItemservice.Get(iditem);
                if (product != null)
                {
                    model.ProductCollectionItem = product; // truyen collection item vao model
                }
            }

            if (idmember != Guid.Empty)
            {
                var member = _memberService.GetMember(idmember);
                if (member != null)
                {
                    model.CheckMemberId = idmember; // truyen member vao model
                    model.IdMemberOld = idmember;
                }
            }


            int count = 0;
            var productitem = _productCollectionItemservice.SearchFollowStock(model.Keyword, productId, productCollectionItemId, stockFrom, stockTo, field, sort, page, pageSize, out count); // list item
            model.CollectionItems = productitem; //model list items  
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productitem.Count();
            model.IdItemOld = iditemold;

            return View(model);
        }

        [HttpGet]
        public ActionResult SelectMember(Guid iditem, Guid item, Guid member, Guid idmemberold, MemberSearchView memberSearch, string search, int? province, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        {
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("SelectMember", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int pageSize = _config.GetValue<int>("PageSize");

            if (iditem != Guid.Empty)
            {
                model.CheckItemID = iditem;
            }


            var membersss = _memberService.Search(memberSearch, model.Keyword, province, occupation, jobtitle, district, gender, storeId, channelId, valid, field, sort, page, size, out int count); // list member  
            model.MemberViewModels = membersss; // model list member
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = membersss.Count();

            model.IdMemberOld = idmemberold;
            return View(model);
        }



        [HttpGet]
        public ActionResult EditMember(Guid idorder, Guid iditem, Guid member, MemberSearchView memberSearch, string search, int? province, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        {
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("EditMember", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int pageSize = _config.GetValue<int>("PageSize");

            if (idorder != Guid.Empty)
            {
                model.CheckOrderID = idorder;
                model.CheckItemID = iditem;
            }
            var membersss = _memberService.Search(memberSearch, model.Keyword, province, occupation, jobtitle, district, gender, storeId, channelId, valid, field, sort, page, size, out int count); // list member  
            model.MemberViewModels = membersss; // model list member
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = membersss.Count();
            return View(model);
        }
        [HttpGet]
        public ActionResult EditProduct(Guid idorder, Guid idmember, string search, int? province, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        {
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel() // truyền dữ liệu
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("EditProduct", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int pageSize = _config.GetValue<int>("PageSize");
            if (idorder != Guid.Empty)
            {
                model.CheckOrderID = idorder;
                model.CheckMemberId = idmember;
            }
            int count = 0;
            var productitem = _productCollectionItemservice.SearchFollowStock(model.Keyword, productId, productCollectionItemId, stockFrom, stockTo, field, sort, page, pageSize, out count); // list item
            model.CollectionItems = productitem; //model list items  
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productitem.Count();
            return View(model);
        }
        #endregion Select and Edit Product/Member 

        //comment function
        #region Comment Function
        //[HttpGet]
        //public ActionResult Order(Guid Id, Guid item, Guid iditem, Guid member, MemberSearchView memberSearch, string search, int? province, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        //{ 
        //    #region List Category  
        //    List<MemberDetailViewModel> members = _memberservice.GetMembers();
        //    members.Add(new MemberDetailViewModel() { Id = Guid.Empty, Profile = "--- Chọn Thành Viên ---" });
        //    var slMember = members.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Profile });
        //    ViewBag.Member = new SelectList(slMember, "Value", "Text");
        //    List<ProductCollectionItemViewModel> productCollectionItemss = _productCollectionItemservice.Gets();
        //    productCollectionItemss.Add(new ProductCollectionItemViewModel() { Id = Guid.Empty, ProductName = "--- Chọn Cái Gì Thì Chọn ---" });
        //    var slProductCollectionItems = productCollectionItemss.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ProductName });
        //    ViewBag.ProductCollectionItem = new SelectList(slProductCollectionItems, "Value", "Text");

        //    var order = _productorderservice.GetPrice(Id, item, member);
        //    ViewBag.ProductOrderStatus = Ultility.EnumToSelectList<PRODUCT_ORDER_STATUS>(false);

        //    #endregion List Category

        //    if (page == 0) page += 1;
        //    size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
        //    int pageSize = _config.GetValue<int>("PageSize");
        //    ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
        //    {
        //        Keyword = search,
        //        Pager = new Ats.Models.PagerViewModel("Order", page, size)
        //        {
        //            OrderField = field,
        //            OrderSort = sort
        //        }
        //    };

        //    int countt = 0;
        //    var productCollectionItems = _productCollectionItemservice.Search(model.Keyword, productId, productCollectionItemId, stockFrom, stockTo, field, sort, page, pageSize, out countt);
        //    var membersss = _memberService.Search(memberSearch, model.Keyword, province, occupation, jobtitle, district, gender, storeId, channelId, valid, field, sort, page, size, out int count);


        //    model.Pager.TotalItem = count;
        //    model.Pager.TotalItemInPage = membersss.Count();
        //    model.Pager.Size = size;
        //    model.ProductOrder = order;
        //    model.ProductOrder.MemberViewModels = membersss;

        //    return View(model);
        //}


        //[HttpPost]
        //public ActionResult Order(ProductOrderSearchViewModel model, int pageIndex, Guid ProductItemId)
        //{

        //    bool create = _productorderservice.CreateProductOrder(model.ProductOrder);
        //    if (create == true)
        //    {
        //        TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
        //    }
        //    else
        //    {
        //        TempData["addSuccess"] = Resource.Common_notify_addFailed;
        //    }

        //    return RedirectToAction("ListOrder", new { pageIndex = pageIndex });
        //} 
        //[HttpGet]
        //public ActionResult ListProductItem(string search, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, string field, bool sort, int page)
        //{
        //    _logger.LogDebug($"Store Index Search={search}, Page={page}");

        //    if (page == 0) page += 1;

        //    int pageSize = _config.GetValue<int>("PageSize");

        //    ProductCollectionItemSearchViewModel model = new ProductCollectionItemSearchViewModel()
        //    {
        //        Keyword = search,
        //        Pager = new PagerViewModel("ListProductItem", page, pageSize)
        //        {
        //            OrderField = field,
        //            OrderSort = sort
        //        }
        //    };

        //    int count = 0;
        //    var productCollectionItems = _productCollectionItemservice.Search(model.Keyword, productId, productCollectionItemId, stockFrom, stockTo, field, sort, page, pageSize, out count);
        //    model.ProductCollectionItems = productCollectionItems;
        //    model.Pager.TotalItem = count;
        //    model.Pager.TotalItemInPage = productCollectionItems.Count();

        //    #region List Category

        //    List<ProductViewModel> products = _productservice.GetProducts();
        //    products.Add(new ProductViewModel() { Id = 0, Name = "--- Chọn Sản Phẩm ---" });
        //    var slProducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
        //    ViewBag.Product = new SelectList(slProducts, "Value", "Text");

        //    List<ProductCollectionViewModel> productCollectionItemes = _productCollectionservice.Gets();
        //    productCollectionItemes.Add(new ProductCollectionViewModel() { Id = Guid.Empty, Name = "--- Chọn Tên Bộ Sưu Tập Sản Phẩm ---" });
        //    var slProductCollectionItems = productCollectionItemes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
        //    ViewBag.ProductCollection = new SelectList(slProductCollectionItems, "Value", "Text");

        //    #endregion List Category

        //    #region Status message
        //    if (TempData["addSuccess"] != null)
        //    {
        //        ViewBag.StatusMessage = TempData["addSuccess"].ToString();
        //        TempData.Remove("addSuccess");
        //    }
        //    if (TempData["updateSuccess"] != null)
        //    {
        //        ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
        //        TempData.Remove("updateSuccess");
        //    }
        //    if (TempData["deleteSuccess"] != null)
        //    {
        //        ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
        //        TempData.Remove("deleteSuccess");
        //    }
        //    #endregion

        //    return View(model);
        //}
        #endregion Comment Function


        // Browsing
        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page, int size)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            
            int count = 0; int status = 1;
            var productOrders = _productorderservice.Search(model.Keyword, field, sort, page, status, size, out count);
            model.ProductOrders = productOrders; // list product order
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productOrders.Count();
            model.Pager.Size = size;


            List<ProductOrderViewModel> tags = productOrders; 
            model.CheckBoxChangeStatus = tags.Select(x => new Ats.Models.DisplayItem()
            {
                Value = x.Id.ToString(),
                MemberCode = x.MemberCode,
                NameMember = x.NameMember,
                NameProduct = x.NameProduct,
                Price = x.Price,
                Receiver = x.Receiver,
                PhoneNo = x.PhoneNo,
                ShippingAddress = x.ShippingAddress,
                DateCreate = x.DateCreate, 
                Quantity =x.Quantity,
                OrderStatus = x.OrderStatus,
                IdMember = x.MemberId,
                IdProductItem = x.ProductItemId,
                Selected = false
            }).ToList();

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            #endregion 
            return View(model);
        }


        [HttpGet("ProductOrder/OnChangePagingDropdownlist")]
        public ActionResult OnChangePagingDropdownlist(string search, string field, bool sort, int page , int size)
        {
            string url = "/ProductOrder/Index?search=" + search + "&field=" + field + "&sort=" + sort + "&page=" + page + "&size=" + size;
            return Json(new { url });
        }
        // Delivering
        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult IndexTabDelivering(string search, string field, bool sort, int page, int size)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("IndexTabDelivering", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0; int status = 2;
            var productOrders = _productorderservice.Search(model.Keyword, field, sort, page, status, size, out count);
            model.ProductOrders = productOrders;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productOrders.Count();
            model.Pager.Size = size;
            List<ProductOrderViewModel> tags = productOrders;
            model.CheckBoxChangeStatus = tags.Select(x => new Ats.Models.DisplayItem()
            {
                Value = x.Id.ToString(),
                MemberCode = x.MemberCode,
                NameMember = x.NameMember,
                NameProduct = x.NameProduct,
                Price = x.Price,
                Receiver = x.Receiver,
                PhoneNo = x.PhoneNo,
                Quantity = x.Quantity,
                ShippingAddress = x.ShippingAddress,
                DateCreate = x.DateCreate,
                OrderStatus = x.OrderStatus,
                IdMember = x.MemberId,
                Selected = false
            }).ToList();

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            #endregion 
            return View(model);
        }

        [HttpGet("Delivering/OnChangePagingDropdownlistDelivering")]
        public ActionResult OnChangePagingDropdownlistDelivering(string search, string field, bool sort, int page, int size)
        {
            string url = "/ProductOrder/IndexTabDelivering?search=" + search + "&field=" + field + "&sort=" + sort + "&page=" + page + "&size=" + size;
            return Json(new { url });
        }

        // Received
        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult IndexTabReceived(string search, string field, bool sort, int page , int size)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("IndexTabReceived", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0; int status = 3;
            var productOrders = _productorderservice.Search(model.Keyword, field, sort, page, status, size, out count);
            model.ProductOrders = productOrders;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productOrders.Count();
            model.Pager.Size = size;

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            #endregion 
            return View(model);
        }
        [HttpGet("Received/OnChangePagingDropdownlistDelivering")]
        public ActionResult OnChangePagingDropdownlistReceived(string search, string field, bool sort, int page, int size)
        {
            string url = "/ProductOrder/IndexTabReceived?search=" + search + "&field=" + field + "&sort=" + sort + "&page=" + page + "&size=" + size;
            return Json(new { url });
        }

        // Cancelled
        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult IndexTabCancelled(string search, string field, bool sort, int page , int size)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            ProductOrderSearchViewModel model = new ProductOrderSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("IndexTabCancelled", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int status = 4;
            var productOrders = _productorderservice.Search(model.Keyword, field, sort, page, status, size, out int count);
            model.ProductOrders = productOrders;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productOrders.Count();
            model.Pager.Size = size;
            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            #endregion 
            return View(model);
        }
        [HttpGet("Cancelled/OnChangePagingDropdownlistDelivering")]
        public ActionResult OnChangePagingDropdownlistCancelled(string search, string field, bool sort, int page, int size)
        {
            string url = "/ProductOrder/IndexTabCancelled?search=" + search + "&field=" + field + "&sort=" + sort + "&page=" + page + "&size=" + size;
            return Json(new { url });
        }
        public ActionResult UpdateStatus(ProductOrderSearchViewModel model)
        { 
            if(model.Status ==0) { RedirectToAction("Index"); } 
            _productorderservice.UpdateStatus(model);

            var ListSelects = model.CheckBoxChangeStatus.Where(x => x.Selected == true).ToList();
            foreach(var select in ListSelects)
            {
                
                if (select.Selected == true)
                {
                    var entity = _productorderservice.GetProductOrder(Guid.Parse(select.Value));

                    
                    if (entity.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY && entity.zaloId != null)
                    {
                        var sendInTransit = new SendZaloViewModel()
                        {
                            user_id = entity.ChangedUserId,
                            member_id = entity.zaloId,
                            text = ($"Đơn đổi quà ({entity.NameProduct}) của bạn đang được giao đi. " +
                            $"\nMọi thắc mắc hãy liên hệ với mình qua hotline 1900.636911 nhé!")
                        };
                        SendZaloOrders(sendInTransit);
                    }
                    if (entity.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL && entity.zaloId != null)
                    {
                        var sendInTransit = new SendZaloViewModel()
                        {
                            user_id = entity.ChangedUserId,
                            member_id = entity.zaloId,
                            text = ($"Rất tiếc! Đơn đổi quà ({entity.NameProduct}) của bạn đã bị hủy. " +
                            $"\nMọi thắc mắc hãy liên hệ với mình qua hotline 1900.636911 nhé!")
                        };
                        SendZaloOrders(sendInTransit);
                    }
                }    
            }    

            if(model.Status == 2) { return RedirectToAction("IndexTabDelivering"); }
            if(model.Status == 3) { return RedirectToAction("IndexTabReceived"); }
            if(model.Status == 4) { return RedirectToAction("IndexTabCancelled"); }
            return RedirectToAction("Index");
        }


        private async void SendZaloOrders(SendZaloViewModel send)
        {
             using (var stringContent = new FormUrlEncodedContent(new[]
              {
                new KeyValuePair<string, string>("user_id", send.user_id),
                new KeyValuePair<string, string>("member_id", send.member_id),
                new KeyValuePair<string, string>("text", send.text)

              }))
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(DomainSendZalo, stringContent);
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
        }

    }
}