using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eShop.Models;

namespace eShop.Controllers
{
    public class AdminProductsController : Controller
  {
        private DbModel db =new DbModel();

    // GET: test
    public ActionResult ViewAll()
    {
      if (Request.IsAjaxRequest()) {
         return PartialView(db.Products.ToList());
      }
      return View(db.Products.ToList());
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

      if (Request.IsAjaxRequest())
      {
        return PartialView();
      }
      return View();
    }

    // POST: test/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,Name,Description,Price")] Product product)
    {
      if (ModelState.IsValid)
      {
        db.Products.Add(product);
        db.SaveChanges();
        return RedirectToAction("Index");
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
    public ActionResult Edit([Bind(Include = "Id,Name,Description,Price")] Product product)
    {
      if (ModelState.IsValid)
      {
        db.Entry(product).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
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