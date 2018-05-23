namespace Monopy.PreceRateWage.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataBaseDays", "CreateYear", c => c.Int(nullable: false));
            AlterColumn("dbo.DataBaseDays", "CreateMonth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataBaseDays", "CreateMonth", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.DataBaseDays", "CreateYear", c => c.String(nullable: false, maxLength: 4));
        }
    }
}
