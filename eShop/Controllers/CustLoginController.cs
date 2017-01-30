using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eShop.Models;

namespace eShop.Controllers
{
    public class CustLoginController : Controller
    {
        private DbModel db = new DbModel();

        // GET: CustLogin
        public ActionResult LoginForms()
        {

            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
            ViewBag.allCategories= allCategories;
            return View();
        }

        // GET: CustLogin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }



    }
}
