namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Pin = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.User, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "User" });
            DropTable("dbo.Customers");
        }
    }
}
