namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationsupdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "From_CustomerId", c => c.Int());
            AddColumn("dbo.Transactions", "To_CustomerId", c => c.Int());
            CreateIndex("dbo.Transactions", "From_CustomerId");
            CreateIndex("dbo.Transactions", "To_CustomerId");
            AddForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers", "CustomerId");
            AddForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "To_CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "From_CustomerId", "dbo.Customers");
            DropIndex("dbo.Transactions", new[] { "To_CustomerId" });
            DropIndex("dbo.Transactions", new[] { "From_CustomerId" });
            DropColumn("dbo.Transactions", "To_CustomerId");
            DropColumn("dbo.Transactions", "From_CustomerId");
        }
    }
}
