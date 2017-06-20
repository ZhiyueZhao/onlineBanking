namespace BankOfBIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountStatus",
                c => new
                    {
                        AccountStatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.AccountStatusId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountStatus");
        }
    }
}
