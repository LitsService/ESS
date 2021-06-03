using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class UserActionACLMappingHelperClass
    {
        private int UA_ID;
        private string UA_NAME;
        private Dictionary<int, bool> UA_Allow;
        private Dictionary<int, int> UA_DataAccessID;

        public int UserActionID
        {
            get { return UA_ID; }
            set { UA_ID = value; }
        }

        public string UserActionName
        {
            get { return UA_NAME; }
            set { UA_NAME = value; }
        }

        public Dictionary<int, bool> Allow
        {
            get { return UA_Allow; }
            set { UA_Allow = value; }
        }

        public Dictionary<int, int> DataAccessID
        {
            get { return UA_DataAccessID; }
            set { UA_DataAccessID = value; }
        }
    }
}