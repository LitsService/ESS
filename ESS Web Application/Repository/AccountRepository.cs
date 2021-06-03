using ESS_Web_Application.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public class AccountRepository : IAccountRepository
    {
        DBContext dbcontext = new DBContext();

        public DataTable Login(Hashtable hs)
        {
            return DBContext.GetDataSet("sp_User_Login", hs).Tables[0];
        }
    }
}