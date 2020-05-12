namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customers", new[] { "User" });
            AlterColumn("dbo.Customers", "User", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.Customers", "User", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "User" });
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Customers", "User", c => c.String(nullable: false, maxLength: 1024));
            CreateIndex("dbo.Customers", "User", unique: true);
        }
    }
}
