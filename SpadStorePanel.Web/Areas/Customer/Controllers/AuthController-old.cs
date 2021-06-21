﻿//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using SpadStorePanel.Core.Models;
//using SpadStorePanel.Core.Utility;
//using SpadStorePanel.Infrastructure.Helpers;
//using SpadStorePanel.Infrastructure.Repositories;
//using SpadStorePanel.Web.ViewModels;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;

//namespace SpadStorePanel.Web.Areas.Customer.Controllers
//{
//    [Authorize]
//    public class AuthController : Controller
//    {
//        private ApplicationSignInManager _signInManager;
//        private ApplicationUserManager _userManager;
//        private UsersRepository _userRepo;

//        public AuthController()
//        {
//        }

//        public AuthController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, UsersRepository userRepo)
//        {
//            UserManager = userManager;
//            SignInManager = signInManager;
//            UserRepo = userRepo;
//        }
//        public UsersRepository UserRepo
//        {
//            get
//            {
//                return _userRepo ?? new UsersRepository();
//            }
//            private set
//            {
//                _userRepo = value;
//            }
//        }
//        public ApplicationSignInManager SignInManager
//        {
//            get
//            {
//                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
//            }
//            private set
//            {
//                _signInManager = value;
//            }
//        }

//        public ApplicationUserManager UserManager
//        {
//            get
//            {
//                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }

//        //
//        // GET: /Auth/Login
//        [AllowAnonymous]
//        public ActionResult Login(string returnUrl)
//        {
//            ViewBag.ReturnUrl = returnUrl;
//            return View();
//        }
//        [AllowAnonymous]
//        public ActionResult LoginPartial()
//        {
//            return PartialView();
//        }
//        [AllowAnonymous]
//        public ActionResult RegisterPartial()
//        {
//            return PartialView();
//        }
//        [AllowAnonymous]
//        public ActionResult AccessDenied(string returnUrl = null)
//        {
//            ViewBag.ReturnUrl = "/Admin/Dashboard/Index";
//            if (!string.IsNullOrEmpty(returnUrl))
//                ViewBag.ReturnUrl = returnUrl;
//            return View();
//        }
//        //
//        // POST: /Auth/Login
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Login(ViewModels.LoginViewModel model, string returnUrl)
//        {
//            if (!ModelState.IsValid)
//            {
//                //return View(model);

//                return RedirectToAction("Index", "Home", new { area = "" });
//            }

//            // This doesn't count login failures towards Auth lockout
//            // To enable password failures to trigger Auth lockout, change to shouldLockout: true
//            var user = UserManager.FindByName(model.UserName);
//            if (user == null)
//            {
//                ViewBag.LoginError = "نام کاربری وارد شده صحیح نیست.";
//                //ModelState.AddModelError(string.Empty, "نام کاربری وارد شده صحیح نیست.");
//                //return View(model);

//                return RedirectToAction("Index", "Home", new { area = "" });
//            }

//            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
//            switch (result)
//            {
//                case SignInStatus.Success:
//                    return RedirectToAction("Index","Home",new { area=""}); // Or use returnUrl
//                case SignInStatus.LockedOut:
//                    return View("Lockout");
//                case SignInStatus.RequiresVerification:
//                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
//                case SignInStatus.Failure:
//                default:
//                    ViewBag.LoginError = "نام کاربری یا رمز عبور وارد شده صحیح نیست.";
//                    //ModelState.AddModelError("", "نام کاربری وارد شده صحیح نیست.");
//                    //return View(model);

//                    return RedirectToAction("Index", "Home", new { area = "" });
//            }
//        }

//        //
//        // GET: /Auth/Register
//        [AllowAnonymous]
//        public ActionResult Register()
//        {
//            return View();
//        }

//        //
//        // POST: /Auth/Register
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Register(RegisterCustomerViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                #region Check for duplicate username or email
//                if (UserRepo.EmailExists(model.Email))
//                {
//                    ViewBag.RegisterError = "ایمیل قبلا ثبت شده.";
//                    //ModelState.AddModelError("", "ایمیل قبلا ثبت شده");
//                    //return View(model);
//                    return RedirectToAction("Index", "Home", new { area = "" });
//                }
//                #endregion

//                var user = new User { UserName = model.Email, Email = model.Email, FirstName ="مشتری", LastName = "مشتری"};
//                UserRepo.CreateUser(user, model.Password);
//                if (user.Id != null)
//                {
//                    // Add Customer Role
//                    UserRepo.AddUserRole(user.Id, StaticVariables.CustomerRoleId);

//                    // Add Customer
//                    var customer = new Core.Models.Customer()
//                    {
//                        UserId = user.Id,
//                        IsDeleted = false
//                    };
//                    UserRepo.AddCustomer(customer);
//                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

//                    // For more information on how to enable Auth confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
//                    // Send an email with this link
//                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
//                    // var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
//                    // await UserManager.SendEmailAsync(user.Id, "Confirm your Auth", "Please confirm your Auth by clicking <a href=\"" + callbackUrl + "\">here</a>");

//                    return RedirectToAction("Index", "Home", new { area = "" });
//                }
//            }

//            //return View(model);
//            // If we got this far, something failed, redisplay form
//            return RedirectToAction("Index", "Home", new { area = "" });
//        }
//        //
//        // GET: /Auth/ForgotPasswordConfirmation
//        [HttpPost]
//        public ActionResult LogOff()
//        {
//            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
//            return RedirectToAction("Index", "Home", new { area = "" });
//        }
//        // GET: /Account/ExternalLoginFailure
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (_userManager != null)
//                {
//                    _userManager.Dispose();
//                    _userManager = null;
//                }

//                if (_signInManager != null)
//                {
//                    _signInManager.Dispose();
//                    _signInManager = null;
//                }
//            }

//            base.Dispose(disposing);
//        }


//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await UserManager.FindByEmailAsync(model.Email);
//                if (user == null)
//                {
//                    // Don't reveal that the user does not exist or is not confirmed
//                    return View("ForgotPasswordConfirmation");
//                }

//                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
//                // Send an email with this link
//                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
//                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
//                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
//                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
//                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
//                var callbackUrl = Url.Action(
//                   "ResetPassword", "Account",
//                   new { userId = user.Id, code = code },
//                   protocol: Request.Url.Scheme);
//                var emailForm = new Email.EmailFormModel
//                {
//                    FromName = "SpadStorePanel Way Team",
//                    FromEmail = "SpadStorePanelWayTeam@gmail.com",
//                    ToEmail = user.Email,
//                    Subject = "بروز رسانی رمز عبور",
//                    Message = "با استفاده از این <a href=\"" + callbackUrl + "\">لینک</a> میتوانید رمز عبور خود را بروز رسانی کنید"
//                };
//                await Task.Run(async () => await Email.SendEmail(emailForm));
//                return View("ForgotPasswordConfirmation");
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        #region Helpers
//        // Used for XSRF protection when adding external logins
//        private const string XsrfKey = "XsrfId";

//        private IAuthenticationManager AuthenticationManager
//        {
//            get
//            {
//                return HttpContext.GetOwinContext().Authentication;
//            }
//        }

//        private void AddErrors(IdentityResult result)
//        {
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError("", error);
//            }
//        }

//        private ActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            return RedirectToAction("Index", "Dashboard");
//        }
//        internal class ChallengeResult : HttpUnauthorizedResult
//        {
//            public ChallengeResult(string provider, string redirectUri)
//                : this(provider, redirectUri, null)
//            {
//            }

//            public ChallengeResult(string provider, string redirectUri, string userId)
//            {
//                LoginProvider = provider;
//                RedirectUri = redirectUri;
//                UserId = userId;
//            }

//            public string LoginProvider { get; set; }
//            public string RedirectUri { get; set; }
//            public string UserId { get; set; }

//            public override void ExecuteResult(ControllerContext context)
//            {
//                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
//                if (UserId != null)
//                {
//                    properties.Dictionary[XsrfKey] = UserId;
//                }
//                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
//            }
//        }
//        #endregion
//    }
//}