using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace DIYVCV.Models
{
    public class DIYVCVDataContext : DbContext
    {
        public DIYVCVDataContext() : base("name=DIYVCVDataContext")
        {

        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Component> Components { get; set; }
    }
}