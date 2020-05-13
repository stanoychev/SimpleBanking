namespace SimpleBanking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Date", c => c.DateTime());
            DropColumn("dbo.Transactions", "TimeOfExecution");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "TimeOfExecution", c => c.DateTime());
            DropColumn("dbo.Transactions", "Date");
        }
    }
}
