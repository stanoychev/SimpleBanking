namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customers", new[] { "User" });
            AlterColumn("dbo.Customers", "User", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 40));
            CreateIndex("dbo.Customers", "User", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "User" });
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Customers", "User", c => c.String(nullable: false, maxLength: 254));
            CreateIndex("dbo.Customers", "User", unique: true);
        }
    }
}
