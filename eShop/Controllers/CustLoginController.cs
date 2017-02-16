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

        //[Bind(Include = "ConfirmPassword,CustName,Password,Address,City,Postcode,Phone,Country")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginForms(Customer newCustomer)
        {
            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
            ViewBag.allCategories = allCategories;
            if (db.Customers.Any(c => c.Phone == newCustomer.Phone))
            {

                /*ViewBag.sucess="Phone already exist! please login to your account or create another one.";
                return View(newCustomer);*/
                return Json(new { success = "PhoneExist", errorMessage = "The phone you entered already exist <a href='#' id='login-from-register'>Login</a> to your account" });
            }
            else if (!newCustomer.Phone.Any(c => char.IsDigit(c)) || !newCustomer.Phone.Trim().StartsWith("07"))
            {
                return Json(new { success = "PhoneWrong", errorMessage = "The phone you entered either contains letters or does not start with 07. Please enter a valid phone number." });
            }
            newCustomer.CompanyID=1;
            if (ModelState.IsValid)
            {
                db.Customers.Add(newCustomer);
                await db.SaveChangesAsync();
                List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
                Session["customer"] = newCustomer;

                if (allItems.Count > 0)
                {
                    return Json(new { succes = "yes", url = Url.Action("CheckoutPage", "Checkout") });
                }
                else
                {
                    return Json(new { succes = "yes", url = Url.Action("HomePage", "CustProducts") });
                }

            }

          

            if (Request.IsAjaxRequest())
            {
                return PartialView(newCustomer);
            }
            return View(newCustomer);
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

        //[Bind(Include = "ConfirmPassword,CustName,Password,Address,City,Postcode,Phone,Country")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer loginCustomer)
        {
            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
            ViewBag.allCategories = allCategories;
            Customer user=db.Customers.First(c=>c.Phone==loginCustomer.Phone);
            if (user == null)
            {
                return Json(new { sucess = "NotRegistered", errorMessage = "we don't have an account with this phone number. click <a href='#' id='register-from-login'>here</a> to register" });
            }
            else
            {
                Customer verifyPassword=db.Customers.FirstOrDefault(c=>(c.Phone==loginCustomer.Phone) && (c.Password==loginCustomer.Password));
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

        
        private int addOrder(Customer newCustomer) {

            List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
            double totalPrice = 0.0;
            foreach (OrderItem item in allItems)
            {
                totalPrice += item.price;
            }
            Session["customer"] = newCustomer;
            Order newOrder = new Order() { companyID = 1, customerID = newCustomer.CustomerID, address = newCustomer.Address, country = newCustomer.Country, custName = newCustomer.CustName, deliverycost = 3, notes = "", orderDate = DateTime.Now, postcode = newCustomer.Postcode, phone = newCustomer.Phone, town = "baghdad", totalPrice = totalPrice };
            Session["order"]=newOrder;
            return allItems.Count;

        }
    }
}
