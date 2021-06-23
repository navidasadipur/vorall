namespace SpadStorePanel.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiscountCodeModelAndAddSomePropertiesToInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscountCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiscountCodeStr = c.String(),
                        ActivationStartDate = c.DateTime(nullable: false),
                        ActivationEndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Value = c.Long(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        InsertUser = c.String(),
                        InsertDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.Invoices", "DiscountAmount", c => c.Long(nullable: false));
            AddColumn("dbo.Invoices", "TotalPriceBeforeDiscount", c => c.Long(nullable: false));
            AddColumn("dbo.Invoices", "CompanyName", c => c.String());
            AddColumn("dbo.Invoices", "Country", c => c.String());
            AddColumn("dbo.Invoices", "City", c => c.String());
            AddColumn("dbo.Invoices", "PostalCode", c => c.String(maxLength: 50));
            AddColumn("dbo.Invoices", "Email", c => c.String());
            AddColumn("dbo.Invoices", "Description", c => c.String());
            AddColumn("dbo.Invoices", "DiscountCodeId", c => c.Int());
            CreateIndex("dbo.Invoices", "DiscountCodeId");
            AddForeignKey("dbo.Invoices", "DiscountCodeId", "dbo.DiscountCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "DiscountCodeId", "dbo.DiscountCodes");
            DropForeignKey("dbo.DiscountCodes", "CustomerId", "dbo.Customers");
            DropIndex("dbo.DiscountCodes", new[] { "CustomerId" });
            DropIndex("dbo.Invoices", new[] { "DiscountCodeId" });
            DropColumn("dbo.Invoices", "DiscountCodeId");
            DropColumn("dbo.Invoices", "Description");
            DropColumn("dbo.Invoices", "Email");
            DropColumn("dbo.Invoices", "PostalCode");
            DropColumn("dbo.Invoices", "City");
            DropColumn("dbo.Invoices", "Country");
            DropColumn("dbo.Invoices", "CompanyName");
            DropColumn("dbo.Invoices", "TotalPriceBeforeDiscount");
            DropColumn("dbo.Invoices", "DiscountAmount");
            DropTable("dbo.DiscountCodes");
        }
    }
}
