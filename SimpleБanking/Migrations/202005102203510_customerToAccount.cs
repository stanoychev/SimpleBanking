namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Account_Id", c => c.Int());
            CreateIndex("dbo.Customers", "Account_Id");
            AddForeignKey("dbo.Customers", "Account_Id", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Customers", new[] { "Account_Id" });
            DropColumn("dbo.Customers", "Account_Id");
        }
    }
}
