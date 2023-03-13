using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PersonBalance.Web.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {}

        public ApplicationDbContext(DbConnection connection) : base(connection, false)
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
    }
}