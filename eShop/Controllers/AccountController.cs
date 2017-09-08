using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel.PeerResolvers;
using eShop.Model;


namespace eShop.Controllers
{

    
    public class AccountController : Controller
    {
        private AppSignInManager _signInManager;
        private AppUserManager _userManager;
        private CustomUserStore _userStore;
        private FirdoosModel db = new FirdoosModel();

        public AppSignInManager SignInManager
        {
            
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public CustomUserStore UserStore
        {
            get
            {
                return _userStore ?? HttpContext.GetOwinContext().GetUserManager<CustomUserStore>();
            }
            private set
            {
                _userStore = value;
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
           // return View(await db.Categories.ToListAsync());
            return View();
        }
        // GET: Account
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterAndLoginViewModel  model)
        {
            if (ModelState.IsValid)
            {
              
                var user = new AppUser { UserName = model.Email, Email = model.Email,City = model.City,Address=model.Address ,Country=model.Country,Postcode=model.Postcode,CompanyId = 1};
                var result = await UserManager.CreateAsync(user, model.Password);
                await UserManager.AddToRoleAsync(user.Id, "Admin");
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    string code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id,"djhdkfhsi");
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                       new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id,
                       "Confirm your account", "Please confirm your account by clicking <a href=\""
                       + callbackUrl + "\">here</a>");
                  
                    return RedirectToAction("DashBoard", "AdminMain");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: Account
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCustomer(RegisterAndLoginViewModel model)
        {

            List<Category> allCategories = db.Categories.Where(c => c.CompanyId == 1).ToList();
            ViewBag.allCategories = allCategories;
            if (ModelState.IsValid)
            {
                var user = new AppUser { FullName = model.FullName,UserName = model.PhoneNumber, PhoneNumber = model.PhoneNumber, Email = model.Email, City = "London", Address = "44-46fieldgate st.", Country = "UK", Postcode = "e1 1es", CompanyId = 1 };

                var checkExist = UserManager.FindByPhoneNumberUserManager(model.PhoneNumber,model.Email);
                if (checkExist)
                {

                    /*ViewBag.sucess="Phone already exist! please login to your account or create another one.";
                    return View(newCustomer);*/
                    return Json(new { success = "PhoneExist", errorMessage = "The phone or email you entered already exist <a href='#' id='login-from-register'>Login</a> to your account" });
                }
                else if (!model.PhoneNumber.Any(c => char.IsDigit(c)) || !model.PhoneNumber.Trim().StartsWith("07"))
                {
                    return Json(new { success = "PhoneWrong", errorMessage = "The phone you entered either contains letters or does not start with 07. Please enter a valid phone number." });
                }
                //model.CompanyId = 1;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Customer");
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    string code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, "djhdkfhsi");
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                       new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id,
                       "Confirm your account", "Please confirm your account by clicking <a href=\""
                       + callbackUrl + "\">here</a>");


                    List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];

                    if (allItems == null || allItems.Count == 0)
                    {
                        return Json(new { success = "yes", url = Url.Action("HomePage", "CustProducts") });
                    }
                    else
                    {
                        return Json(new { success = "yes", url = Url.Action("CheckoutPage", "Checkout") });

                    }

                  //  return RedirectToAction("DashBoard", "AdminMain");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        //
        // GET: /Account/Login
        /*
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        */
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {

            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }
        //
        // POST: /Account/Login
        /*
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(RegisterAndLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, model.RememberMe, shouldLockout: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                return View("Lockout");
                case SignInStatus.RequiresVerification:
                return RedirectToAction("SendCode", new { ReturnUrl = "", RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }
        */

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, model.RememberMe, shouldLockout: false);

            

            switch (result)
            {
                case SignInStatus.Success:

                List<OrderItem> allItems = (List<OrderItem>)Session["OrderItem"];

                if (allItems == null || allItems.Count == 0)
                {
                    return Json(new { success = "yes", url = Url.Action("HomePage", "CustProducts") });
                }
                else
                {
                    return Json(new { success = "yes", url = Url.Action("CheckoutPage", "Checkout") });

                }
                //return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                return View("Lockout");
                case SignInStatus.RequiresVerification:
                return RedirectToAction("SendCode", new { ReturnUrl = "", RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }


    }
}