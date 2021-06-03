using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class FormTypeViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class SearchFormType
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}