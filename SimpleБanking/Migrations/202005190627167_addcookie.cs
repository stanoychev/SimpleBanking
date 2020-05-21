namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcookie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Cookie", c => c.String());
            AddColumn("dbo.Customers", "ExpiresOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "ExpiresOn");
            DropColumn("dbo.Customers", "Cookie");
        }
    }
}
