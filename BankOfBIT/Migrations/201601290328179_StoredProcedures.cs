namespace BankOfBIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProcedures : DbMigration
    {
        public override void Up()
        {
            //call script to re-create the stored procedure 
            this.Sql(Properties.Resources.Create_next_key);
        }

        public override void Down()
        {
            //call script to drop the stored procedure 
            this.Sql(Properties.Resources.Drop_next_key);
        }
    }
}
