using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShop.Models;
namespace eShop.Controllers
{
    public class CheckoutController : Controller
    {

        private DbModel db = new DbModel();
        // GET: Checkout
        public ActionResult CheckoutPage()
        {
            try
            {

                List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();

                ViewBag.allCategories = allCategories;

                List<OrderItem> allOrderItems = (List<OrderItem>)Session["OrderItem"];

             
                if (!allOrderItems.Any() || allOrderItems == null)
                {
                    ViewBag.error="emptyBasket";
                    //ViewBag.message = "There are no items in your basket. please order something before coming to checkout page.";
                    return View();
                }
                Customer currentCustomer=(Customer)Session["customer"];
                if (currentCustomer == null)
                {
                    ViewBag.error="emptyCustomer";
                     return View();
                }
                double totalPrice=allOrderItems.Sum(c => c.price);
                int[] allOrderItemsProductsIDs = allOrderItems.Select(c => c.productID).ToArray();
                List<Product> allProducts = db.Products.Where(c => allOrderItemsProductsIDs.Contains(c.Id)).ToList();
                ViewBag.allProducts=allProducts;
                ViewBag.totalPrice=totalPrice;
                return View(currentCustomer);

            }
            catch (Exception e) {
                throw new Exception("There is an error please try again");
            }
           }//end of CheckoutPage

        public ActionResult ConfirmCheckout()
        {
            try
            {
                List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
                ViewBag.allCategories = allCategories;
                Customer currentCustomer = (Customer)Session["customer"];
                List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
                double totalPrice = allItems.Sum(x => x.price);
                Order newOrder = new Order() { address = currentCustomer.Address, town = currentCustomer.City, postcode = currentCustomer.Postcode, notes = "", country = currentCustomer.Country, custName = currentCustomer.CustName, orderDate = DateTime.Now, customerID = currentCustomer.CustomerID, phone = currentCustomer.Phone, deliverycost = 3, companyID = 1, totalPrice = totalPrice };
                db.Orders.Add(newOrder);
                db.SaveChanges();
                allItems.ForEach(x => x.orderID = newOrder.orderID);
                db.OrderItems.AddRange(allItems);
                Session["OrderItem"]=null;
                return View();
            }
            catch (Exception e) {
             return View();
            }
        }


        public ActionResult ChangeAddress(Customer newAddress)
        {
            try
            {

                List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
                ViewBag.allCategories = allCategories;
                Customer currentCustomer = (Customer)Session["customer"];
                //add check here if session is empty

                currentCustomer.Address=newAddress.Address;
                currentCustomer.City=newAddress.City;
                currentCustomer.Country=newAddress.Country;
                currentCustomer.Phone = newAddress.Phone;
                currentCustomer.CustName=newAddress.CustName;
                Session["customer"]=currentCustomer;
                /*
                List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];
                double totalPrice = allItems.Sum(x => x.price);
                Order newOrder = new Order() { address = currentCustomer.Address, town = currentCustomer.City, postcode = currentCustomer.Postcode, notes = "", country = currentCustomer.Country, custName = currentCustomer.CustName, orderDate = DateTime.Now, customerID = currentCustomer.CustomerID, phone = currentCustomer.Phone, deliverycost = 3, companyID = 1, totalPrice = totalPrice };
                db.Orders.Add(newOrder);
                db.SaveChanges();
                allItems.ForEach(x => x.orderID = newOrder.orderID);
                db.OrderItems.AddRange(allItems);
                Session["OrderItem"] = null;
                */
                return RedirectToAction("CheckoutPage");
            }
            catch (Exception e)
            {
                Console.WriteLine("error in Change address");
                return View();

            }
        }



    }// end of the controller


}