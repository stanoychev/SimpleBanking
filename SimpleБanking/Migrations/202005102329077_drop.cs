namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Customers", new[] { "AccountId" });
            DropIndex("dbo.Customers", new[] { "User" });
            DropIndex("dbo.Transactions", new[] { "Account_Id" });
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
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        TimeOfExecution = c.DateTime(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        AccountId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        User = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Pin = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Transactions", "Account_Id");
            CreateIndex("dbo.Customers", "User", unique: true);
            CreateIndex("dbo.Customers", "AccountId");
            AddForeignKey("dbo.Transactions", "Account_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.Customers", "AccountId", "dbo.Accounts", "Id");
        }
    }
}
