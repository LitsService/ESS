using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ESS_Web_Application.Models;
using ESS_Web_Application.Services;
using System.Web.Security;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;

namespace ESS_Web_Application.Controllers
{
    [Authorize]
    public class AccountController : Controller, IAccountController
    {
        private SignInManager<ApplicationUser, string> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        IAccountService _accountService = new AccountService();

        //public AccountController()
        //{
        //}

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser, string> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public SignInManager<ApplicationUser, string> SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [AllowAnonymous]
        public ActionResult EmailWFAction(string btnApprove, string type, string ReqID, string StatusId, string UserId, string CompanyId, string username, string hdnGuid, string formtype, string leavetype)
        {
            ViewBag.btnType = btnApprove;
            ViewBag.userid = UserId;
            ViewBag.Requestid = ReqID;
            ViewBag.StatusID = StatusId;
            ViewBag.GUID = UserId;
            ViewBag.Type = type;
            ViewBag.CurrentLevelID = StatusId;
            ViewBag.CompanyId = CompanyId;
            ViewBag.formType = formtype;
            ViewBag.leaveType = leavetype;
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var url = "http://rma-bgapi.ddns.net:6063/api/DemoSales/GetSalesJson";
            var model = _download_serialized_json_data<List<POS>>(url);


            //XmlDocument xml = new XmlDocument();
            ////xml.Load("http://rma-bgapi.ddns.net:6063/api/DemoSales/GetSalesODATA");
            //DataSet ds = new DataSet();
            //string myXMLfile = "http://rma-bgapi.ddns.net:6063/api/DemoSales/GetSalesODATA";
            //ds.ReadXml(myXMLfile);
            //XmlElement root = xml.DocumentElement;
            //XmlNodeList nodes = root.SelectNodes("/response/current_observation");

            //foreach (XmlNode node in nodes)System.Xml.XmlException: 'Data at the root level is invalid. Line 1, position 1.'
            //{
            //    string tempf = node["temp_f"].InnerText;
            //    string tempc = node["temp_c"].InnerText;
            //    string feels = node["feelslike_f"].InnerText;

            //    //label2.Text = tempf;
            //    //label4.Text = tempc;
            //    //label6.Text = feels;
            //}


            if (null != Session["UserID"])
            {
                return View();
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

        }

        private static T _download_serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var dt = _accountService.Login(model);
            Session["UserID"] = "";
            Session["UserName"] = "";
            Session["UserRole"] = "";
            Session["UserCompanyID"] = "";
            Session["MyCulture"] = "";
            Session["FullName"] = "";
            Session["MenuList"] = new List<Menu>();
            if (dt.Rows.Count > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", dt.Rows[0]["ID"].ToString());
                var menylist = _accountService.GetAllMenu(ht);
                List<ESS_Web_Application.Models.Menu> MenuList = new List<ESS_Web_Application.Models.Menu>();
                foreach (DataRow item in menylist.Rows)
                {
                    var obj = new ESS_Web_Application.Models.Menu()
                    {
                        ID = item["ID"].ToString(),
                        Name = item["Name"].ToString(),
                        URL = item["URL"].ToString(),
                        type = item["type"].ToString(),
                    };
                    MenuList.Add(obj);
                }

                Session["MenuList"] = MenuList.Where(a => a.type == "r").ToList();
                Session["SecurityMenuList"] = MenuList.Where(a => a.type == "s").ToList();
                Session["WorkflowMenuList"] = MenuList.Where(a => a.type == "w").ToList();
                Session["RptMenuList"] = MenuList.Where(a => a.type == "rpt").ToList();

                Session["UserID"] = dt.Rows[0]["ID"].ToString();
                try { Session["MyCulture"] = dt.Rows[0]["selLanguage"].ToString(); } catch { }
                try { Session["FullName"] = dt.Rows[0]["FullName"].ToString(); } catch { }
                string userType = "";
                if (Convert.ToBoolean(dt.Rows[0]["IsAdmin"].ToString()) == Convert.ToBoolean("true"))
                {
                    userType = "A";
                    Session["UserRole"] = "Admin";
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        Session["UserRole"] += " , " + dt.Rows[i]["Name"].ToString();
                    }

                    var str = Session["UserRole"].ToString();
                    Session["UserName"] = model.Email;
                }
                else
                {
                    userType = "U";
                    Session["UserRole"] = "User";
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        Session["UserRole"] += " , " + dt.Rows[i]["Name"].ToString();
                    }
                    Session["UserName"] = model.Email;
                }
                Session["UserCompanyID"] = dt.Rows[0]["CompanID"].ToString();
                FormsAuthenticationTicket k = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.AddMinutes(60), false, userType, FormsAuthentication.FormsCookiePath);

                string st = null;
                st = FormsAuthentication.Encrypt(k);
                HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, st);
                Response.Cookies.Add(ck);

                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()) == Convert.ToBoolean("true"))
                {
                    //if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) || Request.QueryString["ReturnUrl"].Contains("LoginPage.aspx"))
                    //{//Response.Redirect("~/Home/Index");
                    return RedirectToAction("Dashboard", "Home");
                    //}
                    //else
                    //{ return RedirectToAction(Request.QueryString["ReturnUrl"]); }
                    //Response.Redirect(Server.UrlDecode(Request.QueryString["ReturnUrl"]));
                }
                else
                {
                    FormsAuthentication.SignOut();
                    //lblInActive.Text = "Your Account Is Not Active.";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                //lblError.Text = "Invalid User Name or Password.";
                return RedirectToAction("Login");
            }
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
        }
        public ActionResult Logout()
        {

            Session["UserID"] = "";
            Session["UserName"] = "";
            Session["UserRole"] = "";
            Session["UserCompanyID"] = "";
            Session["MyCulture"] = "";
            Session["FullName"] = "";

            return RedirectToAction("Login");
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
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

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
    public class POS
    {
        public int ERPSaleID { get; set; }
        public DateTime SessionDate { get; set; }
        public int OutletID { get; set; }
        public string Outlet { get; set; }
        public string TranType { get; set; }
        public string TranSubType { get; set; }
        public string TranSubTypeID { get; set; }
        public double Amount { get; set; }
        public string CrDr { get; set; }
        public int SessionNo { get; set; }
    }
}