using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;


namespace DIYVCV.Models
{
    public class DIYVCVDataContext : DbContext
    {
/*        public DIYVCVDataContext() : base("name=DIYVCVDataContext")
        {

        }*/

        /* AWS Connection*/
        public DIYVCVDataContext()
            : base(AWSConnector.GetRDSConnectionString())
        {
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Component> Components { get; set; }
    }
}