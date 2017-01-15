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
    public class AdminOrderItemsController : Controller
    {
        private DbModel db = new DbModel();

        // GET: OrderItems
        public async Task<ActionResult> ViewAll()
        {
            var orderItems = db.OrderItems.Include(o => o.Order).Include(o => o.Product);
            return View(await orderItems.ToListAsync());
        }

        // GET: OrderItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: OrderItems/Create
        public ActionResult Create(Order order)
        {

            ViewBag.orderID =order.orderID;
            ViewBag.productsList=db.Products.Select(r => r.Name);
     
            if (Request.IsAjaxRequest())
            {

                return PartialView();
            }

            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "orderItemID,orderID,productID,price")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.OrderItems.Add(orderItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.orderID = new SelectList(db.Orders, "orderID", "address", orderItem.orderID);
            ViewBag.productID = new SelectList(db.Products, "Id", "Name", orderItem.productID);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.orderID = new SelectList(db.Orders, "orderID", "address", orderItem.orderID);
            ViewBag.productID = new SelectList(db.Products, "Id", "Name", orderItem.productID);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "orderItemID,orderID,productID,price")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.orderID = new SelectList(db.Orders, "orderID", "address", orderItem.orderID);
            ViewBag.productID = new SelectList(db.Products, "Id", "Name", orderItem.productID);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OrderItem orderItem = await db.OrderItems.FindAsync(id);
            db.OrderItems.Remove(orderItem);
            await db.SaveChangesAsync();
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
