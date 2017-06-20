namespace BankOfBIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixSpelling2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NextSavingsAccounts", "NextAvailableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextMortgageAccounts", "NextAvailableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextInvestmentAccounts", "NextAvailableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextChequingAccounts", "NextAvailableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextTransactionNumbers", "NextAvailableNumber", c => c.Int(nullable: false));
            DropColumn("dbo.NextSavingsAccounts", "NextAvaiableNumber");
            DropColumn("dbo.NextMortgageAccounts", "NextAvaiableNumber");
            DropColumn("dbo.NextInvestmentAccounts", "NextAvaiableNumber");
            DropColumn("dbo.NextChequingAccounts", "NextAvaiableNumber");
            DropColumn("dbo.NextTransactionNumbers", "NextAvaiableNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NextTransactionNumbers", "NextAvaiableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextChequingAccounts", "NextAvaiableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextInvestmentAccounts", "NextAvaiableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextMortgageAccounts", "NextAvaiableNumber", c => c.Int(nullable: false));
            AddColumn("dbo.NextSavingsAccounts", "NextAvaiableNumber", c => c.Int(nullable: false));
            DropColumn("dbo.NextTransactionNumbers", "NextAvailableNumber");
            DropColumn("dbo.NextChequingAccounts", "NextAvailableNumber");
            DropColumn("dbo.NextInvestmentAccounts", "NextAvailableNumber");
            DropColumn("dbo.NextMortgageAccounts", "NextAvailableNumber");
            DropColumn("dbo.NextSavingsAccounts", "NextAvailableNumber");
        }
    }
}
