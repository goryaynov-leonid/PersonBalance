namespace PersonBalance.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "RowVersion");
        }
    }
}
