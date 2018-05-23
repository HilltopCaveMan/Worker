namespace Monopy.PreceRateWage.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class month : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataBaseMonths", "CreateYear", c => c.Int(nullable: false));
            AddColumn("dbo.DataBaseMonths", "CreateMonth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataBaseMonths", "CreateMonth");
            DropColumn("dbo.DataBaseMonths", "CreateYear");
        }
    }
}
