using ESS_Web_Application.Entity;
using ESS_Web_Application.Helper;
using ESS_Web_Application.Models;
using ESS_Web_Application.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public class AccountService : IAccountService
    {
        IAccountRepository _accountRepo = new AccountRepository();
        public DataTable Login(LoginViewModel model)
        {
            DataTable dt = new DataTable();

            Hashtable ht = new Hashtable();
            ht.Add("@UserName", model.Email);
            ht.Add("@password", clsEncryption.EncryptData(model.Password));

            dt = _accountRepo.Login(ht);
            return dt;

        }
        public DataTable GetAllMenu(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_User_FormTypes", ht).Tables[0];
        }
    }
}