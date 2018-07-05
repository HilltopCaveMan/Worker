using System.Data.Entity;

namespace Monopy.PreceRateWage.Model
{
    public class HHContext : DbContext
    {
        public HHContext()
            : base("name=HHContext")
        {
            //Database.CreateIfNotExists();
        }
        public virtual DbSet<DataBase1GZD> TDataBase1GZD { get; set; }
        public virtual DbSet<BaseGroup> TBaseGroup { get; set; }
        public virtual DbSet<BaseRole> TBaseRole { get; set; }
        public virtual DbSet<BaseUser> TBaseUser { get; set; }
        public virtual DbSet<BaseGroupRole> TBaseGroupRole { get; set; }
        public virtual DbSet<BaseGroupUser> TBaseGroupUser { get; set; }
        public virtual DbSet<BaseRoleUser> TBaseRoleUser { get; set; }
        public virtual DbSet<DataBaseMonth> TDataBaseMonth { get; set; }
        public virtual DbSet<DataBaseDay> TDataBaseDay { get; set; }
        public virtual DbSet<DataBaseMsg> TDataBaseMsg { get; set; }
        public virtual DbSet<DataBase3JB_XWRKHGP> TDataBase3JB_XWRKHGP { get; set; }
        public virtual DbSet<DataBase3JB_XWRYCQ> TDataBase3JB_XWRYCQ { get; set; }
        public virtual DbSet<DataBase3JB_JJRLR> TDataBase3JB_JJRLR { get; set; }
        public virtual DbSet<DataBase3JB_MCLBJJ> TDataBase3JB_MCLBJJ { get; set; }
        public virtual DbSet<DataBase3JB_MCLBJJ_YZ1> TDataBase3JB_MCLBJJ_YZ1 { get; set; }
        public virtual DbSet<DataBase3JB_MCLBJJ_YZ2> TDataBase3JB_MCLBJJ_YZ2 { get; set; }
        public virtual DbSet<DataBase3JB_MCLBJJ_YZ3> TDataBase3JB_MCLBJJ_YZ3 { get; set; }
        public virtual DbSet<DataBase3JB_MCLBJJ_YZ4> TDataBase3JB_MCLBJJ_YZ4 { get; set; }
        public virtual DbSet<DataBase3JB_MCLBJJ_YZ5> TDataBase3JB_MCLBJJ_YZ5 { get; set; }
        public virtual DbSet<DataBase3JB_CJJJ> TDataBase3JB_CJJJ { get; set; }
        public virtual DbSet<DataBase3JB_XSMCLBJJ> TDataBase3JB_XSMCLBJJ { get; set; }
        public virtual DbSet<DataBase3JB_XCSJ_WX> TDataBase3JB_XCSJ_WX { get; set; }
        public virtual DbSet<DataBase3JB_FZYH> TDataBase3JB_FZYH { get; set; }
        public virtual DbSet<DataBaseGeneral_JT> TDataBaseGeneral_JT { get; set; }
        public virtual DbSet<DataBaseGeneral_ZF> TDataBaseGeneral_ZF { get; set; }
        public virtual DbSet<DataBaseGeneral_YEY> TDataBaseGeneral_YEY { get; set; }
        public virtual DbSet<DataBaseGeneral_JC_Dept> TDataBaseGeneral_JC_Dept { get; set; }
        public virtual DbSet<BaseHeadcount> TBaseHeadcount { get; set; }
        public virtual DbSet<DataBaseGeneral_BX> TDataBaseGeneral_BX { get; set; }
        public virtual DbSet<DataBaseGeneral_WX> TDataBaseGeneral_WX { get; set; }
        public virtual DbSet<DataBaseGeneral_HZJJ_NoMoney> TDataBaseGeneral_HZJJ_NoMoney { get; set; }
        public virtual DbSet<DataBaseGeneral_JC_Factory> TDataBaseGeneral_JC_Factory { get; set; }
        public virtual DbSet<DataBaseGeneral_LZ> TDataBaseGeneral_LZ { get; set; }
        public virtual DbSet<DataBaseGeneral_JSF> TDataBaseGeneral_JSF { get; set; }
        public virtual DbSet<DataBaseGeneral_CP> TDataBaseGeneral_CP { get; set; }
        public virtual DbSet<DataBaseGeneral_TJF> TDataBaseGeneral_TJF { get; set; }
        public virtual DbSet<DataBaseGeneral_GZZRX> TDataBaseGeneral_GZZRX { get; set; }
        public virtual DbSet<DataBaseGeneral_GL> TDataBaseGeneral_GL { get; set; }
        public virtual DbSet<DataBaseGeneral_CQ> TDataBaseGeneral_CQ { get; set; }
        public virtual DbSet<DataBaseGeneral_BF> TDataBaseGeneral_BF { get; set; }
        public virtual DbSet<GZD> TGZD { get; set; }
        public virtual DbSet<DataBase3JB_XZF> TDataBase3JB_XZF { get; set; }
        public virtual DbSet<DataBaseGeneral_HZJJ_High> TDataBaseGeneral_HZJJ_High { get; set; }
        public virtual DbSet<DataBase3JB_PGYHZTS> TDataBase3JB_PGYHZTS { get; set; }
        public virtual DbSet<LogTable> TLogTable { get; set; }
        public virtual DbSet<DataBaseGeneral_XTDay> TDataBaseGeneral_XTDay { get; set; }
        public virtual DbSet<DataBaseGeneral_XT> TDataBaseGeneral_XT { get; set; }
        public virtual DbSet<DataBase1CC_XTTZ> TDataBase1CC_XTTZ { get; set; }
        /*=========================================磨具开始=========================================*/

        public virtual DbSet<DataBase3MJ_CJKH> TDataBase3MJ_CJKH { get; set; }
        public virtual DbSet<DataBase3MJ_DJCJYB> TDataBase3MJ_DJCJYB { get; set; }
        public virtual DbSet<DataBase3MJ_XJCJYB> TDataBase3MJ_XJCJYB { get; set; }
        public virtual DbSet<DataBase3MJ_XSGJJ> TDataBase3MJ_XSGJJ { get; set; }
        public virtual DbSet<DataBase3MJ_PMCDJ> TDataBase3MJ_PMCDJ { get; set; }
        public virtual DbSet<DataBase3MJ_PMCXJ> TDataBase3MJ_PMCXJ { get; set; }
        public virtual DbSet<DataBase3MJ_PMCSX> TDataBase3MJ_PMCSX { get; set; }
        public virtual DbSet<DataBaseGeneral_FZYH> TDataBaseGeneral_FZYH { get; set; }
        public virtual DbSet<DataBaseGeneral_RZBZ> TDataBaseGeneral_RZBZ { get; set; }

        /*=========================================原料开始=========================================*/
        public virtual DbSet<DataBase3YL_JYGKHB> TDataBase3YL_JYGKHB { get; set; }
        public virtual DbSet<DataBase3YL_JJTJB> TDataBase3YL_JJTJB { get; set; }
        public virtual DbSet<DataBase3YL_JJHS> TDataBase3YL_JJHS { get; set; }

        public virtual DbSet<DataBaseGeneral_JBSPB> TDataBaseGeneral_JBSPB { get; set; }

        /*=========================================喷釉开始=========================================*/
        public virtual DbSet<DataBase3PY_PYGZHS> TDataBase3PY_PYGZHS { get; set; }
        public virtual DbSet<DataBase3PY_JXG_KH> TDataBase3PY_JXG_KH { get; set; }
        public virtual DbSet<DataBase3PY_CLG_KH> TDataBase3PY_CLG_KH { get; set; }
        public virtual DbSet<DataBase3PY_BZ_KH> TDataBase3PY_BZ_KH { get; set; }
        public virtual DbSet<DataBase3PY_BZ_KH_Sum> TDataBase3PY_BZ_KH_Sum { get; set; }
        public virtual DbSet<DataBase3PY_BG> TDataBase3PY_BG { get; set; }
        public virtual DbSet<DataBase3PY_PS> TDataBase3PY_PS { get; set; }
        public virtual DbSet<DataBaseGeneral_PMCPS> TDataBaseGeneral_PMCPS { get; set; }
        public virtual DbSet<DataBaseGeneral_KFSS> TDataBaseGeneral_KFSS { get; set; }
        public virtual DbSet<DataBase3PY_BG_YZ> TDataBase3PY_BG_YZ { get; set; }
        /*=========================================烧成开始=========================================*/
        public virtual DbSet<DataBase3SC_01_ZYJJHKH> TDataBase3SC_01_ZYJJHKH { get; set; }
        public virtual DbSet<DataBase3SC_0203_ZY_HS> TDataBase3SC_0203_ZY_HS { get; set; }
        public virtual DbSet<DataBase3SC_0203_ZY_HS_KYGKH> TDataBase3SC_0203_ZY_HS_KYGKH { get; set; }
        public virtual DbSet<DataBase3SC_0203_ZY_HS_SXYKH> TDataBase3SC_0203_ZY_HS_SXYKH { get; set; }
        public virtual DbSet<DataBase3SC_0203_ZY_HS_XCGKH> TDataBase3SC_0203_ZY_HS_XCGKH { get; set; }
        public virtual DbSet<DataBase3SC_0203_ZY_HS_ZYGXKH> TDataBase3SC_0203_ZY_HS_ZYGXKH { get; set; }
        public virtual DbSet<DataBase3SC_04_3DZY> TDataBase3SC_04_3DZY { get; set; }
        public virtual DbSet<DataBase3SC_05_KYBG> TDataBase3SC_05_KYBG { get; set; }
        public virtual DbSet<DataBase3SC_06_HSJYADL> TDataBase3SC_06_HSJYADL { get; set; }
        public virtual DbSet<DataBase3SC_06_HSJYADL_SCBCGKH> TDataBase3SC_06_HSJYADL_SCBCGKH { get; set; }
        public virtual DbSet<DataBase3SC_07_QZAQY> TDataBase3SC_07_QZAQY { get; set; }
        public virtual DbSet<DataBase3SC_08_TBSJQXS> TDataBase3SC_08_TBSJQXS { get; set; }
        public virtual DbSet<DataBase3SC_09_TBJJJL> TDataBase3SC_09_TBJJJL { get; set; }
        public virtual DbSet<DataBase3SC_10_ZYJJADL> TDataBase3SC_10_ZYJJADL { get; set; }
        public virtual DbSet<DataBase3SC_10_ZYJJADL_XXTBKH> TDataBase3SC_10_ZYJJADL_XXTBKH { get; set; }
        public virtual DbSet<DataBase3SC_11_JSYKH> TDataBase3SC_11_JSYKH { get; set; }
        public virtual DbSet<DataBase3SC_12_JSYBG> TDataBase3SC_12_JSYBG { get; set; }
        public virtual DbSet<DataBaseGeneral_RZBZ_Month> TDataBaseGeneral_RZBZ_Month { get; set; }
        public virtual DbSet<DataBase3SC_13_BG> TDataBase3SC_13_BG { get; set; }
        public virtual DbSet<DataBase3SC_13_BG_DY> TDataBase3SC_13_BG_DY { get; set; }
        public virtual DbSet<DataBase3SC_13_BG_GZYZ> TDataBase3SC_13_BG_GZYZ { get; set; }
        /*=========================================成型（高压面具）开始=========================================*/
        public virtual DbSet<DataBase3CX_GYMJ_01_PMCDDGYJJJP> TDataBase3CX_GYMJ_01_PMCDDGYJJJP { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_02_PMCBJYB> TDataBase3CX_GYMJ_02_PMCBJYB { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL> TDataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_05_CJBG> TDataBase3CX_GYMJ_05_CJBG { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_06_JPB> TDataBase3CX_GYMJ_06_JPB { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_07_BZFGY> TDataBase3CX_GYMJ_07_BZFGY { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_081_ZLWCHZB> TDataBase3CX_GYMJ_081_ZLWCHZB { get; set; }
        public virtual DbSet<DataBase3CX_GYMJ_082_BBKH> TDataBase3CX_GYMJ_082_BBKH { get; set; }
        public virtual DbSet<DataBase3CX_General_CXYB> TDataBase3CX_General_CXYB { get; set; }
        public virtual DbSet<DataBase3CX_General_JB> TDataBase3CX_General_JB { get; set; }

        /// <summary>
        /// 人员工号对应关系
        /// </summary>
        public virtual DbSet<DataBase3CX_General_RYDY> TDataBase3CX_General_RYDY { get; set; }

        /*=========================================成型（半检拉坯）开始=========================================*/
        public virtual DbSet<DataBase3CX_BJLP_01_LPYB> TDataBase3CX_BJLP_01_LPYB { get; set; }

        /*=========================================成型（高压水箱）开始=========================================*/
        public virtual DbSet<DataBase3CX_GYSX_01_BZF> TDataBase3CX_GYSX_01_BZF { get; set; }
        public virtual DbSet<DataBase3CX_GYSX_02_GYSX> TDataBase3CX_GYSX_02_GYSX { get; set; }

        /*=========================================成型（技术员）开始=========================================*/
        public virtual DbSet<DataBase3CX_JSY_01CXJSYKH_01CSZL> TDataBase3CX_JSY_01CXJSYKH_01CSZL { get; set; }
        public virtual DbSet<DataBase3CX_JSY_01CXJSYKH_02KH> TDataBase3CX_JSY_01CXJSYKH_02KH { get; set; }
        public virtual DbSet<DataBase3CX_JSY_02MJJSYKH> TDataBase3CX_JSY_02MJJSYKH { get; set; }
        /*=========================================成型开始=========================================*/
        public virtual DbSet<DataBase3CX_CX_01JJKHTB> TDataBase3CX_CX_01JJKHTB { get; set; }
        public virtual DbSet<DataBase3CX_CX_01JJKHTB_Out> TDataBase3CX_CX_01JJKHTB_Out { get; set; }
        public virtual DbSet<DataBase3CX_CX_02MXS> TDataBase3CX_CX_02MXS { get; set; }
        public virtual DbSet<DataBase3CX_CX_03JJ> TDataBase3CX_CX_03JJ { get; set; }
        public virtual DbSet<DataBase3CX_CX_03JJ_7T> TDataBase3CX_CX_03JJ_7T { get; set; }
        public virtual DbSet<DataBase3CX_CX_03JJ_GR> TDataBase3CX_CX_03JJ_GR { get; set; }
        public virtual DbSet<DataBase3CX_CX_04SXBZ> TDataBase3CX_CX_04SXBZ { get; set; }
        public virtual DbSet<DataBase3CX_CX_04SX_TZ> TDataBase3CX_CX_04SX_TZ { get; set; }
        public virtual DbSet<DataBase3CX_CX_05PX_TZ> TDataBase3CX_CX_05PX_TZ { get; set; }
        public virtual DbSet<DataBase3CX_CX_05PXBZ> TDataBase3CX_CX_05PXBZ { get; set; }
        public virtual DbSet<DataBase3CX_CX_06ZXJ> TDataBase3CX_CX_06ZXJ { get; set; }
        public virtual DbSet<DataBase3CX_CX_07FLFK> TDataBase3CX_CX_07FLFK { get; set; }
        public virtual DbSet<DataBase3CX_CX_09SS> TDataBase3CX_CX_09SS { get; set; }
        public virtual DbSet<DataBase3CX_CX_11PSBZ> TDataBase3CX_CX_11PSBZ { get; set; }
        public virtual DbSet<DataBase3CX_CX_12SFBZ> TDataBase3CX_CX_12SFBZ { get; set; }


        /*=========================================一厂仓储开始=========================================*/
        public virtual DbSet<DataBase1CC_CJB> TDataBase1CC_CJB { get; set; }
        public virtual DbSet<DataBase1CC_PGYH> TDataBase1CC_PGYH { get; set; }
        /*=========================================一厂仓储结束=========================================*/


        /*=========================================一厂原料开始=========================================*/
        public virtual DbSet<DataBase1YL_YLTJ> TDataBase1YL_YLTJ { get; set; }
        public virtual DbSet<DataBase1YL_JJ> TDataBase1YL_JJ { get; set; }

        /*=========================================一厂原料结束=========================================*/

        /*=========================================一厂模具开始=========================================*/
        public virtual DbSet<DataBase1MJ_XSGJJ> TDataBase1MJ_XSGJJ { get; set; }
        public virtual DbSet<DataBase1MJ_RGTB> TDataBase1MJ_RGTB { get; set; }
        public virtual DbSet<DataBase1MJ_DJCJYB> TDataBase1MJ_DJCJYB { get; set; }
        public virtual DbSet<DataBase1MJ_PMCDJ> TDataBase1MJ_PMCDJ { get; set; }
        public virtual DbSet<DataBase1MJ_PMCXJ> TDataBase1MJ_PMCXJ { get; set; }
        public virtual DbSet<DataBase1MJ_XJCJYB> TDataBase1MJ_XJCJYB { get; set; }
        public virtual DbSet<DataBase1MJ_YMJJ> TDataBase1MJ_YMJJ { get; set; }

        /*=========================================一厂模具结束=========================================*/

        /*=========================================一厂成型开始=========================================*/
        public virtual DbSet<DataBase1CX_BJLP_01_LPYB> TDataBase1CX_BJLP_01_LPYB { get; set; }
        public virtual DbSet<DataBase1CX_BJLP_01_JJ> TDataBase1CX_BJLP_01_JJ { get; set; }
        public virtual DbSet<DataBase1CX_DNG_01CXDNGKH_01DCL> TDataBase1CX_DNG_01CXDNGKH_01DCL { get; set; }
        public virtual DbSet<DataBase1CX_DNG_01CXDNGKH_02KH> TDataBase1CX_DNG_01CXDNGKH_02KH { get; set; }
        public virtual DbSet<DataBase1CX_CX_06JJ> TDataBase1CX_CX_03JJ { get; set; }
        public virtual DbSet<DataBase1CX_CX_03JJ_7T> TDataBase1CX_CX_03JJ_7T { get; set; }
        public virtual DbSet<DataBase1CX_CX_04BKH> TDataBase1CX_CX_04BKH { get; set; }
        public virtual DbSet<DataBase1CX_CX_07DLDT> TDataBase1CX_CX_07DLDT { get; set; }
        public virtual DbSet<DataBase1CX_CX_06JJ_GR> TDataBase1CX_CX_06JJ_GR { get; set; }
        public virtual DbSet<DataBase1CX_CX_02JJKHTB> TDataBase1CX_CX_02JJKHTB { get; set; }
        public virtual DbSet<DataBase1CX_CX_02JJKHTB_Out> TDataBase1CX_CX_02JJKHTB_Out { get; set; }

        public virtual DbSet<DataBase1CX_KFSS> TDataBase1CX_KFSS { get; set; }
        public virtual DbSet<DataBase1CX_PMCJP> TDataBase1CX_PMCJP { get; set; }
        public virtual DbSet<DataBase1CX_XJBG> TDataBase1CX_XJBG { get; set; }
        public virtual DbSet<DataBase1CX_GHDR> TDataBase1CX_GHDR { get; set; }
        public virtual DbSet<DataBase1CX_FLJF> TDataBase1CX_FLJF { get; set; }
        public virtual DbSet<DataBase1CX_QTJJ> TDataBase1CX_QTJJ { get; set; }
        public virtual DbSet<DataBase1CX_CXJJ> TDataBase1CX_CXJJ { get; set; }
        public virtual DbSet<DataBase1CX_BCTZ> TDataBase1CX_BCTZ { get; set; }
        public virtual DbSet<DataBase1CX_BCBZ> TDataBase1CX_BCBZ { get; set; }
        public virtual DbSet<DataBase1CX_General_CXYB> TDataBase1CX_General_CXYB { get; set; }


        /*=========================================一厂成型结束=========================================*/

        /*=========================================一厂检包开始=========================================*/
        public virtual DbSet<DataBase1JB_XWRKHGP> TDataBase1JB_XWRKHGP { get; set; }
        public virtual DbSet<DataBase1JB_XWRYCQ> TDataBase1JB_XWRYCQ { get; set; }
        public virtual DbSet<DataBase1JB_XNDY> TDataBase1JB_XNDY { get; set; }
        public virtual DbSet<DataBase1JB_MCLBJJ> TDataBase1JB_MCLBJJ { get; set; }
        public virtual DbSet<DataBase1JB_KFSS> TDataBase1JB_KFSS { get; set; }
        public virtual DbSet<DataBase1JB_PMCMC> TDataBase1JB_PMCMC { get; set; }
        public virtual DbSet<DataBase1JB_PMCMCYKC> TDataBase1JB_PMCMCYKC { get; set; }
        public virtual DbSet<DataBase1JB_PMCSY> TDataBase1JB_PMCSY { get; set; }
        public virtual DbSet<DataBase1JB_CJJJ> TDataBase1JB_CJJJ { get; set; }
        public virtual DbSet<DataBase1JB_JJRLR> TDataBase1JB_JJRLR { get; set; }

        /*=========================================一厂检包结束=========================================*/

        /*=========================================一厂烧成开始=========================================*/
        public virtual DbSet<DataBase1SC_CJB> TDataBase1SC_CJB { get; set; }
        public virtual DbSet<DataBase1SC_ZY> TDataBase1SC_ZY { get; set; }
        public virtual DbSet<DataBase1SC_ZYJJHKH> TDataBase1SC_ZYJJHKH { get; set; }
        public virtual DbSet<DataBase1SC_QTJJ> TDataBase1SC_QTJJ { get; set; }
        public virtual DbSet<DataBase1SC_SY> TDataBase1SC_SY { get; set; }
        public virtual DbSet<DataBase1SC_JSBZ> TDataBase1SC_JSBZ { get; set; }
        public virtual DbSet<DataBase1SC_HSY> TDataBase1SC_HSY { get; set; }
        public virtual DbSet<DataBase1SC_HSY_BCKH> TDataBase1SC_HSY_BCKH { get; set; }

        /*=========================================一厂烧成结束=========================================*/

        /*=========================================一厂修检开始=========================================*/
        public virtual DbSet<DataBase1XJ_XJGJJ> TDataBase1XJ_XJGJJ { get; set; }
        public virtual DbSet<DataBase1XJ_XJDYBG> TDataBase1XJ_XJDYBG { get; set; }
        public virtual DbSet<DataBase1XJ_XCKH> TDataBase1XJ_XCKH { get; set; }

        /*=========================================一厂修检结束=========================================*/

        /*=========================================一厂喷釉开始=========================================*/
        public virtual DbSet<DataBase1PY_PYPJ> TDataBase1PY_PYPJ { get; set; }
        public virtual DbSet<DataBase1PY_RJDR> TDataBase1PY_RJDR { get; set; }
        public virtual DbSet<DataBase1PY_LPJJ> TDataBase1PY_LPJJ { get; set; }
        public virtual DbSet<DataBase1PY_CJB> TDataBase1PY_CJB { get; set; }
        public virtual DbSet<DataBase1PY_CJB_YGZ> TDataBase1PY_CJB_YGZ { get; set; }

        /*=========================================一厂喷釉结束=========================================*/

        /*=========================================二厂仓储开始=========================================*/
        public virtual DbSet<DataBase2CC_CJB> TDataBase2CC_CJB { get; set; }

        /*=========================================二厂仓储结束=========================================*/

        /*=========================================二厂喷釉开始=========================================*/
        public virtual DbSet<DataBase2PY_CJB> TDataBase2PY_CJB { get; set; }
        public virtual DbSet<DataBase2PY_CJB_YGZ> TDataBase2PY_CJB_YGZ { get; set; }
        public virtual DbSet<DataBase2PY_RJDR> TDataBase2PY_RJDR { get; set; }
        public virtual DbSet<DataBase2PY_JSBZ> TDataBase2PY_JSBZ { get; set; }

        /*=========================================二厂喷釉结束=========================================*/

        /*=========================================二厂烧成开始=========================================*/
        public virtual DbSet<DataBase2SC_ZYJJHKH> TDataBase2SC_ZYJJHKH { get; set; }
        public virtual DbSet<DataBase2SC_CJB> TDataBase2SC_CJB { get; set; }

        /*=========================================二厂烧成结束=========================================*/

        /*=========================================二厂原料开始=========================================*/
        public virtual DbSet<DataBase2YL_XCYLTJ> TDataBase2YL_XCYLTJ { get; set; }
        public virtual DbSet<DataBase2YL_XCJJ> TDataBase2YL_XCJJ { get; set; }
        public virtual DbSet<DataBase2YL_NYLYLJJ_02NJ> TDataBase1YL_NYLYLJJ_02NJ { get; set; }
        public virtual DbSet<DataBase2YL_NYLYLJJ_01YJ> TDataBase1YL_NYLYLJJ_01YJ { get; set; }
        public virtual DbSet<DataBase2YL_JJTB> TDataBase2YL_JJTB { get; set; }
        public virtual DbSet<DataBase2YL_GRJJ> TDataBase2YL_GRJJ { get; set; }

        /*=========================================二厂原料结束=========================================*/

        /*=========================================二厂模具开始=========================================*/
        public virtual DbSet<DataBase2MJ_YMJJ> TDataBase2MJ_YMJJ { get; set; }
        public virtual DbSet<DataBase2MJ_PMCXJ> TDataBase2MJ_PMCXJ { get; set; }
        public virtual DbSet<DataBase2MJ_PMCDJ> TDataBase2MJ_PMCDJ { get; set; }
        public virtual DbSet<DataBase2MJ_DJCJYB> TDataBase2MJ_DJCJYB { get; set; }
        public virtual DbSet<DataBase2MJ_XJCJYB> TDataBase2MJ_XJCJYB { get; set; }
        public virtual DbSet<DataBase2MJ_XSGJJ> TDataBase2MJ_XSGJJ { get; set; }
        public virtual DbSet<DataBase2MJ_QTJJ> TDataBase2MJ_QTJJ { get; set; }
        public virtual DbSet<DataBase2MJ_SCXTTZ> TDataBase2MJ_SCXTTZ { get; set; }
        public virtual DbSet<DataBase2MJ_SCXTDay> TDataBase2MJ_SCXTDay { get; set; }

        /*=========================================二厂模具结束=========================================*/

        /*=========================================二厂成型（技术员）开始=========================================*/
        public virtual DbSet<DataBase2CX_JSY_01CXJSYKH_02KH> TDataBase2CX_JSY_01CXJSYKH_02KH { get; set; }
        public virtual DbSet<DataBase2CX_JSY_01CXJSYKH_01CSZL> TDataBase2CX_JSY_01CXJSYKH_01CSZL { get; set; }

        /*=========================================二厂成型（技术员）结束=========================================*/

        /*=========================================二厂半检拉坯开始=========================================*/
        public virtual DbSet<DataBase2CX_BJLP_BJYB> TDataBase2CX_BJLP_BJYB { get; set; }
        public virtual DbSet<DataBase2CX_BJLP_BJJJ> TDataBase2CX_BJLP_BJJJ { get; set; }
        public virtual DbSet<DataBase2CX_BJLP_CJTB> TDataBase2CX_BJLP_CJTB { get; set; }

        /*=========================================二厂半检拉坯结束=========================================*/

        /*=========================================二厂成型开始=========================================*/
        public virtual DbSet<DataBase2CX_PMCJP> TDataBase2CX_PMCJP { get; set; }
        public virtual DbSet<DataBase2CX_XJBG> TDataBase2CX_XJBG { get; set; }
        public virtual DbSet<DataBase2CX_GHDR> TDataBase2CX_GHDR { get; set; }
        public virtual DbSet<DataBase2CX_FLJF> TDataBase2CX_FLJF { get; set; }
        
    }
}