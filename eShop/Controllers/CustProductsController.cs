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


        public ActionResult ViewBasket()
        {
            try
            {
                List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();

                Session["OrderItem"]=null;
                if (Session["OrderItem"] == null)
                {
                    ViewBag.allCategories = allCategories;
                    ViewBag.message="There are no items in your bag.";
                    return View();

                }
                else
                {
                    List<OrderItem> allOrderItems=(List<OrderItem>)Session["OrderItem"];
                    int[] allOrderItemsProductsIDs=allOrderItems.Select(c => c.productID).ToArray();
                    List<Product> allProducts = db.Products.Where(c =>allOrderItemsProductsIDs.Contains(c.Id)).ToList();

                    if (!allProducts.Any() || allProducts == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        ViewBag.allCategories = allCategories;
                        ViewBag.allProducts=allProducts;
                        if (Request.IsAjaxRequest())
                        {

                            return (PartialView(allOrderItems));
                        }
                        return View(allOrderItems);
                    }
                }
            }
            catch (Exception e) {
              return HttpNotFound();
            }
           
        }


        // GET: CustProduct
        [HttpPost]
        [HandleError()]
        public ActionResult OrderProduct(string Id)
        {

            int productID=0;
            if (!Int32.TryParse(Id, out productID))
            {
                 throw new Exception("product ID id not an Integer");
            }

            Product product=db.Products.Single(c =>c.Id==productID);
            if (product == null) { throw new Exception("product does not exist"); }

            List<OrderItem> list = new List<OrderItem>();
            if (Session["OrderItem"] != null)
            {
                list=(List<OrderItem>)Session["OrderItem"];
               
            }

            list.Add(new OrderItem() { quantity = 1, price = product.Price, productID = product.Id });
            Session["OrderItem"]=list;


           

            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
          
            List<Product> allProducts = db.Products.Where(c => c.categoryID == product.categoryID).ToList();
            ViewBag.allCategories = allCategories;
            return View(allProducts);
            /*0
                Order curre1ntOrder=new Order() { deliverycost=0, phone="", postcode="", totalPrice=0,  town="", customerID=0,
   , notes="", orderDate= DateTime.UtcNow, companyID=1, address="",  country="iraq",  };
                Session["orde*/

            }


        // GET: CustProduct
        public ActionResult ViewAll()
        {
            List<Category> allCategories=db.Categories.Where(c => c.companyID==1).ToList();

            List<int> allCatIDs= db.Categories.Where(c => c.companyID == 1).Select(c =>c.categoryID).ToList();
            List <Product> allProducts=db.Products.Where(c => allCatIDs.Contains(c.categoryID)).ToList();


            if (!allProducts.Any() || allProducts == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest()) {

                return(PartialView(allProducts));
            }
            return View(allProducts);
        }

        public ActionResult ViewCategory(int id)
        {
            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
            //List<int> allCatIDs = db.Categories.Where(c => c.companyID == 1).Select(c => c.categoryID).ToList();
            List<Product> allProducts = db.Products.Where(c => c.categoryID==id).ToList();

            if (!allProducts.Any() || allProducts==null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.allCategories = allCategories;
                if (Request.IsAjaxRequest())
                {

                    return (PartialView(allProducts));
                }
                return View(allProducts);
            }
        }

        public ActionResult ViewProduct(int id)
        {
            if (id == 0 ) {
                //Throw error
            }
            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
            //List<int> allCatIDs = db.Categories.Where(c => c.companyID == 1).Select(c => c.categoryID).ToList();
            Product  product = db.Products.FirstOrDefault(c => c.Id == id);

            if ( product == null)
            {
                return HttpNotFound();
            }
          
            else
            {
                ViewBag.allCategories = allCategories;
                if (Request.IsAjaxRequest())
                {

                    return (PartialView(product));
                }
                return View(product);
            }
        }

        // GET: CustProduct
        public ActionResult HomePage()
        {
            List<Category> allCategories = db.Categories.Where(c => c.companyID == 1).ToList();
            if (!allCategories.Any()) {
              return HttpNotFound();
            }
            ViewBag.allCategories=allCategories;
            if (Request.IsAjaxRequest())
            {

                return (PartialView());
            }
            return View();
        }

    }
}