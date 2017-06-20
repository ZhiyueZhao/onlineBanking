namespace BankOfBIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixSpelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NextClientNumbers", "NextAvailableNumber", c => c.Int(nullable: false));
            DropColumn("dbo.NextClientNumbers", "NextAvaiableNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NextClientNumbers", "NextAvaiableNumber", c => c.Int(nullable: false));
            DropColumn("dbo.NextClientNumbers", "NextAvailableNumber");
        }
    }
}
