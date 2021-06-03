using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public interface IAccountRepository
    {
        DataTable Login(Hashtable hs);
    }
}