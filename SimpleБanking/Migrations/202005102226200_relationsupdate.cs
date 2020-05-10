namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationsupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Customers", new[] { "Account_Id" });
            AddColumn("dbo.Transactions", "Account_Id", c => c.Int());
            CreateIndex("dbo.Transactions", "Account_Id");
            AddForeignKey("dbo.Transactions", "Account_Id", "dbo.Accounts", "Id");
            DropColumn("dbo.Customers", "Account_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Account_Id", c => c.Int());
            DropForeignKey("dbo.Transactions", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "Account_Id" });
            DropColumn("dbo.Transactions", "Account_Id");
            CreateIndex("dbo.Customers", "Account_Id");
            AddForeignKey("dbo.Customers", "Account_Id", "dbo.Accounts", "Id");
        }
    }
}
