using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.LoyaltyTier;
using Ats.Models.Member;
using Ats.Models.Product;
using Ats.Models.Reports;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Models;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

using static Ats.Commons.Constants;
using System.Data;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private IReportsService _service;
        private IConfiguration _config;
        public ReportsController(UserManager<User> userManage, IReportsService service, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _service = service;
            _config = config;
        }

        [AuthorizeRoles(RoleName.SYS_ROLE_REPORT_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public async Task<IActionResult> MemberReport(string search, string datefrom, string dateto, string field, bool sort, int page, int size = 0)
        {

            _logger.LogDebug($"Member Report Search= datefrom :{datefrom}, dateto{dateto}, Page={page}");
            DateTime? _dateFrom, _dateTo;

            if (page == 0) page += 1;
            size = size == 0 ? 30 : (int)size;
            MemberReportsSearchViewModel model = new MemberReportsSearchViewModel()
            {
                Keyword = search,
                DateFrom = datefrom,
                DateTo = dateto,
                PagerNewMemberReports = new Ats.Models.PagerNewMemberReportsViewModel("MemberReport", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var members =  _service.SearchMemberReport(model.Keyword, model.DateFrom, model.DateTo, field, sort, page, size, out int count);
            model.memberReports = members;
            
            model.PagerNewMemberReports.TotalItem = count;
            model.PagerNewMemberReports.TotalItemInPage = members.Count();
            model.PagerNewMemberReports.Size = size;
            model.PagerNewMemberReports.Search = search;
            model.PagerNewMemberReports.dateFrom = datefrom;
            model.PagerNewMemberReports.dateTo = dateto;
            if (datefrom != null)
            {
                _dateFrom = DateTime.Parse(datefrom).EarlyInDay();
                model.StringDateFrom = string.Format("{0:MM/yyyy}", _dateFrom);

            }
            if (dateto != null)
            {
                _dateTo = DateTime.Parse(dateto).EndInDay();
                model.StringDateTo = string.Format("{0:MM/yyyy}", _dateTo);
            }
            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            #endregion

            return await Task.Run(() => View(model));
        }
        [HttpPost]
        public async Task<IActionResult> SearchMemberReport(string Search, string DateFrom, string DateTo, string field, bool sort, int page, int size, string reset)
        {
            if (!string.IsNullOrEmpty(reset))
            {
                return RedirectToAction("MemberReport", new
                {
                    search = Search,
                    datefrom = DateFrom,
                    dateto = DateTo,
                    field,
                    sort,
                    page,
                    size
                });
            }
            else
            {
                return RedirectToAction("MemberReport", new
                {

                });
            }
      
       

        }

   

        public async Task<IActionResult> ExportNewMember(string DateFrom, string DateTo)
        {

            DataTable dt = new DataTable("New-Member-Report");
            MemoryStream stream = new MemoryStream();
            dt.Columns.AddRange(new DataColumn[11] { new DataColumn("Mã Thành Viên"),
                                        new DataColumn("Tên"),
                                        new DataColumn("Họ"),
                                        new DataColumn("Giới Tính"),
                                        new DataColumn("Năm Sinh"),
                                        new DataColumn("Cấp Bậc"),
                                        new DataColumn("Điểm"),
                                        new DataColumn("Ngày Đăng Ký"),
                                        new DataColumn("Tỉnh/Thành Phố"),
                                        new DataColumn("Số Điện Thoại"),
                                        new DataColumn("Trạng Thái")

                                        });

            var membersReport = await _service.ExportMemberReport(DateFrom, DateTo);
            if (membersReport.Count > 0)
            {
                foreach (var member in membersReport)
                {
                    dt.Rows.Add(member.MemberCode, member.FirstName, member.LastName, member.MaleOrFemale, member.YOB, member.TierName, member.Point,
                        member.CreateDate, member.ProvinceName, member.PhoneNumber, member.StringStatus);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);

                    using (stream)
                    {
                        wb.SaveAs(stream);
                    }


                }
                return await Task.Run(() => File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NewMemberReport.xlsx"));

            }

            return RedirectToAction("MemberReport", new
            {

            });
        }


        [AuthorizeRoles(RoleName.SYS_ROLE_REPORT_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public IActionResult ProductPointsScanReport(string search, string[] productId, string datefrom, string dateto, string field, bool sort, int page, int size = 0)
        {
            _logger.LogDebug($"Reports Product Points Scan Report loaded");

            DateTime? _dateFrom, _dateTo;
            if (page == 0) page += 1;
            size = size == 0 ? 2 : (int)size;
            //int stt = 0;
            ProductQrReportsSearchViewModel model = new ProductQrReportsSearchViewModel()
            {
                Keyword = search,
                ArProductId = productId,
                StringDateFrom = datefrom,
                StringDateTo = dateto,
                PagerProductPointsScan = new Ats.Models.PagerProductPointsScanReportViewModel("ProductPointsScanReport", page, size)
                {

                    OrderField = field,
                    OrderSort = sort
                }
            };
            var qrReports =  _service.SearchProductQrReport(model.Keyword, model.ArProductId, model.StringDateFrom, model.StringDateTo, field, sort, page, size, out int count);
            model.productQrReports = qrReports;
            //model.productQrReports. = stt++;
            model.PagerProductPointsScan.Search = search;
            model.PagerProductPointsScan.Productid = productId;
            model.PagerProductPointsScan.dateFrom = datefrom;
            model.PagerProductPointsScan.dateTo = dateto;
            model.PagerProductPointsScan.TotalItemInPage = qrReports.Count();
            model.PagerProductPointsScan.Size = size;
            model.PagerProductPointsScan.TotalItem = count;


            if (datefrom != null)
            {
                _dateFrom = DateTime.Parse(datefrom).EarlyInDay();
                model.Stringdatefrom = string.Format("{0:MM-yyyy}", _dateFrom);

            }
            if (dateto != null)
            {
                _dateTo = DateTime.Parse(dateto).EarlyInDay();
                model.Stringdateto = string.Format("{0:MM-yyyy}", _dateTo);
            }

            model.ProductIdList = GetProducts();


            return View(model);
        }
        private IList<SelectListItem> GetProducts()
        {
            var products = _service.GetProducts().ToList();
            var slproducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.Code} - {x.Name}" }).ToList();

            return slproducts;

        }

        [HttpPost]
        public IActionResult SearchScanProductPoints(string Search, string[] ArProductId, string StringDateFrom, string StringDateTo, string field, bool sort, int page, int size, string reset)
        {

            if (!string.IsNullOrEmpty(reset))
            {
                return RedirectToAction("ProductPointsScanReport", new
                {
                    search = Search,
                    productId = ArProductId,
                    datefrom = StringDateFrom,
                    dateto = StringDateTo,
                    field,
                    sort,
                    page,
                    size
                });

            }
            else
            {
                return RedirectToAction("ProductPointsScanReport", new
                {

                });
            }
        
      
        }

        public async Task<IActionResult> ExportScanProductPoints(string[] ArProductId, string StringDateFrom, string StringDateTo)
        {



            DataTable dt = new DataTable("Product-Scan-Report");
            MemoryStream stream = new MemoryStream();
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("STT"),
                                        new DataColumn("Ngày Quét Mã QR"),
                                        new DataColumn("Mã Thành Viên"),
                                        new DataColumn("Họ Và Tên"),
                                        new DataColumn("UIID Mã QR"),                          
                                        new DataColumn("Mã Sản Phẩm"),
                                        new DataColumn("Tên Sản Phẩm"),

                                        });

            var qrReports = await _service.ExportProductQrReport(ArProductId, StringDateFrom, StringDateTo);
            if (qrReports.Count > 0)
            {
                foreach (var ProductQr in qrReports)
                {
                    dt.Rows.Add(ProductQr.STT, ProductQr.CodeScanDate, ProductQr.MemberCode, ProductQr.FullName, ProductQr.BillNo, ProductQr.ProductCode, ProductQr.ProductName);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);

                    using (stream)
                    {
                        wb.SaveAs(stream);
                    }


                }
                return await Task.Run(() => File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductScanReport.xlsx"));

            }

            return RedirectToAction("ProductPointsScanReport", new
            {

            });
        }



        [AuthorizeRoles(RoleName.SYS_ROLE_REPORT_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public IActionResult RedemptionReport(string search, Guid? collectionId, string field, bool sort, int page, int size = 0)
        {
            _logger.LogDebug($"Reports RedemptioncReport loaded");


            if (page == 0) page += 1;
            size = size == 0 ? 2 : (int)size;
            RedemptionReportsSearchViewModel model = new RedemptionReportsSearchViewModel()
            {
                Keyword = search,
                collectionId = collectionId,
                PagerRedemptionReports = new Ats.Models.PagerRedemptionReportsViewModel("RedemptionReport", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var redemReports =  _service.SearchRedemptionReport(model.Keyword, model.collectionId, field, sort, page, size, out int count);
            
            model.redemptionReports = redemReports;
            model.PagerRedemptionReports.TotalItem = count;
            model.PagerRedemptionReports.TotalItemInPage = redemReports.Count();
            model.PagerRedemptionReports.Size = size;
            model.PagerRedemptionReports.Search = search;
            model.PagerRedemptionReports.ColectionId = collectionId;

            List<ProductCollectionViewModel> collections = _service.GetCollections();
            collections.Add(new ProductCollectionViewModel { Id = Guid.Empty, Name = "--- Select ---" });
            var slCollections = collections.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Collections = new SelectList(slCollections, "Value", "Text");

            if (model.collectionId != null && model.collectionId != Guid.Empty)
            {
                var NamePr = collections.Where(x => x.Id == collectionId).FirstOrDefault();
                model.collectionName = NamePr.Name;
            }

            return View(model);
        }
        [HttpPost]
        public  ActionResult SearchRedemption(string Search, Guid? CollectionId, string field, bool sort, int page, int size, string reset, string export)
        {

            if (!string.IsNullOrEmpty(reset))
            {
                return RedirectToAction("RedemptionReport", new
                {
                    search = Search,
                    collectionId = CollectionId,
                    field,
                    sort,
                    page,
                    size
                });

            }
            else
            {
                return RedirectToAction("RedemptionReport", new
                {

                });
            }
      
        }
        public async Task<IActionResult> ExportRedemption(Guid? CollectionId)
        {

            DataTable dt = new DataTable("Redemption-Report");
            MemoryStream stream = new MemoryStream();

            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Mã Sản Phẩm"),
                                        new DataColumn("Tên Sản Phẩm"),
                                        new DataColumn("Tổng Hàng Đang Duyệt"),
                                        new DataColumn("Tổng Hàng Đang Giao"),
                                        new DataColumn("Tổng Hàng Hoàn Thành"),
                                        new DataColumn("Tổng Hàng Đã Hủy"),
                                        });
            var redemReports = await _service.ExportRedemptionReport(CollectionId);
            if (redemReports.Count > 0)
            {
                foreach (var redem in redemReports)
                {
                    dt.Rows.Add(redem.ProductCode, redem.ProductName, redem.TotalNewOrder, redem.TotalInTransit, redem.TotalCompleted, redem.TotalCanceled);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);

                    using (stream)
                    {
                        wb.SaveAs(stream);
                    }


                }
                return await Task.Run(() => File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RedemptionReport.xlsx"));

            }

            return RedirectToAction("RedemptionReport", new
            {

            });
        }
    }
}