using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using ESS_Web_Application.Services;
using ESS_Web_Application.ViewModels;
using System.IO;
using System.Web.UI.WebControls;
using ESS_Web_Application.Helper;
using ESS_Web_Application.Infrastructure;
using ESS_Web_Application.Repository;

namespace ESS_Web_Application.Controllers
{
    [AuthorizeActionFilter]
    public partial class UserManagedController : Controller
    {
        public static Hashtable htSearchParams = null;
        IManagedUsersService _mangedUser = new ManagedUsersService();
        IManagedUsersRespository _manageuserrepository = new ManagedUsersRespository();

        //private readonly IManagedUsersService _mangedUser;
        //public UserManagedController(IManagedUsersService mangedUser)
        //{
        //    _mangedUser = mangedUser;
        //}

        // GET: Admin/UserManaged

        #region Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "ManageUsers"))
            {
                var dt = _manageuserrepository.GetCompany();
                var Company = new List<Company>();
                while (dt.Read())
                {
                    Company User = new Company()
                    {
                        Name = dt["Name"].ToString(),
                        CompanyGuid = Guid.Parse(dt["CompanyGuid"].ToString()),
                    };
                    Company.Add(User);
                }
                ViewBag.CompanList = Company;
                ViewBag.Roles = GetAllRoles();
                htSearchParams = new Hashtable();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetUsers(string Employee, string RoleId)
        {
            htSearchParams = new Hashtable();
            if ((!string.IsNullOrEmpty(RoleId) || !string.IsNullOrEmpty(Employee)) && RoleId != "All")
            {
                htSearchParams.Add("@RoleID", string.IsNullOrEmpty(RoleId) ? 0 : int.Parse(RoleId));
                if (string.IsNullOrEmpty(Employee))
                {
                    htSearchParams.Add("@UserName", "all");
                }
                else
                {
                    htSearchParams.Add("@UserName", Employee.Trim());
                }
            }
            htSearchParams.Add("@CompanyId", Session["UserCompanyID"]);

            var Users = _mangedUser.GetAllUsers(htSearchParams);
            return Json(Users);
        }
        public List<DropDownBindViewModel> GetAllRoles()
        {
            htSearchParams = new Hashtable();
            htSearchParams.Add("@CompanyId", Guid.Parse(Session["UserCompanyID"].ToString()));
            var Roles = _mangedUser.GetRoles(htSearchParams);
            return Roles;
        }

        public JsonResult GetUserCompany(string UserId)
        {
            htSearchParams = new Hashtable();
            htSearchParams.Add("@UserID", string.IsNullOrEmpty(UserId) ? 0 : int.Parse(UserId));
            var Users = _mangedUser.GetUserCompany(htSearchParams);
            return Json(Users);

        }
        public JsonResult GetEmployee(string Type,string CompanyId)
        {

            List<DropDownBindViewModel> dp = new List<DropDownBindViewModel>();
            if (Type == "New")
            {
                dp = _mangedUser.GetEmployeeForReg();
            }
            else if (Type == "Update")
            {
                dp = _mangedUser.GetEmployeeForUpdate();
            }
            return Json(dp);
        }
        public JsonResult UserInsertUpdate(string Operation, string EmployeeID, string PasswordHash, string ID,
            string Username, string IsAdmin, string IsActive, string UserRoleId, string ImgBody, List<string> CompaniesSelectedList,string Company
            )
        {
            if (!string.IsNullOrEmpty(ID))
            {
                Operation = "Update";
            }

            string result = _mangedUser.InsertUpdate(Operation, EmployeeID, PasswordHash, ID,
             Username, IsAdmin, IsActive, UserRoleId.Split(','), "", Guid.Parse(/*Session["UserCompanyID"].ToString()*/Company), CompaniesSelectedList);
            return Json(result);
        }
        public JsonResult DeleteUser(string ID)
        {
            _mangedUser.DeleteUser(ID);
            return Json("");
        }
        public JsonResult UpdateUserPassword(string ID, string Password)
        {
            int UserID = int.Parse(ID);

            if (!string.IsNullOrEmpty(Password) /*&& txtPwd.Text != OldPassword*/ && UserID > 0)
            {
                _mangedUser.UpdateUserPassword(UserID, Password);
            }
            return Json("");
        }
        [HttpPost]
        public void Upload()
        {

            if (Request.Files.Count != 0)
            {

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    file.SaveAs(path);
                }

            }

        }
        private void Inser_or_Update_ProfImage(FileUpload imgUploadCtrl, string UserID)
        {
            if (imgUploadCtrl.HasFile)
            {
                byte[] ImageByteArray = null;
                ImageByteArray = ConvertImageToByteArray(imgUploadCtrl);
                //  int imageId = clsCommon.SaveImageToDB(ImageByteArray, UserID);
            }

        }
        private byte[] ConvertImageToByteArray(FileUpload fuImage)
        {
            byte[] ImageByteArray;
            try
            {
                MemoryStream ms = new MemoryStream(fuImage.FileBytes);
                ImageByteArray = ms.ToArray();
                return ImageByteArray;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Roles
        public ActionResult Roles()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "ManageRoles"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetRoles()
        {
            htSearchParams = new Hashtable();
            htSearchParams.Add("@CompanyId", Session["UserCompanyID"]);
            var Roles = _mangedUser.GetAllRoles(htSearchParams);
            return Json(Roles);
        }
        public JsonResult RoleInsertUpdate(string Operation, string ID, string Name, string IsActive)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                Operation = "Update";
            }
            string result = _mangedUser.InsertUpdateRole(Operation, ID, Name, IsActive, Guid.Parse(Session["UserCompanyID"].ToString()));
            return Json(result);
        }
        public JsonResult DeleteRole(string ID)
        {
            _mangedUser.DeleteRole(ID);
            return Json("");
        }
        #endregion

        #region ACL
        public ActionResult ACL()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "ManageForms"))
            {
                htSearchParams = new Hashtable();
                htSearchParams.Add("@CompanyId", Guid.Parse(Session["UserCompanyID"].ToString()));
                var ACL = _mangedUser.GetAcl(htSearchParams);
                ViewBag.Roles = GetAllRoles();
                return View(ACL);
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult ACLInsertUpdate(string ActionId, string RoleId, string Allow, string Access)
        {
            _mangedUser.ACLInsertUpdate(ActionId, RoleId, Allow, Access);
            return Json("");
        }
        #endregion
    }
}