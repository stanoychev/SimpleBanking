namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbdesignchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        User = c.String(nullable: false, maxLength: 1024),
                        Pin = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.CustomerId)
                .Index(t => t.User, unique: true);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        TimeOfExecution = c.DateTime(),
                        From_CustomerId = c.Int(),
                        To_CustomerId = c.Int(),
                        Customer_CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Customers", t => t.From_CustomerId)
                .ForeignKey("dbo.Customers", t => t.To_CustomerId)
                .ForeignKey("dbo.Customers", t => t.Customer_CustomerId)
                .Index(t => t.From_CustomerId)
                .Index(t => t.To_CustomerId)
                .Index(t => t.Customer_CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Customer_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers");
            DropIndex("dbo.Transactions", new[] { "Customer_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "To_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "From_CustomerId" });
            DropIndex("dbo.Customers", new[] { "User" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Customers");
        }
    }
}
