namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class buildBankDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User = c.String(nullable: false, maxLength: 100),
                        Pin = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.User, unique: true);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(),
                        Receiver_Id = c.Int(),
                        Sender_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Receiver_Id)
                .ForeignKey("dbo.Customers", t => t.Sender_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "Sender_Id", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "Receiver_Id", "dbo.Customers");
            DropIndex("dbo.Transactions", new[] { "Customer_Id" });
            DropIndex("dbo.Transactions", new[] { "Sender_Id" });
            DropIndex("dbo.Transactions", new[] { "Receiver_Id" });
            DropIndex("dbo.Customers", new[] { "User" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Customers");
        }
    }
}
