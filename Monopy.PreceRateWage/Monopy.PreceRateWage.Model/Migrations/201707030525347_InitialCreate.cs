namespace Monopy.PreceRateWage.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseGroupInfoes",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    GroupID = c.String(),
                    GroupName = c.String(),
                    IsChecked = c.Boolean(nullable: false),
                    ParentID = c.String(),
                    Tooltip = c.String(),
                    GroupClass = c.String(),
                    IsBeginGroup = c.Boolean(nullable: false),
                    Symbol = c.String(),
                    Showdialog = c.Boolean(nullable: false),
                    CloseButtonVisible = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.DataBase3JB_JJRLR",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    TheYear = c.Int(nullable: false),
                    TheMonth = c.Int(nullable: false),
                    TheDay = c.Int(nullable: false),
                    Line = c.String(),
                    GW = c.String(),
                    UserCode = c.String(),
                    UserName = c.String(),
                    F_1 = c.String(),
                    F_2 = c.String(),
                    F_3 = c.String(),
                    F_B_1 = c.String(),
                    F_B_2 = c.String(),
                    F_B_3 = c.String(),
                    F_B_4 = c.String(),
                    F_B_5 = c.String(),
                    PMC_1 = c.String(),
                    PMC_2 = c.String(),
                    PMC_3 = c.String(),
                    PMC_4 = c.String(),
                    PMC_5 = c.String(),
                    PMC_6 = c.String(),
                    PMC_7 = c.String(),
                    PMC_8 = c.String(),
                    PMC_9 = c.String(),
                    PMC_10 = c.String(),
                    PMC_11 = c.String(),
                    PMC_12 = c.String(),
                    PMC_13 = c.String(),
                    PMC_14 = c.String(),
                    PMC_15 = c.String(),
                    PMC_16 = c.String(),
                    PMC_17 = c.String(),
                    PMC_18 = c.String(),
                    PMC_19 = c.String(),
                    PMC_20 = c.String(),
                    PMC_21 = c.String(),
                    PMC_22 = c.String(),
                    PMC_B_1 = c.String(),
                    PMC_B_2 = c.String(),
                    PMC_B_3 = c.String(),
                    PMC_B_4 = c.String(),
                    PMC_B_5 = c.String(),
                    PG_1 = c.String(),
                    PG_2 = c.String(),
                    PG_3 = c.String(),
                    PG_4 = c.String(),
                    PG_5 = c.String(),
                    PG_6 = c.String(),
                    PG_7 = c.String(),
                    PG_8 = c.String(),
                    PG_B_1 = c.String(),
                    PG_B_2 = c.String(),
                    PG_B_3 = c.String(),
                    PG_B_4 = c.String(),
                    PG_B_5 = c.String(),
                    KF_1 = c.String(),
                    KF_B_1 = c.String(),
                    KF_B_2 = c.String(),
                    KF_B_3 = c.String(),
                    KF_B_4 = c.String(),
                    KF_B_5 = c.String(),
                    WX_PMCDD_1 = c.String(),
                    WX_1 = c.String(),
                    WX_B_1 = c.String(),
                    WX_B_2 = c.String(),
                    WX_B_3 = c.String(),
                    WX_B_4 = c.String(),
                    WX_B_5 = c.String(),
                    JE = c.String(),
                    CreateTime = c.DateTime(nullable: false),
                    CreateUser = c.String(nullable: false),
                    ModifyTime = c.DateTime(),
                    ModifyUser = c.String(),
                    PMC_Time = c.DateTime(),
                    PMC_User = c.String(),
                    PMC_Manager_Time = c.DateTime(),
                    PMC_Manager_User = c.String(),
                    PG_Time = c.DateTime(),
                    PG_User = c.String(),
                    PG_Manager_Time = c.DateTime(),
                    PG_Manager_User = c.String(),
                    KF_Time = c.DateTime(),
                    KF_User = c.String(),
                    KF_Manager_Time = c.DateTime(),
                    KF_Manager_User = c.String(),
                    WX_Time = c.DateTime(),
                    WX_User = c.String(),
                    WX_Manager_Time = c.DateTime(),
                    WX_Manager_User = c.String(),
                    PMCDD_Time = c.DateTime(),
                    PMCDD_User = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.DataBase3JB_XWRKHGP",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    CreateTime = c.DateTime(nullable: false),
                    CreateUser = c.String(nullable: false),
                    TheYear = c.Int(nullable: false),
                    TheMonth = c.Int(nullable: false),
                    TheDay = c.Int(nullable: false),
                    TypesName = c.String(nullable: false),
                    Unit = c.String(),
                    X1 = c.String(),
                    X2 = c.String(),
                    X3 = c.String(),
                    X4 = c.String(),
                    X5 = c.String(),
                    X6 = c.String(),
                    X7 = c.String(),
                    X8 = c.String(),
                    X9 = c.String(),
                    X10 = c.String(),
                    UnitPrice = c.String(),
                    L1 = c.String(),
                    L2 = c.String(),
                    L3 = c.String(),
                    L4 = c.String(),
                    L5 = c.String(),
                    L6 = c.String(),
                    L7 = c.String(),
                    L8 = c.String(),
                    L9 = c.String(),
                    L10 = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.DataBase3JB_XWRYCQ",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    CreateTime = c.DateTime(nullable: false),
                    CreateUser = c.String(nullable: false),
                    TheYear = c.Int(nullable: false),
                    TheMonth = c.Int(nullable: false),
                    TheDay = c.Int(nullable: false),
                    XWType = c.String(),
                    XW = c.String(),
                    GWMC = c.String(),
                    UserCode = c.String(),
                    UserName = c.String(),
                    StudyDay = c.String(),
                    WorkDay = c.String(),
                    GZZB = c.String(),
                    TotalGZ = c.String(),
                    GWZCQ = c.String(),
                    DGWGZ = c.String(),
                    XTRG = c.String(),
                    RZBZGZ = c.String(),
                    JBGZ = c.String(),
                    TBGZE = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.DataBaseDays",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    
                    FactoryNo = c.String(nullable: false, maxLength: 10),
                    WorkshopName = c.String(nullable: false, maxLength: 50),
                    PostName = c.String(maxLength: 50),
                    Classification = c.String(maxLength: 50),
                    JBXW = c.String(maxLength: 50),
                    TypesName = c.String(maxLength: 50),
                    TypesType = c.String(maxLength: 50),
                    TypesUnit = c.String(maxLength: 50),
                    UnitPrice = c.String(),
                    ZB_JB_JJGZ = c.String(),
                    ZB_PY_BZKH = c.String(),
                    BZKHZB = c.String(),
                    BZJSB = c.String(),
                    BZJLDJ = c.String(),
                    GRKHZB1 = c.String(),
                    GRJSB1 = c.String(),
                    GRJLDJ1 = c.String(),
                    GRFKDJ1 = c.String(),
                    GRKHZB2 = c.String(),
                    GRJSB2 = c.String(),
                    GRJLDJ2 = c.String(),
                    GRFKDJ2 = c.String(),
                    GRKHZB3 = c.String(),
                    GRJSB3 = c.String(),
                    GRJLDJ3 = c.String(),
                    GRFKDJ3 = c.String(),
                    GRKHZB4 = c.String(),
                    GRJSB4 = c.String(),
                    GRJLDJ4 = c.String(),
                    GRFKDJ4 = c.String(),
                    KHJESFD = c.String(),
                    KHJEXFD = c.String(),
                    DXKYL = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.DataBaseMonths",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    FactoryNo = c.String(nullable: false, maxLength: 10),
                    WorkshopName = c.String(nullable: false, maxLength: 50),
                    PostName = c.String(maxLength: 50),
                    Classification = c.String(maxLength: 50),
                    Gender = c.String(maxLength: 50),
                    MonthData = c.String(maxLength: 50),
                    ProductType = c.String(maxLength: 50),
                    Types = c.String(maxLength: 50),
                    TypesType = c.String(maxLength: 50),
                    SFbackTD = c.String(),
                    DayWork_FZYH = c.String(),
                    DayWork_LSG = c.String(),
                    DayWork_XT = c.String(),
                    DayWork_TXJ = c.String(),
                    MoneyShift = c.String(),
                    ZB_XT_JB = c.String(),
                    ZB_XT_JN = c.String(),
                    MoneyBase = c.String(),
                    MoneyJN = c.String(),
                    MoneyJB = c.String(),
                    MoneyQQJ = c.String(),
                    MoneyKH = c.String(),
                    MoneyRZBZ = c.String(),
                    MoneyBCBZ = c.String(),
                })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.DataBaseMonths");
            DropTable("dbo.DataBaseDays");
            DropTable("dbo.DataBase3JB_XWRYCQ");
            DropTable("dbo.DataBase3JB_XWRKHGP");
            DropTable("dbo.DataBase3JB_JJRLR");
            DropTable("dbo.BaseGroupInfoes");
        }
    }
}