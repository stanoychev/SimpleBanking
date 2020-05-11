namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class build : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        User = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Pin = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Accounts", t => t.CustomerId)
                .Index(t => t.CustomerId)
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
                        Account_AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Customers", t => t.From_CustomerId)
                .ForeignKey("dbo.Customers", t => t.To_CustomerId)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountId)
                .Index(t => t.From_CustomerId)
                .Index(t => t.To_CustomerId)
                .Index(t => t.Account_AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Account_AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "CustomerId", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "Account_AccountId" });
            DropIndex("dbo.Transactions", new[] { "To_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "From_CustomerId" });
            DropIndex("dbo.Customers", new[] { "User" });
            DropIndex("dbo.Customers", new[] { "CustomerId" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Customers");
            DropTable("dbo.Accounts");
        }
    }
}
