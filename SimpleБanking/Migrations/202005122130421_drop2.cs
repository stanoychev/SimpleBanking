namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "CustomerId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "Account_AccountId", "dbo.Accounts");
            DropIndex("dbo.Customers", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "User" });
            DropIndex("dbo.Transactions", new[] { "From_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "To_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "Account_AccountId" });
            DropTable("dbo.Accounts");
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
                        Account_AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        User = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Pin = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Name = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateIndex("dbo.Transactions", "Account_AccountId");
            CreateIndex("dbo.Transactions", "To_CustomerId");
            CreateIndex("dbo.Transactions", "From_CustomerId");
            CreateIndex("dbo.Customers", "User", unique: true);
            CreateIndex("dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Transactions", "Account_AccountId", "dbo.Accounts", "AccountId");
            AddForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Customers", "CustomerId", "dbo.Accounts", "AccountId");
        }
    }
}
