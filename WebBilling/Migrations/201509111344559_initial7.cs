namespace WebInvoicing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        customerID = c.String(),
                        name = c.String(),
                        date = c.DateTime(nullable: false),
                        paid = c.Boolean(nullable: false),
                        PDFFileID = c.Int(nullable: false),
                        customer_userName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserAccount", t => t.customer_userName)
                .ForeignKey("dbo.PDFFile", t => t.PDFFileID, cascadeDelete: true)
                .Index(t => t.PDFFileID)
                .Index(t => t.customer_userName);
            
            CreateTable(
                "dbo.UserAccount",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        hashPassword = c.String(),
                        permissionGroup = c.String(),
                    })
                .PrimaryKey(t => t.userName);
            
            CreateTable(
                "dbo.PDFFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "PDFFileID", "dbo.PDFFile");
            DropForeignKey("dbo.Invoices", "customer_userName", "dbo.UserAccount");
            DropIndex("dbo.Invoices", new[] { "customer_userName" });
            DropIndex("dbo.Invoices", new[] { "PDFFileID" });
            DropTable("dbo.PDFFile");
            DropTable("dbo.UserAccount");
            DropTable("dbo.Invoices");
        }
    }
}
