using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eShop.Controllers
{
    public class AdminMainController : Controller
    {
        // GET: AdminMain
        public ActionResult DashBoard()
        {

      if (Request.IsAjaxRequest()) {
           return PartialView();
      }
            return View();
        }
    }
}