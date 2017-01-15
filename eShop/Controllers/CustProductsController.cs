using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShop.Models;


namespace eShop.Controllers
{
    public class CustProductsController : Controller
    {
        private DbModel db = new DbModel();

        // GET: CustProduct
        public ActionResult ViewAll()
        {

            if (Request.IsAjaxRequest()) {

                return(PartialView());
            }
            return View();
        }
    }
}