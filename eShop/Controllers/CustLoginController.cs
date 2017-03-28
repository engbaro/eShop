using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eShop.Model;

namespace eShop.Controllers
{
    public class CustLoginController : Controller
    {
        private FirdoosModel db = new FirdoosModel();

        // GET: CustLogin
        public ActionResult LoginForms()
        {
            AppUser currentCustomer=(AppUser)Session["customer"];
            if (currentCustomer != null) {
                RedirectToAction("CheckoutPage", "Checkout");
            }
            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            ViewBag.allCategories= allCategories;
            return View();
        }

        //[Bind(Include = "ConfirmPassword,CustName,Password,Address,City,Postcode,Phone,Country")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginForms(AppUser newCustomer)
        {
            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            ViewBag.allCategories = allCategories;
            if (db.Users.Any(c => c.PhoneNumber == newCustomer.PhoneNumber))
            {

                /*ViewBag.sucess="Phone already exist! please login to your account or create another one.";
                return View(newCustomer);*/
                return Json(new { success = "PhoneExist", errorMessage = "The phone you entered already exist <a href='#' id='login-from-register'>Login</a> to your account" });
            }
            else if (!newCustomer.PhoneNumber.Any(c => char.IsDigit(c)) || !newCustomer.PhoneNumber.Trim().StartsWith("07"))
            {
                return Json(new { success = "PhoneWrong", errorMessage = "The phone you entered either contains letters or does not start with 07. Please enter a valid phone number." });
            }
            newCustomer.CompanyId = 1;
            if (ModelState.IsValid)
            {
                db.Users.Add(newCustomer);
                await db.SaveChangesAsync();
                List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
                Session["customer"] = newCustomer;

                if (allItems==null|| allItems.Count ==0)
                {
                    return Json(new { success = "yes", url = Url.Action("HomePage", "CustProducts") });
                }
                else
                {
                    return Json(new { success = "yes", url = Url.Action("CheckoutPage", "Checkout") });
                   
                }

            }

          

            if (Request.IsAjaxRequest())
            {
                return PartialView(newCustomer);
            }
            return View(newCustomer);
        }
        // GET: CustLogin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser customer =  db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //[Bind(Include = "ConfirmPassword,CustName,Password,Address,City,Postcode,Phone,Country")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AppUser loginCustomer)
        {
            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            ViewBag.allCategories = allCategories;
            AppUser user=db.Users.First(c=>c.PhoneNumber==loginCustomer.PhoneNumber);
            if (user == null)
            {
                return Json(new { sucess = "NotRegistered", errorMessage = "we don't have an account with this phone number. click <a href='#' id='register-from-login'>here</a> to register" });
            }
            else
            {
                AppUser verifyPassword=db.Users.FirstOrDefault(c=>(c.PhoneNumber==loginCustomer.PhoneNumber) && (c.PasswordHash==loginCustomer.PasswordHash));
                if (verifyPassword == null)
                {
                    return Json(new { sucess = "WrongPassword", errorMessage = "The password you entered is not correct" });
                }
                else {
                    List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
                    Session["customer"] = verifyPassword;
                    if (allItems == null)
                    {
                        return Json(new { succes = "yes", url = Url.Action("HomePage", "CustProducts") });
                    }
                    if (allItems.Count > 0)
                    {
                        return Json(new { succes="yes", url = Url.Action("CheckoutPage", "Checkout") });
                    }
                    else
                    {
                        return Json(new { succes = "yes", url = Url.Action("HomePage", "CustProducts") });
                    }
                }

            }
        }

       //Have to check where this is being used. 
        private int addOrder(AppUser newCustomer) {

            List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
            double totalPrice = 0.0;
            foreach (OrderItem item in allItems)
            {
                totalPrice += item.Price;
            }
            Session["customer"] = newCustomer;
            Order newOrder = new Order() { CompanyId = 1, UserId = newCustomer.Id, Address = newCustomer.Address, Country = newCustomer.Country, CustomerName = newCustomer.UserName , DeliveryCost = 3, Notes = "", OrderDate = DateTime.Now, Postcode = newCustomer.Postcode, Phone = newCustomer.PhoneNumber, Town = "baghdad", TotalPrice = totalPrice };
            Session["order"]=newOrder;
            return allItems.Count;

        }
    }
}
