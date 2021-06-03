using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Models
{
    public class Company
    {
        public int CompanyID { get; set; }
        public Guid CompanyGuid { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
    }
}