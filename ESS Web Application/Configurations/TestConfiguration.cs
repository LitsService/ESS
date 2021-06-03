using ESS_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Configurations
{
    public class TestConfiguration : EntityTypeConfiguration<Test>
    {
        public TestConfiguration()
        {
            ToTable("Tests");
            Property(g => g.Name).IsRequired().HasMaxLength(50);
            //Property(g => g.Price).IsRequired().HasPrecision(8, 2);
            //Property(g => g.CategoryID).IsRequired();
        }
    }
}