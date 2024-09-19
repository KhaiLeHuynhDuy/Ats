using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lpnet.WebApp.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}