using eShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eShop.Controllers
{
  public class ProductsController : Controller
  {

    private DbModel db = new DbModel();
    // GET: Default
    public ActionResult Index()
    {
      
        return View();
      }




    public ActionResult saveProduct()
    {

      String ProductName="";


      return View();
    }
  }
}
