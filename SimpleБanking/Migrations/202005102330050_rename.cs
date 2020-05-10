namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename : DbMigration
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
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        TimeOfExecution = c.DateTime(),
                        Account_AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountId)
                .Index(t => t.Account_AccountId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        User = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Pin = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                    })
                .PrimaryKey(t => t.CustomerId)
                .Index(t => t.User, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Account_AccountId", "dbo.Accounts");
            DropIndex("dbo.Customers", new[] { "User" });
            DropIndex("dbo.Transactions", new[] { "Account_AccountId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Transactions");
            DropTable("dbo.Accounts");
        }
    }
}
