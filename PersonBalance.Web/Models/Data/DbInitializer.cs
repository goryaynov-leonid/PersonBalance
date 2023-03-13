using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace PersonBalance.Web.Models.Data
{
    public class DbInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            DbMigrationsConfiguration configuration = new DbMigrationsConfiguration()
            {
                MigrationsAssembly = typeof(ApplicationDbContext).Assembly,
                ContextType = typeof(ApplicationDbContext),
                AutomaticMigrationsEnabled = true,
            };

            DbMigrator dbMigrator = new DbMigrator(configuration);
            dbMigrator.Update(null);
        }
    }
}