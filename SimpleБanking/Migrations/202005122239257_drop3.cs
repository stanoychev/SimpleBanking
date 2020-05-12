namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "Customer_CustomerId", "dbo.Customers");
            DropIndex("dbo.Customers", new[] { "User" });
            DropIndex("dbo.Transactions", new[] { "From_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "To_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "Customer_CustomerId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Transactions");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.TransactionId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        User = c.String(nullable: false, maxLength: 40),
                        Pin = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateIndex("dbo.Transactions", "Customer_CustomerId");
            CreateIndex("dbo.Transactions", "To_CustomerId");
            CreateIndex("dbo.Transactions", "From_CustomerId");
            CreateIndex("dbo.Customers", "User", unique: true);
            AddForeignKey("dbo.Transactions", "Customer_CustomerId", "dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers", "CustomerId");
        }
    }
}
