﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Specialized;
using eShop.Model;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace eShop.Controllers
{
    public class AdminProductsController : Controller
  {
        private FirdoosModel db =new FirdoosModel();

        // GET: test
        public ActionResult ViewAll()
        {
            List<int> allProductsIds = db.Products.Select(c => c.Id).ToList();
            List<ProductImage> images = db.ProductImages.Where(c => allProductsIds.Contains(c.ProductId)).Where(c => c.Main == true).ToList();
            ViewBag.images=images;
            if (Request.IsAjaxRequest())
            {
                return PartialView("View", db.Products.ToList());
            }
            return View("View", db.Products.ToList());
        }
 public ActionResult ViewCategoryProducts(int ? id)
        {

            List<Product> allProducts = db.Products.Where(c => c.CategoryId == id).ToList();
            List<int> allProductsIds = allProducts.Select(c => c.Id).ToList();
            List<ProductImage> images = db.ProductImages.Where(c => allProductsIds.Contains(c.ProductId)).Where(c => c.Main == true).ToList();
            ViewBag.images=images;
            if (Request.IsAjaxRequest())
            {
                return PartialView("View",allProducts);
            }
            return View("View", allProducts);
        }

        // GET: test/Details/5
        public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Product product = db.Products.Find(id);
      if (product == null)
      {
        return HttpNotFound();
      }

      if (Request.IsAjaxRequest())
      {
        return PartialView(product);
      }
      return View(product);
    }

    // GET: test/Create
    public ActionResult Create()
    {

       List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
       ViewBag.allCategories = allCategories;
       if (Request.IsAjaxRequest())
      {
        return PartialView();
      }
      return View();
    }
        public ActionResult UploadPhoto(Product product)
        {

            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            ViewBag.allCategories = allCategories;
            
            if (Request.IsAjaxRequest())
            {
                return PartialView(product);
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult UploadPhoto(object sender, EventArgs e)
        {
            
            int productID = Int32.Parse(Request.Params["Id"]);
            Product currentProduct=db.Products.FirstOrDefault(c=>c.Id==productID);
            if (currentProduct == null) {
                //throw new exception
                // todo:
               
            }

            String mainPhoto=Request.Form["Main"];
            if (mainPhoto == null) {
                mainPhoto="";
            }
            Boolean isMain=false;
            List<ProductImage> images= new List<ProductImage>();
            // todo: Check if the product has uploaded 5 photos then will display danger message and should not display the form.
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase upload = Request.Files[i];
                if (upload != null && upload.ContentLength > 0)
                {
                    String pathOriginals = Server.MapPath("~/product-images/originals");
                    String pathThumbnails = Server.MapPath("~/product-images/Thumbnails");
                    if (!System.IO.Directory.Exists(pathOriginals))
                    {
                        System.IO.Directory.CreateDirectory(pathOriginals);

                    }
                        if (!System.IO.Directory.Exists(pathThumbnails))
                        {
                            System.IO.Directory.CreateDirectory(pathThumbnails);
                        }
                        //changing the name of the photo to productID_i.ext(png,jpeg,...)
                        string fileName = upload.FileName;
                        int lastIndex = fileName.LastIndexOf('.');
                        string fileNameExtention = fileName.Substring(lastIndex + 1);
                        String imageLocation = productID + "_" + i + "." + fileNameExtention;
                        upload.SaveAs(pathOriginals + "\\" + imageLocation);
                        string imageLocationNoExtention = imageLocation.Substring(0, imageLocation.LastIndexOf('.'));
                        ProductImage image = new ProductImage() { ProductId = productID, ImageLocation = imageLocation, Main = false };
                        if (mainPhoto.Equals(fileName))
                        {
                            image.Main = true;
                            isMain = true;
                        };
                        images.Add(image);


                        //creating a thumbnail and save it 
                        using (var srcImage = Image.FromFile(pathOriginals + "\\" + imageLocation))
                        using (var newImage = new Bitmap(200, 300))
                        using (var graphics = Graphics.FromImage(newImage))
                        using (var stream = new MemoryStream())
                        {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphics.DrawImage(srcImage, new Rectangle(0, 0, 200, 300));
                            newImage.Save(pathThumbnails + "\\" + imageLocation);
                            // newImage.Save(pathThumbnails + "\\" + imageLocationNoExtention + ".png", ImageFormat.Png);
                        };
                        //currentProduct.ImageLocation = imageLocation;
                        //db.SaveChanges();
                    }
                
            }
            if (!isMain) {
                images.First().Main=true;
            }
            db.ProductImages.AddRange(images);
            db.SaveChanges();
            return RedirectToAction("Edit",new { id=productID });
        }
        // POST: test/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,Name,Description,Price,categoryID")] Product product)
    {
      if (ModelState.IsValid)
      {
        db.Products.Add(product);
        db.SaveChanges();
        return RedirectToAction("UploadPhoto",product);
      }

      if (Request.IsAjaxRequest())
      {
        return PartialView(product);
      }
      return View(product);
    }

    // GET: test/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Product product = db.Products.Find(id);
      if (product == null)
      {
        return HttpNotFound();
      }

      if (Request.IsAjaxRequest())
      {
        return PartialView(product);
      }

      return View(product);
    }

    // POST: test/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,CategoryId,Name,Description,Price")] Product product)
    {
      if (ModelState.IsValid)
      {
        db.Entry(product).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("ViewAll");
      }

      if (Request.IsAjaxRequest())
      {
        return PartialView(product);
      }
      return View(product);
    }

    // GET: test/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Product product = db.Products.Find(id);
      if (product == null)
      {
        return HttpNotFound();
      }


      if (Request.IsAjaxRequest())
      {
        return PartialView(product);
      }
      return View(product);
    }

    // POST: test/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Product product = db.Products.Find(id);
      db.Products.Remove(product);
      db.SaveChanges();
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}