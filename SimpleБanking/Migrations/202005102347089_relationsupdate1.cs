namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationsupdate1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Customers");
            AlterColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Customers", "CustomerId");
            CreateIndex("dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Customers", "CustomerId", "dbo.Accounts", "AccountId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "CustomerId", "dbo.Accounts");
            DropIndex("dbo.Customers", new[] { "CustomerId" });
            DropPrimaryKey("dbo.Customers");
            AlterColumn("dbo.Customers", "CustomerId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Customers", "CustomerId");
        }
    }
}
