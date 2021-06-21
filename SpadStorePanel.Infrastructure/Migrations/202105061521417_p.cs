namespace SpadStorePanel.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class p : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ProductCode");
        }
    }
}
