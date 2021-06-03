using ESS_Web_Application.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public class Repository
    {
        public DataTable GetAllEmployeesWithCompany(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_Get_All_Employees_with_company", ht).Tables[0];
        }
    }
}