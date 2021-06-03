using ESS_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Configurations
{
    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            ToTable("Comapny");
            Property(g => g.Name).IsRequired().HasMaxLength(50);
            Property(g => g.CompanyGuid).IsRequired();

        }
    }
}