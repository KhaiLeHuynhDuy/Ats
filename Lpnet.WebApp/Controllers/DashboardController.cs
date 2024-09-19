using Ats.Domain.Identity.Models;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using System.Globalization;
using Ats.Commons;
using Ats.Models.Dashboard;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private ICampaignService _campaignservice;
        private IMemberProductQrCodeService _memberproductqrcodeservice;
        private IMembershipService _memberService;
        private IProductOrderService _productorderService;
        private IClaimsService _claimsService;
        public DashboardController(UserManager<User> userManage, ICampaignService campaignservice, IProductOrderService productorder, IMemberProductQrCodeService memberproductqrcode, IMembershipService memberService,IClaimsService claimsService ,SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _campaignservice = campaignservice;
            _memberService = memberService;
            _productorderService = productorder;
            _memberproductqrcodeservice = memberproductqrcode;
            _claimsService = claimsService;
        }

        // GET: Dashboard
        public ActionResult Index(string[]? data, int? total)
        {

            string years = DateTime.Now.Year.ToString();
            DateTime dateto = DateTime.Now.StartOfMonth();
            DateTime datefrom = DateTime.Now.EndOfMonth();
            int datatoday = DateTime.Now.Month;
            string strName = Contants.LAYOUT_VERTICAL;
            string strWelcomeText = "Dashboard";

            if (TempData["ModeName"] != null)
                strName = TempData["ModeName"].ToString();

            if (TempData["WelcomeText"] != null)
                strWelcomeText = TempData["WelcomeText"].ToString();

            ViewBag.ModeName = strName;
            ViewBag.WelcomeText = strWelcomeText;

            var list4campaign = _campaignservice.FatestFourCampaigns(); // list four campaign new create
            ViewBag.ListCampaign = list4campaign;

            int totalmemberinactivebyyear = _memberService.GetAllMembersOfYear(years);

            var memberOfMonth = GetNewMemberByYear(years);

            ViewBag.DataChart = memberOfMonth.Data.ToArray();
            ViewBag.TotalNewMember = memberOfMonth.Total;
            ViewBag.TotalMemberInactiveOfYear = memberOfMonth.TotalMemberInactive;
            ViewBag.TotalNewMemberInMonths = memberOfMonth.totalmemberinmonths;

            ViewBag.TotalMemberInByYear = totalmemberinactivebyyear;
            ViewBag.Month = datatoday;
            ViewBag.Year = years;

            return View();
        }
        public ActionResult GetNewMemberByYearToJson(string years)
        {
            if (years == null) { years = DateTime.Now.Year.ToString(); }

            var memberOfMonth = GetNewMemberByYear(years);
            return Json(memberOfMonth);
        }

        public Ats.Models.Member.MemberOfMonthViewModel GetNewMemberByYear(string? years) // bỏ qua service thì oke hơn
        {
            Ats.Models.Member.MemberOfMonthViewModel result = new Ats.Models.Member.MemberOfMonthViewModel();
            string yearNow = DateTime.Now.Year.ToString();
            DateTime dateto = DateTime.Now.StartOfMonth();
            DateTime datefrom = DateTime.Now.EndOfMonth();
            //Ats.Models.Member.ToTalMemberOfMonthViewModel total = new Ats.Models.Member.ToTalMemberOfMonthViewModel();
            if (years != null)
            {
                for (int i = 0; i < 12; i++)
                {

                    int NewMemberOfMonth = _memberService.GetNewMembersOfMonth(i + 1, years); // số thành viên mới trong mỗi tháng
                    result.Total += NewMemberOfMonth;
                    result.Data.Add(NewMemberOfMonth.ToString());


                    result.YearSelect = years;

                }
                result.Data.Add(result.Total.ToString());

                for (int i = 0; i < 12; i++)
                {

            

                    int NewMemberOfMonths = _memberService.GetMembersOfMonthInActive(i + 1, years); // số thành viên không hoạt động trong mỗi tháng
                    result.TotalMemberInactive += NewMemberOfMonths;
                    result.Data.Add(NewMemberOfMonths.ToString());

                    result.YearSelect = years;

                }
                result.Data.Add(result.TotalMemberInactive.ToString());
                if(yearNow != years)
                {
                    int totalmemberinmonths = 0;
                }
                else
                { int totalmemberinmonths = _memberService.GetNewMembersMonths(dateto, datefrom);
                    result.totalmemberinmonths += totalmemberinmonths;
                }
                result.Data.Add(result.totalmemberinmonths.ToString());


            }
            return result;
        }
        //public ActionResult GetNewMemberByYearToJsonData(string years) //data
        //{
        //    if (years == null) { years = DateTime.Now.Year.ToString(); }
        //    var memberOfMonth = GetNewMemberByYearData(years);
        //    return Json(memberOfMonth);
        //}
        //public Ats.Models.Member.MemberOfMonthViewModel GetNewMemberByYearData(string? years) // bỏ qua service thì oke hơn
        //{
        //    Ats.Models.Member.MemberOfMonthViewModel result = new Ats.Models.Member.MemberOfMonthViewModel();
        //    //Ats.Models.Member.ToTalMemberOfMonthViewModel total = new Ats.Models.Member.ToTalMemberOfMonthViewModel();
        //    if (years != null)
        //    {
        //        for (int i = 0; i < 12; i++)
        //        {

        //            int NewMemberOfMonth = _memberService.GetNewMembersOfMonth(i + 1, years); // số thành viên mới trong mỗi tháng
        //            result.Total += NewMemberOfMonth;
        //            result.Data.Add(NewMemberOfMonth.ToString());


        //            result.YearSelect = years;

        //        }
        //        result.Data.Add(result.Total.ToString());

        //    }
        //    return result;
        //}


        DateTime ConvertStringToDate(string dateInString)
        {
            try
            {
                return DateTime.ParseExact(dateInString, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            return DateTime.Now;
        }

        public ActionResult GetStatisticsDateToJson(string datetodate)
        {
            int number = datetodate.Length;
            string StartDay = datetodate.Substring(0, 10); // start day
            string EndDay = datetodate.Substring(13);  // end day
            DateTime DayB = ConvertStringToDate(StartDay);
            DateTime DayE = ConvertStringToDate(EndDay);
            var statisticsrangedate = GetStatisticsDate(DayB, DayE);
            return Json(statisticsrangedate);
        }

        public ActionResult GetStatisticsDateToJsonClear()
        {
            var statisticsrangedate = GetStatisticsDate();
            return Json(statisticsrangedate);
        } 
        public ActionResult GetStatisticsDateToJsonThisMonth()
        {
            var statisticsrangedate = GetStatisticsDateThisMonth();
            return Json(statisticsrangedate);
        }

        public Ats.Models.Dashboard.StatisticsViewModel GetStatisticsDate(DateTime DayB, DateTime DayE) //date to date
        {
            Ats.Models.Dashboard.StatisticsViewModel result = new Ats.Models.Dashboard.StatisticsViewModel();
            int SLQuetMaQr = _memberproductqrcodeservice.GetStatisticsMemberProductQrCodes(DayB, DayE);//Số lần quét mã ( chưa có tính năng quết mã nên chưa làm) 
            int SLDoiThuong = _productorderService.GetStatisticsProductOrders(DayB, DayE);//Số lần đổi thưởng 
            int SLThanhVienMoi = _memberService.GetStatisticsNewMembers(DayB, DayE); //Số lượng thành viên mới
            result.NumberOfCodeScans = SLQuetMaQr;
            result.NumberOfRedemptions = SLDoiThuong;
            result.NumberOfNewMembers = SLThanhVienMoi;
            return result;
        }
        public Ats.Models.Dashboard.StatisticsViewModel GetStatisticsDate() // clear get all
        {
            Ats.Models.Dashboard.StatisticsViewModel result = new Ats.Models.Dashboard.StatisticsViewModel();
            int SLQuetMaQr = _memberproductqrcodeservice.GetStatisticsMemberProductQrCodes();
            int SLDoiThuong = _productorderService.GetStatisticsProductOrders();
            int SLThanhVienMoi = _memberService.GetStatisticsNewMembers();
            result.NumberOfCodeScans = SLQuetMaQr;
            result.NumberOfRedemptions = SLDoiThuong;
            result.NumberOfNewMembers = SLThanhVienMoi;
            return result;
        }
        public Ats.Models.Dashboard.StatisticsViewModel GetStatisticsDateThisMonth() // this month
        { 
            DateTime myDate = DateTime.Today;
            //// Lấy ra ngày đầu tiên của tháng hiện tại. Đó luôn là ngày 1
            DateTime firstDayOfMonth = new DateTime(myDate.Year, myDate.Month, 1);
            //// Lấy ra ngày đầu tiên của tháng tiếp theo bằng cách sử dụng hàm AddMonth
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            //// Ngày cuối cùng của tháng là ngày trước đó
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);

            Ats.Models.Dashboard.StatisticsViewModel result = new Ats.Models.Dashboard.StatisticsViewModel();
            int SLQuetMaQr = _memberproductqrcodeservice.GetStatisticsMemberProductQrCodesThisMonth(firstDayOfMonth, lastDayOfMonth);
            int SLDoiThuong = _productorderService.GetStatisticsProductOrdersThisMonth(firstDayOfMonth, lastDayOfMonth);
            int SLThanhVienMoi = _memberService.GetStatisticsNewMembersThisMonth(firstDayOfMonth, lastDayOfMonth);
            result.NumberOfCodeScans = SLQuetMaQr;
            result.NumberOfRedemptions = SLDoiThuong;
            result.NumberOfNewMembers = SLThanhVienMoi;
            result.DateToDateThisMonth = string.Format("{0:dd/MM/yyyy}", firstDayOfMonth) + " - " + string.Format("{0:dd/MM/yyyy}", lastDayOfMonth);
            return result;
        }

     

    }
}