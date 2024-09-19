using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lpnet.WebApp.Controllers
{
    public class MapsController : Controller
    {
        [ActionName("maps-google")]
        public ActionResult mapsgoogle()
        {
            return View();
        }

        [ActionName("maps-vector")]
        public ActionResult mapsvector()
        {
            return View();
        }
    }
}