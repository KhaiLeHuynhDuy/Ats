using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lpnet.WebApp.Controllers
{
    public class EmailTemplatesController : Controller
    {
        [ActionName("email-template-basic")]
        public ActionResult emailtemplatebasic()
        {
            return View();
        }
        
        [ActionName("email-template-alert")]
        public ActionResult emailtemplatealert()
        {
            return View();
        }

        [ActionName("email-template-billing")]
        public ActionResult emailtemplatebilling()
        {
            return View();
        }
    }
}