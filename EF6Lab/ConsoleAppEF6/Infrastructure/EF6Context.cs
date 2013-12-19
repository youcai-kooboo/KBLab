using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppEF6.Models;

namespace ConsoleAppEF6.Infrastructure
{
    //[DbConfigurationType(typeof(CustomConfiguration))]
    public class EF6Context : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           modelBuilder.Properties<string>()
               .Where(m=>m.Name == "Iso")
               .Configure(m=>m.IsKey().HasColumnOrder(1));

           modelBuilder.Properties<string>()
              .Where(m => m.Name == "Code")
              .Configure(m => m.IsKey().HasColumnOrder(2));
        }
    }
}
