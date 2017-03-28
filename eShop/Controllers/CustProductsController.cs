using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eShop.Model;


namespace eShop.Controllers
{
    public class CustProductsController : Controller
    {
        private FirdoosModel db = new FirdoosModel();


        public ActionResult ViewBasket()
        {
            try
            {
                List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();

                ViewBag.allCategories = allCategories;
                if (Session["OrderItem"] == null)
                {

                    ViewBag.message = "There are no items in your basket.";
                    return View();

                }
                else
                {
                    List<OrderItem> allOrderItems = (List<OrderItem>)Session["OrderItem"];

                    int[] allOrderItemsProductsIDs = allOrderItems.Select(c => c.ProductId).ToArray();
                    if (!allOrderItems.Any() || allOrderItems == null)
                    {
                        ViewBag.message = "There are no items in your basket.";
                        return View();
                    }

                    var allProducts = db.Products.Join(db.ProductImages,product=>product.Id, ProductImage=> ProductImage.ProductId,(Product, ProductImage)=>new ProjectData.ProductImageBean{ Product=Product, ProductImage = ProductImage }).Where(c => (allOrderItemsProductsIDs.Contains(c.Product.Id))&& c.ProductImage.Main==true).ToList();
                   // List<ProductImage> images=db.ProductImages.Where(c=>(allOrderItemsProductsIDs.Contains(c.ProductId)) && (c.Main == true)).ToList();

                    if (!allProducts.Any() || allProducts == null)
                    {
                        return HttpNotFound();
                    }
                    
                    else
                    {
                       // ViewBag.images = images;
                        ViewBag.allProducts = allProducts;
                        if (Request.IsAjaxRequest())
                        {

                            return (PartialView(allOrderItems));
                        }
                        return View(allOrderItems);
                    }
                }
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }

        }


        // GET: CustProduct
        [HttpPost]
        [HandleError()]
        public ActionResult OrderProduct(string Id)
        {
            int productID = 0;
            if (!Int32.TryParse(Id, out productID))
            {
                throw new Exception("product ID id not an Integer");
            }
            Product product = db.Products.Single(c => c.Id == productID);
            if (product == null)
            {
                throw new Exception("product does not exist");
            }
            List<OrderItem> list = new List<OrderItem>();
            int count = 0;
            if (Session["OrderItem"] != null)
            {
                list = (List<OrderItem>)Session["OrderItem"];
                count = (int)Session["itemCount"];
            }
            list.Add(new OrderItem() { Id = count, Quantity = 1, Price = product.Price, ProductId = product.Id });
            count++;
            Session["OrderItem"] = list;
            Session["itemCount"] = count;
           // List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            //List<Product> allProducts = db.Products.Where(c => c.CategoryId == product.CategoryId).ToList();
           // ViewBag.allCategories = allCategories;
           // return View(allProducts);
           return RedirectToAction("ViewCategory",new { id=product.CategoryId });
        }


        // GET: CustProduct
        public ActionResult ViewAll()
        {
            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            List<int> allCatIDs = db.Categories.Where(c => c.CompanyId == 1).Select(c => c.Id).ToList();
            List<Product> allProducts = db.Products.Where(c => allCatIDs.Contains(c.CategoryId)).ToList();
            if (!allProducts.Any() || allProducts == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {

                return (PartialView(allProducts));
            }
            return View(allProducts);
        }

        public ActionResult ViewCategory(int id)
        {

            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            List<Product> allProducts = db.Products.Where(c => c.CategoryId == id).ToList();
            List<int> allProductsIds=allProducts.Select(c=>c.Id).ToList();
            List<ProductImage> images =db.ProductImages.Where(c=>allProductsIds.Contains(c.ProductId)).Where(c => c.Main==true).ToList();
            // todo: check that each image has only one main photo and if it doesn't have any main photo then display the first one.
            //images = allProductsIds.Select(pid => images.First(i => i.ProductId == pid)).ToList();
            if (!allProducts.Any() || allProducts == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.allCategories = allCategories;
                ViewBag.images = images;
                if (Request.IsAjaxRequest())
                {
                    return (PartialView(allProducts));
                }
                return View(allProducts);
            }
        }

        public ActionResult ViewProduct(int id)
        {
            if (id == 0)
            {
                //Throw error
            }
      
 
          
            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            Product product = db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            
            else
            {
                List<ProductImage> images = db.ProductImages.Where(c => id == c.ProductId).ToList();
                ViewBag.images=images;
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
            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            if (!allCategories.Any())
            {
                return HttpNotFound();
            }
            ViewBag.allCategories = allCategories;
            if (Request.IsAjaxRequest())
            {

                return (PartialView());
            }
            return View();
        }

        // GET: CustProduct
        [HttpPost]
        public ActionResult RemoveFromBasket(int id)
        {
            try
            {
                List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
                if (!allCategories.Any())
                {
                    throw new Exception();
                }
                ViewBag.allCategories = allCategories;
                if (Session["OrderItem"] == null)
                {
                    throw new Exception();
                }

                List<OrderItem> allOrderItems = (List<OrderItem>)Session["OrderItem"];

                OrderItem removedItem = allOrderItems.FirstOrDefault(c => c.Id == id);
                if (removedItem == null)
                {
                    throw new Exception();
                }
                allOrderItems.Remove(removedItem);
                if (allOrderItems.Count == 0)
                {
                    return Json(new { empty = true, message = "There are no items in your basket." });
                }
                int[] allOrderItemsProductsIDs = allOrderItems.Select(c => c.ProductId).ToArray();
                //List<Product> allProducts = db.Products.Where(c => allOrderItemsProductsIDs.Contains(c.Id)).ToList();
                var allProducts = db.Products.Join(db.ProductImages, product => product.Id, ProductImage => ProductImage.ProductId, (Product, ProductImage) => new ProjectData.ProductImageBean { Product = Product, ProductImage = ProductImage }).Where(c => (allOrderItemsProductsIDs.Contains(c.Product.Id)) && c.ProductImage.Main == true).ToList();
                ViewBag.allProducts = allProducts;
                return Json(new { empty = false, message = "An item was removed from your basket" });
            }
            catch (Exception e)
            {
                throw new Exception();
            }

        }



    }
}