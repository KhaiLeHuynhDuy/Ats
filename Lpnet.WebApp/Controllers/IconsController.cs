using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lpnet.WebApp.Controllers
{
    public class IconsController : Controller
    {
        [ActionName("icons-material")]
        public ActionResult iconsmaterial()
        {
            return View();
        }

        [ActionName("icons-fontawesome")]
        public ActionResult iconsfontawesome()
        {
            return View();
        }

        [ActionName("icons-ion")]
        public ActionResult iconsion()
        {
            return View();
        }

        [ActionName("icons-themify")]
        public ActionResult iconsthemify()
        {
            return View();
        }

        [ActionName("icons-dripicons")]
        public ActionResult iconsdripicons()
        {
            return View();
        }
       
        [ActionName("icons-typicons")]
        public ActionResult iconstypicons()
        {
            return View();
        }
    }
}