namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Transactions", name: "CustomerId", newName: "Customer_CustomerId");
            RenameIndex(table: "dbo.Transactions", name: "IX_CustomerId", newName: "IX_Customer_CustomerId");
            DropColumn("dbo.Transactions", "FromId");
            DropColumn("dbo.Transactions", "ToId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "ToId", c => c.Int());
            AddColumn("dbo.Transactions", "FromId", c => c.Int());
            RenameIndex(table: "dbo.Transactions", name: "IX_Customer_CustomerId", newName: "IX_CustomerId");
            RenameColumn(table: "dbo.Transactions", name: "Customer_CustomerId", newName: "CustomerId");
        }
    }
}
