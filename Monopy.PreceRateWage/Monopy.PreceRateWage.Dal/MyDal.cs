using Dapper;
using Monopy.PreceRateWage.Common;
using Monopy.PreceRateWage.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Monopy.PreceRateWage.Dal
{
    public class MyDal
    {
        public static DataBase3JB_XWRKHGP GetTotalDataBase_JB_XWRKHGP(List<DataBase3JB_XWRKHGP> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_XWRKHGP() { TypesName = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day };
            }
            return new DataBase3JB_XWRKHGP() { TypesName = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day, Unit = string.Empty, X1 = list.Sum(t => string.IsNullOrEmpty(t.X1) ? 0 : Convert.ToDecimal(t.X1)).ToString(), X2 = list.Sum(t => string.IsNullOrEmpty(t.X2) ? 0 : Convert.ToDecimal(t.X2)).ToString(), X3 = list.Sum(t => string.IsNullOrEmpty(t.X3) ? 0 : Convert.ToDecimal(t.X3)).ToString(), X4 = list.Sum(t => string.IsNullOrEmpty(t.X4) ? 0 : Convert.ToDecimal(t.X4)).ToString(), X5 = list.Sum(t => string.IsNullOrEmpty(t.X5) ? 0 : Convert.ToDecimal(t.X5)).ToString(), X6 = list.Sum(t => string.IsNullOrEmpty(t.X6) ? 0 : Convert.ToDecimal(t.X6)).ToString(), X7 = list.Sum(t => string.IsNullOrEmpty(t.X7) ? 0 : Convert.ToDecimal(t.X7)).ToString(), X8 = list.Sum(t => string.IsNullOrEmpty(t.X8) ? 0 : Convert.ToDecimal(t.X8)).ToString(), X9 = list.Sum(t => string.IsNullOrEmpty(t.X9) ? 0 : Convert.ToDecimal(t.X9)).ToString(), X10 = list.Sum(t => string.IsNullOrEmpty(t.X10) ? 0 : Convert.ToDecimal(t.X10)).ToString(), X11 = list.Sum(t => string.IsNullOrEmpty(t.X11) ? 0 : Convert.ToDecimal(t.X11)).ToString(), UnitPrice = "实际", L1 = list.Sum(t => string.IsNullOrEmpty(t.L1) ? 0 : Convert.ToDecimal(t.L1)).ToString(), L2 = list.Sum(t => string.IsNullOrEmpty(t.L2) ? 0 : Convert.ToDecimal(t.L2)).ToString(), L3 = list.Sum(t => string.IsNullOrEmpty(t.L3) ? 0 : Convert.ToDecimal(t.L3)).ToString(), L4 = list.Sum(t => string.IsNullOrEmpty(t.L4) ? 0 : Convert.ToDecimal(t.L4)).ToString(), L5 = list.Sum(t => string.IsNullOrEmpty(t.L5) ? 0 : Convert.ToDecimal(t.L5)).ToString(), L6 = list.Sum(t => string.IsNullOrEmpty(t.L6) ? 0 : Convert.ToDecimal(t.L6)).ToString(), L7 = list.Sum(t => string.IsNullOrEmpty(t.L7) ? 0 : Convert.ToDecimal(t.L7)).ToString(), L8 = list.Sum(t => string.IsNullOrEmpty(t.L8) ? 0 : Convert.ToDecimal(t.L8)).ToString(), L9 = list.Sum(t => string.IsNullOrEmpty(t.L9) ? 0 : Convert.ToDecimal(t.L9)).ToString(), L10 = list.Sum(t => string.IsNullOrEmpty(t.L10) ? 0 : Convert.ToDecimal(t.L10)).ToString(), L11 = list.Sum(t => string.IsNullOrEmpty(t.L11) ? 0 : Convert.ToDecimal(t.L11)).ToString() };
        }

        public static DataBase3JB_XWRYCQ GetTotalDataBase3JB_XWRYCQ(List<DataBase3JB_XWRYCQ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_XWRYCQ() { XWType = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day };
            }
            return new DataBase3JB_XWRYCQ() { XWType = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day, DGWGZ = list.Sum(t => string.IsNullOrEmpty(t.DGWGZ) ? 0 : Convert.ToDecimal(t.DGWGZ)).ToString(), RZBZGZ = list.Sum(t => string.IsNullOrEmpty(t.RZBZGZ) ? 0 : Convert.ToDecimal(t.RZBZGZ)).ToString(), TBGZE = list.Sum(t => string.IsNullOrEmpty(t.TBGZE) ? 0 : Convert.ToDecimal(t.TBGZE)).ToString(), StudyDay = list.Sum(t => string.IsNullOrEmpty(t.StudyDay) ? 0M : Convert.ToDecimal(t.StudyDay)).ToString(), WorkDay = list.Sum(t => string.IsNullOrEmpty(t.WorkDay) ? 0M : Convert.ToDecimal(t.WorkDay)).ToString() };
        }

        public static DataBase3JB_JJRLR GetTotalDataBase3JB_JJRLR(List<DataBase3JB_JJRLR> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_JJRLR() { Line = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day };
            }
            return new DataBase3JB_JJRLR() { Line = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day, F_1 = list.Sum(t => string.IsNullOrEmpty(t.F_1) ? 0 : Convert.ToDecimal(t.F_1)).ToString(), F_2 = list.Sum(t => string.IsNullOrEmpty(t.F_2) ? 0 : Convert.ToDecimal(t.F_2)).ToString(), F_3 = list.Sum(t => string.IsNullOrEmpty(t.F_3) ? 0 : Convert.ToDecimal(t.F_3)).ToString(), F_B_1 = list.Sum(t => string.IsNullOrEmpty(t.F_B_1) ? 0 : Convert.ToDecimal(t.F_B_1)).ToString(), F_B_2 = list.Sum(t => string.IsNullOrEmpty(t.F_B_2) ? 0 : Convert.ToDecimal(t.F_B_2)).ToString(), F_B_3 = list.Sum(t => string.IsNullOrEmpty(t.F_B_3) ? 0 : Convert.ToDecimal(t.F_B_3)).ToString(), F_B_4 = list.Sum(t => string.IsNullOrEmpty(t.F_B_4) ? 0 : Convert.ToDecimal(t.F_B_4)).ToString(), F_B_5 = list.Sum(t => string.IsNullOrEmpty(t.F_B_5) ? 0 : Convert.ToDecimal(t.F_B_5)).ToString(), PMC_1 = list.Sum(t => string.IsNullOrEmpty(t.PMC_1) ? 0 : Convert.ToDecimal(t.PMC_1)).ToString(), PMC_2 = list.Sum(t => string.IsNullOrEmpty(t.PMC_2) ? 0 : Convert.ToDecimal(t.PMC_2)).ToString(), PMC_3 = list.Sum(t => string.IsNullOrEmpty(t.PMC_3) ? 0 : Convert.ToDecimal(t.PMC_3)).ToString(), PMC_4 = list.Sum(t => string.IsNullOrEmpty(t.PMC_4) ? 0 : Convert.ToDecimal(t.PMC_4)).ToString(), PMC_5 = list.Sum(t => string.IsNullOrEmpty(t.PMC_5) ? 0 : Convert.ToDecimal(t.PMC_5)).ToString(), PMC_6 = list.Sum(t => string.IsNullOrEmpty(t.PMC_6) ? 0 : Convert.ToDecimal(t.PMC_6)).ToString(), PMC_7 = list.Sum(t => string.IsNullOrEmpty(t.PMC_7) ? 0 : Convert.ToDecimal(t.PMC_7)).ToString(), PMC_8 = list.Sum(t => string.IsNullOrEmpty(t.PMC_8) ? 0 : Convert.ToDecimal(t.PMC_8)).ToString(), PMC_9 = list.Sum(t => string.IsNullOrEmpty(t.PMC_9) ? 0 : Convert.ToDecimal(t.PMC_9)).ToString(), PMC_10 = list.Sum(t => string.IsNullOrEmpty(t.PMC_10) ? 0 : Convert.ToDecimal(t.PMC_10)).ToString(), PMC_11 = list.Sum(t => string.IsNullOrEmpty(t.PMC_11) ? 0 : Convert.ToDecimal(t.PMC_11)).ToString(), PMC_12 = list.Sum(t => string.IsNullOrEmpty(t.PMC_12) ? 0 : Convert.ToDecimal(t.PMC_12)).ToString(), PMC_13 = list.Sum(t => string.IsNullOrEmpty(t.PMC_13) ? 0 : Convert.ToDecimal(t.PMC_13)).ToString(), PMC_14 = list.Sum(t => string.IsNullOrEmpty(t.PMC_14) ? 0 : Convert.ToDecimal(t.PMC_14)).ToString(), PMC_15 = list.Sum(t => string.IsNullOrEmpty(t.PMC_15) ? 0 : Convert.ToDecimal(t.PMC_15)).ToString(), PMC_16 = list.Sum(t => string.IsNullOrEmpty(t.PMC_16) ? 0 : Convert.ToDecimal(t.PMC_16)).ToString(), PMC_17 = list.Sum(t => string.IsNullOrEmpty(t.PMC_17) ? 0 : Convert.ToDecimal(t.PMC_17)).ToString(), PMC_18 = list.Sum(t => string.IsNullOrEmpty(t.PMC_18) ? 0 : Convert.ToDecimal(t.PMC_18)).ToString(), PMC_19 = list.Sum(t => string.IsNullOrEmpty(t.PMC_19) ? 0 : Convert.ToDecimal(t.PMC_19)).ToString(), PMC_20 = list.Sum(t => string.IsNullOrEmpty(t.PMC_20) ? 0 : Convert.ToDecimal(t.PMC_20)).ToString(), PMC_21 = list.Sum(t => string.IsNullOrEmpty(t.PMC_21) ? 0 : Convert.ToDecimal(t.PMC_21)).ToString(), PMC_22 = list.Sum(t => string.IsNullOrEmpty(t.PMC_22) ? 0 : Convert.ToDecimal(t.PMC_22)).ToString(), PMC_23 = list.Sum(t => string.IsNullOrEmpty(t.PMC_23) ? 0 : Convert.ToDecimal(t.PMC_23)).ToString(), PMC_B_1 = list.Sum(t => string.IsNullOrEmpty(t.PMC_B_1) ? 0 : Convert.ToDecimal(t.PMC_B_1)).ToString(), PMC_B_2 = list.Sum(t => string.IsNullOrEmpty(t.PMC_B_2) ? 0 : Convert.ToDecimal(t.PMC_B_2)).ToString(), PMC_B_3 = list.Sum(t => string.IsNullOrEmpty(t.PMC_B_3) ? 0 : Convert.ToDecimal(t.PMC_B_3)).ToString(), PMC_B_4 = list.Sum(t => string.IsNullOrEmpty(t.PMC_B_4) ? 0 : Convert.ToDecimal(t.PMC_B_4)).ToString(), PMC_B_5 = list.Sum(t => string.IsNullOrEmpty(t.PMC_B_5) ? 0 : Convert.ToDecimal(t.PMC_B_5)).ToString(), PG_1 = list.Sum(t => string.IsNullOrEmpty(t.PG_1) ? 0 : Convert.ToDecimal(t.PG_1)).ToString(), PG_2 = list.Sum(t => string.IsNullOrEmpty(t.PG_2) ? 0 : Convert.ToDecimal(t.PG_2)).ToString(), PG_3 = list.Sum(t => string.IsNullOrEmpty(t.PG_3) ? 0 : Convert.ToDecimal(t.PG_3)).ToString(), PG_4 = list.Sum(t => string.IsNullOrEmpty(t.PG_4) ? 0 : Convert.ToDecimal(t.PG_4)).ToString(), PG_5 = list.Sum(t => string.IsNullOrEmpty(t.PG_5) ? 0 : Convert.ToDecimal(t.PG_5)).ToString(), PG_6 = list.Sum(t => string.IsNullOrEmpty(t.PG_6) ? 0 : Convert.ToDecimal(t.PG_6)).ToString(), PG_7 = list.Sum(t => string.IsNullOrEmpty(t.PG_7) ? 0 : Convert.ToDecimal(t.PG_7)).ToString(), PG_8 = list.Sum(t => string.IsNullOrEmpty(t.PG_8) ? 0 : Convert.ToDecimal(t.PG_8)).ToString(), PG_B_1 = list.Sum(t => string.IsNullOrEmpty(t.PG_B_1) ? 0 : Convert.ToDecimal(t.PG_B_1)).ToString(), PG_B_2 = list.Sum(t => string.IsNullOrEmpty(t.PG_B_2) ? 0 : Convert.ToDecimal(t.PG_B_2)).ToString(), PG_B_3 = list.Sum(t => string.IsNullOrEmpty(t.PG_B_3) ? 0 : Convert.ToDecimal(t.PG_B_3)).ToString(), PG_B_4 = list.Sum(t => string.IsNullOrEmpty(t.PG_B_4) ? 0 : Convert.ToDecimal(t.PG_B_4)).ToString(), PG_B_5 = list.Sum(t => string.IsNullOrEmpty(t.PG_B_5) ? 0 : Convert.ToDecimal(t.PG_B_5)).ToString(), KF_1 = list.Sum(t => string.IsNullOrEmpty(t.KF_1) ? 0 : Convert.ToDecimal(t.KF_1)).ToString(), KF_B_1 = list.Sum(t => string.IsNullOrEmpty(t.KF_B_1) ? 0 : Convert.ToDecimal(t.KF_B_1)).ToString(), KF_B_2 = list.Sum(t => string.IsNullOrEmpty(t.KF_B_2) ? 0 : Convert.ToDecimal(t.KF_B_2)).ToString(), KF_B_3 = list.Sum(t => string.IsNullOrEmpty(t.KF_B_3) ? 0 : Convert.ToDecimal(t.KF_B_3)).ToString(), KF_B_4 = list.Sum(t => string.IsNullOrEmpty(t.KF_B_4) ? 0 : Convert.ToDecimal(t.KF_B_4)).ToString(), KF_B_5 = list.Sum(t => string.IsNullOrEmpty(t.KF_B_5) ? 0 : Convert.ToDecimal(t.KF_B_5)).ToString(), WX_PMCDD_1 = list.Sum(t => string.IsNullOrEmpty(t.WX_PMCDD_1) ? 0 : Convert.ToDecimal(t.WX_PMCDD_1)).ToString(), WX_1 = list.Sum(t => string.IsNullOrEmpty(t.WX_1) ? 0 : Convert.ToDecimal(t.WX_1)).ToString(), WX_B_1 = list.Sum(t => string.IsNullOrEmpty(t.WX_B_1) ? 0 : Convert.ToDecimal(t.WX_B_1)).ToString(), WX_B_2 = list.Sum(t => string.IsNullOrEmpty(t.WX_B_2) ? 0 : Convert.ToDecimal(t.WX_B_2)).ToString(), WX_B_3 = list.Sum(t => string.IsNullOrEmpty(t.WX_B_3) ? 0 : Convert.ToDecimal(t.WX_B_3)).ToString(), WX_B_4 = list.Sum(t => string.IsNullOrEmpty(t.WX_B_4) ? 0 : Convert.ToDecimal(t.WX_B_4)).ToString(), WX_B_5 = list.Sum(t => string.IsNullOrEmpty(t.WX_B_5) ? 0 : Convert.ToDecimal(t.WX_B_5)).ToString(), PMC_Time = list[0].PMC_Time, PG_Time = list[0].PMC_Time, KF_Time = list[0].KF_Time, PMCDD_Time = list[0].PMCDD_Time, WX_Time = list[0].WX_Time, JE = list.Sum(t => string.IsNullOrEmpty(t.JE) ? 0 : Convert.ToDecimal(t.JE)).ToString(), IsPMC_Check = list[0].IsPMC_Check, IsPG_Check = list[0].IsPG_Check, IsKF_Check = list[0].IsKF_Check, IsPMCDD_Check = list[0].IsPMCDD_Check, IsWX_Check = list[0].IsWX_Check };
        }

        public static DataBase3JB_MCLBJJ GetTotalDataBase3JB_MCLBJJ(List<DataBase3JB_MCLBJJ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_MCLBJJ() { PZ = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBase3JB_MCLBJJ() { PZ = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, McCount = list.Sum(t => string.IsNullOrEmpty(t.McCount) ? 0 : Convert.ToDecimal(t.McCount)).ToString(), YkcpgCount = list.Sum(t => string.IsNullOrEmpty(t.YkcpgCount) ? 0 : Convert.ToDecimal(t.YkcpgCount)).ToString(), LbCount = list.Sum(t => string.IsNullOrEmpty(t.LbCount) ? 0 : Convert.ToDecimal(t.LbCount)).ToString(), McMoney = list.Sum(t => string.IsNullOrEmpty(t.McMoney) ? 0 : Convert.ToDecimal(t.McMoney)).ToString(), YkcpgMoney = list.Sum(t => string.IsNullOrEmpty(t.YkcpgMoney) ? 0 : Convert.ToDecimal(t.YkcpgMoney)).ToString(), LbMoney = list.Sum(t => string.IsNullOrEmpty(t.LbMoney) ? 0 : Convert.ToDecimal(t.LbMoney)).ToString(), Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0m : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBase3JB_XSMCLBJJ GetTotalDataBase3JB_XSMCLBJJ(List<DataBase3JB_XSMCLBJJ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_XSMCLBJJ { UserName = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBase3JB_XSMCLBJJ { UserName = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, McCount = list.Sum(t => string.IsNullOrEmpty(t.McCount) ? 0M : Convert.ToDecimal(t.McCount)).ToString(), LbCount = list.Sum(t => string.IsNullOrEmpty(t.LbCount) ? 0M : Convert.ToDecimal(t.LbCount)).ToString(), McMoney = list.Sum(t => string.IsNullOrEmpty(t.McMoney) ? 0M : Convert.ToDecimal(t.McMoney)).ToString(), LbMoney = list.Sum(t => string.IsNullOrEmpty(t.LbMoney) ? 0M : Convert.ToDecimal(t.LbMoney)).ToString() };
        }

        public static DataBase3JB_XCSJ_WX GetTotalDataBase3JB_XCSJ_WX(List<DataBase3JB_XCSJ_WX> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_XCSJ_WX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBase3JB_XCSJ_WX { UserCode = "合计", UserName = list.Count.ToString() + "人", TheYear = dateTime.Year, TheMonth = dateTime.Month, SlgDays = list.Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays)).ToString(), MqDays = list.Sum(t => string.IsNullOrEmpty(t.MqDays) ? 0M : Convert.ToDecimal(t.MqDays)).ToString(), BaseSalary = list.Sum(t => string.IsNullOrEmpty(t.BaseSalary) ? 0M : Convert.ToDecimal(t.BaseSalary)).ToString(), Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBase3JB_FZYH GetTotalDataBase3JB_FZYH(List<DataBase3JB_FZYH> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase3JB_FZYH { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBase3JB_FZYH { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, UserName = list.Count.ToString() + "人", SlgDays = list.Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays)).ToString(), Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_JT GetTotalDataBaseGeneral_JT(List<DataBaseGeneral_JT> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_JT { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_JT { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, UserName = list.Count.ToString() + '人', Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_ZF GetTotalDataBaseGeneral_ZF(List<DataBaseGeneral_ZF> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_ZF { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_ZF { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, UserName = list.Count.ToString() + '人', Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_YEY GetTotalDataBaseGeneral_YEY(List<DataBaseGeneral_YEY> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_YEY { FUserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_YEY { FUserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, MUserCode = list.Count.ToString() + "人", Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_JC_Dept GetTotalDataBaseGeneral_JC(List<DataBaseGeneral_JC_Dept> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_JC_Dept { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_JC_Dept { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, J = list.Sum(t => string.IsNullOrEmpty(t.J) ? 0M : Convert.ToDecimal(t.J)).ToString(), C = list.Sum(t => string.IsNullOrEmpty(t.C) ? 0M : Convert.ToDecimal(t.C)).ToString() };
        }

        public static DataBaseGeneral_BX GetTotalDataBaseGeneral_BX(List<DataBaseGeneral_BX> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_BX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_BX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, BX = list.Sum(t => string.IsNullOrEmpty(t.BX) ? 0M : Convert.ToDecimal(t.BX)).ToString(), GJJ = list.Sum(t => string.IsNullOrEmpty(t.GJJ) ? 0M : Convert.ToDecimal(t.GJJ)).ToString() };
        }

        public static DataBaseGeneral_WX GetTotalDataBaseGeneral_WX(List<DataBaseGeneral_WX> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_WX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_WX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_HZJJ_NoMoney GetTotalDataBaseGeneral_HZJJ_NoMoney(List<DataBaseGeneral_HZJJ_NoMoney> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_HZJJ_NoMoney { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_HZJJ_NoMoney { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, UserName = list.Count.ToString() + "人" };
        }

        public static DataBaseGeneral_JC_Factory GetTotalDataBaseGeneral_JC_Factory(List<DataBaseGeneral_JC_Factory> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_JC_Factory { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_JC_Factory { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, UserName = list.Count.ToString() + "人", Content = "奖：" + list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money) > 0 ? Convert.ToDecimal(t.Money) : 0M).ToString(), Money = "罚：" + list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money) > 0 ? 0M : Convert.ToDecimal(t.Money)).ToString(), JCId = "总金额:" + list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_LZ GetTotalDataBaseGeneral_LZ(List<DataBaseGeneral_LZ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_LZ { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_LZ { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, GZF = list.Sum(t => decimal.TryParse(t.GZF, out decimal gzf) ? gzf : 0M).ToString(), KQK = list.Sum(t => decimal.TryParse(t.KQK, out decimal kqk) ? kqk : 0M).ToString(), GJ = list.Sum(t => decimal.TryParse(t.GJ, out decimal gj) ? gj : 0M).ToString(), PXF = list.Sum(t => decimal.TryParse(t.PXF, out decimal pxf) ? pxf : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal money) ? money : 0M).ToString() };
        }

        public static DataBaseGeneral_JSF GetTotalDataBaseGeneral_JSF(List<DataBaseGeneral_JSF> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_JSF { Bjs_UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_JSF { Bjs_UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, Bjs_UserName = "共" + list.Count.ToString() + "人", Jsf = list.Sum(t => string.IsNullOrEmpty(t.Jsf) ? 0M : Convert.ToDecimal(t.Jsf)).ToString() };
        }

        public static DataBaseGeneral_CP GetTotalDataBaseGeneral_CP(List<DataBaseGeneral_CP> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_CP { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_CP { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_TJF GetTotalDataBaseGeneral_TJF(List<DataBaseGeneral_TJF> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_TJF { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_TJF { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_GZZRX GetTotalDataBaseGeneral_GZZRX(List<DataBaseGeneral_GZZRX> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_GZZRX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_GZZRX { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, MoneyPlus = list.Sum(t => string.IsNullOrEmpty(t.MoneyPlus) ? 0M : Convert.ToDecimal(t.MoneyPlus)).ToString(), MoneyMinus = list.Sum(t => string.IsNullOrEmpty(t.MoneyMinus) ? 0M : Convert.ToDecimal(t.MoneyMinus)).ToString() };
        }

        public static DataBaseGeneral_CQ GetTotalDataBaseGeneral_CQ(List<DataBaseGeneral_CQ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_CQ { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_CQ { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, UserName = list.Count.ToString() + "人", DayYcq = list.Sum(t => decimal.TryParse(t.DayYcq, out decimal dayycq) ? dayycq : 0M).ToString(), DayScq = list.Sum(t => decimal.TryParse(t.DayScq, out decimal dayscq) ? dayscq : 0M).ToString(), DayDktx = list.Sum(t => decimal.TryParse(t.DayDktx, out decimal daydktx) ? daydktx : 0M).ToString(), DayDx = list.Sum(t => decimal.TryParse(t.DayDx, out decimal daydx) ? daydx : 0M).ToString(), DayJbjx = list.Sum(t => decimal.TryParse(t.DayJbjx, out decimal dayjbjx) ? dayjbjx : 0M).ToString(), DayTxj = list.Sum(t => decimal.TryParse(t.DayTxj, out decimal daytxj) ? daytxj : 0M).ToString(), CountCdzt = list.Sum(t => decimal.TryParse(t.CountCdzt, out decimal countcdzt) ? countcdzt : 0M).ToString(), DayKg = list.Sum(t => decimal.TryParse(t.DayKg, out decimal daykg) ? daykg : 0M).ToString(), DaySj = list.Sum(t => decimal.TryParse(t.DaySj, out decimal daysj) ? daysj : 0M).ToString(), DayBj = list.Sum(t => decimal.TryParse(t.DayBj, out decimal daybj) ? daybj : 0M).ToString(), DayTotal = list.Sum(t => decimal.TryParse(t.DayTotal, out decimal daytotal) ? daytotal : 0M).ToString() };
        }

        public static DataBaseGeneral_GL GetTotalDataBaseGeneral_GL(List<DataBaseGeneral_GL> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_GL { UserCode = "合计", TheYear = dateTime.Year };
            }
            return new DataBaseGeneral_GL { UserCode = "合计", TheYear = dateTime.Year, UserName = list.Count.ToString() + "人", BeforeMoney = list.Sum(t => string.IsNullOrEmpty(t.BeforeMoney) ? 0M : Convert.ToDecimal(t.BeforeMoney)).ToString(), NowMoney = list.Sum(t => string.IsNullOrEmpty(t.NowMoney) ? 0M : Convert.ToDecimal(t.NowMoney)).ToString(), Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_BF GetTotalDataBaseGeneral_BF(List<DataBaseGeneral_BF> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBaseGeneral_BF { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBaseGeneral_BF { UserCode = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static BaseHeadcount GetTotalBaseHeadcount(List<BaseHeadcount> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new BaseHeadcount { DeptName = "合计" };
            }
            return new BaseHeadcount { DeptName = "合计", Name = list.Count.ToString(), UserCount = list.Sum(t => string.IsNullOrEmpty(t.UserCount) ? 0M : Convert.ToDecimal(t.UserCount)).ToString(), TheYear = dateTime.Year, TheMonth = dateTime.Month };
        }

        public static GZD GetTotalGZD(List<GZD> list, DateTime dateTime)
        {
            return (list == null || list.Count == 0) ? new GZD { Factory = "合计" } : new GZD { Factory = "合计", JBGZE = list.Sum(t => string.IsNullOrEmpty(t.JBGZE) ? 0M : Convert.ToDecimal(t.JBGZE)).ToString(), GLE = list.Sum(t => string.IsNullOrEmpty(t.GLE) ? 0M : Convert.ToDecimal(t.GLE)).ToString(), JNGZE = list.Sum(t => string.IsNullOrEmpty(t.JNGZE) ? 0M : Convert.ToDecimal(t.JNGZE)).ToString(), JBGZE1 = list.Sum(t => string.IsNullOrEmpty(t.JBGZE1) ? 0M : Convert.ToDecimal(t.JBGZE1)).ToString(), JBGZE2 = list.Sum(t => string.IsNullOrEmpty(t.JBGZE2) ? 0M : Convert.ToDecimal(t.JBGZE2)).ToString(), QQJ = list.Sum(t => string.IsNullOrEmpty(t.QQJ) ? 0M : Convert.ToDecimal(t.QQJ)).ToString(), RG = list.Sum(t => string.IsNullOrEmpty(t.RG) ? 0M : Convert.ToDecimal(t.RG)).ToString(), TXJBZ = list.Sum(t => string.IsNullOrEmpty(t.TXJBZ) ? 0M : Convert.ToDecimal(t.TXJBZ)).ToString(), BFGZ = list.Sum(t => string.IsNullOrEmpty(t.BFGZ) ? 0M : Convert.ToDecimal(t.BFGZ)).ToString(), SFJJ = list.Sum(t => string.IsNullOrEmpty(t.SFJJ) ? 0M : Convert.ToDecimal(t.SFJJ)).ToString(), JJGZ1 = list.Sum(t => string.IsNullOrEmpty(t.JJGZ1) ? 0M : Convert.ToDecimal(t.JJGZ1)).ToString(), JJGZ2 = list.Sum(t => string.IsNullOrEmpty(t.JJGZ2) ? 0M : Convert.ToDecimal(t.JJGZ2)).ToString(), JJGZ3 = list.Sum(t => string.IsNullOrEmpty(t.JJGZ3) ? 0M : Convert.ToDecimal(t.JJGZ3)).ToString(), CLKHGZ = list.Sum(t => string.IsNullOrEmpty(t.CLKHGZ) ? 0M : Convert.ToDecimal(t.CLKHGZ)).ToString(), ZLKHGZ = list.Sum(t => string.IsNullOrEmpty(t.ZLKHGZ) ? 0M : Convert.ToDecimal(t.ZLKHGZ)).ToString(), GRKHGZ = list.Sum(t => string.IsNullOrEmpty(t.GRKHGZ) ? 0M : Convert.ToDecimal(t.GRKHGZ)).ToString(), BZKHGZ = list.Sum(t => string.IsNullOrEmpty(t.BZKHGZ) ? 0M : Convert.ToDecimal(t.BZKHGZ)).ToString(), PGYH = list.Sum(t => string.IsNullOrEmpty(t.PGYH) ? 0M : Convert.ToDecimal(t.PGYH)).ToString(), TBGZ = list.Sum(t => string.IsNullOrEmpty(t.TBGZ) ? 0M : Convert.ToDecimal(t.TBGZ)).ToString(), BZF = list.Sum(t => string.IsNullOrEmpty(t.BZF) ? 0M : Convert.ToDecimal(t.BZF)).ToString(), RZBZ = list.Sum(t => string.IsNullOrEmpty(t.RZBZ) ? 0M : Convert.ToDecimal(t.RZBZ)).ToString(), BCBZ = list.Sum(t => string.IsNullOrEmpty(t.BCBZ) ? 0M : Convert.ToDecimal(t.BCBZ)).ToString(), FPJF = list.Sum(t => string.IsNullOrEmpty(t.FPJF) ? 0M : Convert.ToDecimal(t.FPJF)).ToString(), KFSSBZ = list.Sum(t => string.IsNullOrEmpty(t.KFSSBZ) ? 0M : Convert.ToDecimal(t.KFSSBZ)).ToString(), GCSSBZ = list.Sum(t => string.IsNullOrEmpty(t.GCSSBZ) ? 0M : Convert.ToDecimal(t.GCSSBZ)).ToString(), TMBZ = list.Sum(t => string.IsNullOrEmpty(t.TMBZ) ? 0M : Convert.ToDecimal(t.TMBZ)).ToString(), JJBT = list.Sum(t => string.IsNullOrEmpty(t.JJBT) ? 0M : Convert.ToDecimal(t.JJBT)).ToString(), JTBZ = list.Sum(t => string.IsNullOrEmpty(t.JTBZ) ? 0M : Convert.ToDecimal(t.JTBZ)).ToString(), ZFBZ = list.Sum(t => string.IsNullOrEmpty(t.ZFBZ) ? 0M : Convert.ToDecimal(t.ZFBZ)).ToString(), YEYBZ = list.Sum(t => string.IsNullOrEmpty(t.YEYBZ) ? 0M : Convert.ToDecimal(t.YEYBZ)).ToString(), DHBT = list.Sum(t => string.IsNullOrEmpty(t.DHBT) ? 0M : Convert.ToDecimal(t.DHBT)).ToString(), TJFBX = list.Sum(t => string.IsNullOrEmpty(t.TJFBX) ? 0M : Convert.ToDecimal(t.TJFBX)).ToString(), JSF = list.Sum(t => string.IsNullOrEmpty(t.JSF) ? 0M : Convert.ToDecimal(t.JSF)).ToString(), CPBX = list.Sum(t => string.IsNullOrEmpty(t.CPBX) ? 0M : Convert.ToDecimal(t.CPBX)).ToString(), WXBZ = list.Sum(t => string.IsNullOrEmpty(t.WXBZ) ? 0M : Convert.ToDecimal(t.WXBZ)).ToString(), JL = list.Sum(t => string.IsNullOrEmpty(t.JL) ? 0M : Convert.ToDecimal(t.JL)).ToString(), CJ = list.Sum(t => string.IsNullOrEmpty(t.CJ) ? 0M : Convert.ToDecimal(t.CJ)).ToString(), CXFLJF = list.Sum(t => string.IsNullOrEmpty(t.CXFLJF) ? 0M : Convert.ToDecimal(t.CXFLJF)).ToString(), PSBZ = list.Sum(t => string.IsNullOrEmpty(t.PSBZ) ? 0M : Convert.ToDecimal(t.PSBZ)).ToString(), PSFK = list.Sum(t => string.IsNullOrEmpty(t.PSFK) ? 0M : Convert.ToDecimal(t.PSFK)).ToString(), CJFK = list.Sum(t => string.IsNullOrEmpty(t.CJFK) ? 0M : Convert.ToDecimal(t.CJFK)).ToString(), PGFK = list.Sum(t => string.IsNullOrEmpty(t.PGFK) ? 0M : Convert.ToDecimal(t.PGFK)).ToString(), GZZRXKK = list.Sum(t => string.IsNullOrEmpty(t.GZZRXKK) ? 0M : Convert.ToDecimal(t.GZZRXKK)).ToString(), DK_LZKK = list.Sum(t => string.IsNullOrEmpty(t.DK_LZKK) ? 0M : Convert.ToDecimal(t.DK_LZKK)).ToString(), GSJL = list.Sum(t => string.IsNullOrEmpty(t.GSJL) ? 0M : Convert.ToDecimal(t.GSJL)).ToString(), GSCJ = list.Sum(t => string.IsNullOrEmpty(t.GSCJ) ? 0M : Convert.ToDecimal(t.GSCJ)).ToString(), YFHJ = list.Sum(t => string.IsNullOrEmpty(t.YFHJ) ? 0M : Convert.ToDecimal(t.YFHJ)).ToString(), GHHZJ = list.Sum(t => string.IsNullOrEmpty(t.GHHZJ) ? 0M : Convert.ToDecimal(t.GHHZJ)).ToString(), DKGJJ = list.Sum(t => string.IsNullOrEmpty(t.DKGJJ) ? 0M : Convert.ToDecimal(t.DKGJJ)).ToString(), DKBXF = list.Sum(t => string.IsNullOrEmpty(t.DKBXF) ? 0M : Convert.ToDecimal(t.DKBXF)).ToString(), GS = list.Sum(t => string.IsNullOrEmpty(t.GS) ? 0M : Convert.ToDecimal(t.GS)).ToString(), SHKK = list.Sum(t => string.IsNullOrEmpty(t.SHKK) ? 0M : Convert.ToDecimal(t.SHKK)).ToString(), SFHJ = list.Sum(t => string.IsNullOrEmpty(t.SFHJ) ? 0M : Convert.ToDecimal(t.SFHJ)).ToString() };
        }

        public static DataBase3JB_XZF GetTotalDataBase3JB_XZF(List<DataBase3JB_XZF> list, DateTime dateTime)
        {
            return (list == null || list.Count == 0) ? new DataBase3JB_XZF { UserCode = "合计" } : new DataBase3JB_XZF { UserCode = "合计", UserName = list.Count.ToString() + "人", DayCq = list.Sum(t => string.IsNullOrEmpty(t.DayCq) ? 0M : Convert.ToDecimal(t.DayCq)).ToString(), DayYcq = list.Sum(t => string.IsNullOrEmpty(t.DayYcq) ? 0M : Convert.ToDecimal(t.DayYcq)).ToString(), Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0M : Convert.ToDecimal(t.Money)).ToString() };
        }

        public static DataBaseGeneral_HZJJ_High GetTotalDataBaseGeneral_HZJJ_High(List<DataBaseGeneral_HZJJ_High> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_HZJJ_High { UserCode = "合计", UserName = list.Count.ToString() + "人" } : new DataBaseGeneral_HZJJ_High { UserCode = "合计", UserName = list.Count.ToString() + "人", HZJJ = list.Sum(t => string.IsNullOrEmpty(t.HZJJ) ? 0M : Convert.ToDecimal(t.HZJJ)).ToString() };
        }

        public static DataBase3MJ_XSGJJ GetTotalDataBase3MJ_XSGJJ(List<DataBase3MJ_XSGJJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3MJ_XSGJJ { No = "合计", Money = 0.ToString(), DS = 0.ToString() } : new DataBase3MJ_XSGJJ { No = "合计", Money = list.Sum(t => decimal.TryParse(t.Money, out decimal result) ? result : 0).ToString(), DS = list.Sum(t => decimal.TryParse(t.DS, out decimal ds) ? ds : 0).ToString() };
        }

        public static DataBase3JB_PGYHZTS GetTotalDataBase3JB_PGYHZTS(List<DataBase3JB_PGYHZTS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3JB_PGYHZTS { UserCode = "合计" } : new DataBase3JB_PGYHZTS { UserCode = "合计", UserName = list.Count.ToString() + "人", Days = list.Sum(t => string.IsNullOrEmpty(t.Days) ? 0M : Convert.ToDecimal(t.Days)).ToString() };
        }

        public static DataBase3MJ_DJCJYB GetTotalDataBase3MJ_DJCJYB(List<DataBase3MJ_DJCJYB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3MJ_DJCJYB { No = "合计", SCLJ = 0.ToString() } : new DataBase3MJ_DJCJYB { No = "合计", SCLJ = list.Sum(t => decimal.TryParse(t.SCLJ, out decimal result) ? result : 0M).ToString() };
        }

        public static DataBase3MJ_XJCJYB GetTotalDataBase3MJ_XJCJYB(List<DataBase3MJ_XJCJYB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3MJ_XJCJYB { No = "合计", SCSL = 0.ToString() } : new DataBase3MJ_XJCJYB { No = "合计", SCSL = list.Sum(t => decimal.TryParse(t.SCSL, out decimal result) ? result : 0M).ToString() };
        }

        public static DataBaseGeneral_RZBZ GetTotalDataBaseGeneral_RZBZ(List<DataBaseGeneral_RZBZ> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_RZBZ { No = "合计", Day_BZ = 0.ToString(), Money = 0.ToString() } : new DataBaseGeneral_RZBZ { No = "合计", Day_BZ = list.Sum(t => decimal.TryParse(t.Day_BZ, out decimal result) ? result : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal money) ? money : 0M).ToString() };
        }

        public static DataBaseGeneral_FZYH GetTotalDataBaseGeneral_FZYH(List<DataBaseGeneral_FZYH> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_FZYH { No = "合计", DayCount = 0.ToString(), Money = 0.ToString() } : new DataBaseGeneral_FZYH { No = "合计", DayCount = list.Sum(t => decimal.TryParse(t.DayCount, out decimal result) ? result : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal money) ? money : 0M).ToString() };
        }

        public static DataBase3YL_JJTJB GetTotalDataBase3YL_JJTJB(List<DataBase3YL_JJTJB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3YL_JJTJB { No = "合计", JJGZ = 0.ToString(), JYG = 0.ToString(), QYG = 0.ToString(), BZF = 0.ToString() } : new DataBase3YL_JJTJB { No = "合计", JJGZ = list.Sum(t => decimal.TryParse(t.JJGZ, out decimal result) ? result : 0M).ToString(), JYG = list.Sum(t => decimal.TryParse(t.JYG, out decimal result) ? result : 0M).ToString(), QYG = list.Sum(t => decimal.TryParse(t.QYG, out decimal result) ? result : 0M).ToString(), BZF = list.Sum(t => decimal.TryParse(t.BZF, out decimal result) ? result : 0M).ToString() };
        }

        public static DataBase3YL_JJHS GetTotalDataBase3YL_JJHS(List<DataBase3YL_JJHS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3YL_JJHS { No = "合计", JJGZ = 0.ToString(), SL = 0.ToString() } : new DataBase3YL_JJHS { No = "合计", JJGZ = list.Sum(t => decimal.TryParse(t.JJGZ, out decimal result) ? result : 0M).ToString(), SL = list.Sum(t => decimal.TryParse(t.SL, out decimal result) ? result : 0M).ToString() };
        }

        public static DataBaseGeneral_JBSPB GetTotalDataBaseGeneral_JBSPB(List<DataBaseGeneral_JBSPB> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_JBSPB { No = "合计", JBTS = 0.ToString(), Money = 0.ToString() } : new DataBaseGeneral_JBSPB { No = "合计", JBTS = list.Sum(t => decimal.TryParse(t.JBTS, out decimal result) ? result : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal result) ? result : 0M).ToString() };
        }

        public static DataBase3PY_PYGZHS GetTotalDataBase3PY_PYGZHS(List<DataBase3PY_PYGZHS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_PYGZHS { No = "合计", KYL = 0.ToString(), YJP = 0.ToString(), E_KHJE = 0.ToString(), E_JJJE = 0.ToString(), E_Money = 0.ToString() } : new DataBase3PY_PYGZHS { No = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal kyl) ? kyl : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal yjp) ? yjp : 0M).ToString(), E_KHJE = list.Sum(t => decimal.TryParse(t.E_KHJE, out decimal khje) ? khje : 0M).ToString(), E_JJJE = list.Sum(t => decimal.TryParse(t.E_JJJE, out decimal jjje) ? jjje : 0M).ToString(), E_Money = list.Sum(t => decimal.TryParse(t.E_Money, out decimal money) ? money : 0M).ToString() };
        }

        public static DataBase3PY_PYGZHS GetTotalDataBase3PY_PYGZHS_Sum(List<DataBase3PY_PYGZHS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_PYGZHS { No = "合计", E_KHJE = 0.ToString(), E_JJJE = 0.ToString(), E_Money = 0.ToString() } : new DataBase3PY_PYGZHS { No = "合计", E_KHJE = list.Sum(t => decimal.TryParse(t.E_KHJE, out decimal khje) ? khje : 0M).ToString(), E_JJJE = list.Sum(t => decimal.TryParse(t.E_JJJE, out decimal jjje) ? jjje : 0M).ToString(), E_Money = list.Sum(t => decimal.TryParse(t.E_Money, out decimal money) ? money : 0M).ToString() };
        }

        public static DataBase3PY_JXG_KH GetTotalDataBase3PY_JXG_KH(List<DataBase3PY_JXG_KH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_JXG_KH { No = "合计", KYL = 0.ToString(), YJP = 0.ToString(), KHJE = 0.ToString(), JJJE = 0.ToString(), LT = 0.ToString(), KHJE2 = 0.ToString() } : new DataBase3PY_JXG_KH { No = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal kyl) ? kyl : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal yjp) ? yjp : 0M).ToString(), KHJE = list.Sum(t => decimal.TryParse(t.KHJE, out decimal khje) ? khje : 0M).ToString(), JJJE = list.Sum(t => decimal.TryParse(t.JJJE, out decimal jjje) ? jjje : 0M).ToString(), LT = list.Sum(t => decimal.TryParse(t.LT, out decimal d) ? d : 0m).ToString(), KHJE2 = list.Sum(t => decimal.TryParse(t.KHJE2, out decimal d) ? d : 0m).ToString() };
        }

        public static DataBase3PY_CLG_KH GetTotalDataBase3PY_CLG_KH(List<DataBase3PY_CLG_KH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_CLG_KH { No = "合计", KYL = 0.ToString(), YJP = 0.ToString(), KHJE = 0.ToString() } : new DataBase3PY_CLG_KH { No = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal kyl) ? kyl : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal yjp) ? yjp : 0M).ToString(), KHJE = list.Sum(t => decimal.TryParse(t.KHJE, out decimal khje) ? khje : 0M).ToString() };
        }

        public static DataBase3PY_BZ_KH GetTotalDataBase3PY_BZ_KH(List<DataBase3PY_BZ_KH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_BZ_KH { No = "合计", PY_KYL = 0.ToString(), PY_YJP = 0.ToString(), XJX_KYL = 0.ToString(), XJX_YJP = 0.ToString(), XJX2_GY = 0.ToString(), XJX2_BY = 0.ToString(), XJX2_PZ = 0.ToString() } : new DataBase3PY_BZ_KH { No = "合计", PY_KYL = list.Sum(t => decimal.TryParse(t.PY_KYL, out decimal result) ? result : 0M).ToString(), PY_YJP = list.Sum(t => decimal.TryParse(t.PY_YJP, out decimal result) ? result : 0M).ToString(), XJX_KYL = list.Sum(t => decimal.TryParse(t.XJX_KYL, out decimal result) ? result : 0M).ToString(), XJX_YJP = list.Sum(t => decimal.TryParse(t.XJX_YJP, out decimal result) ? result : 0M).ToString(), XJX2_GY = list.Sum(t => decimal.TryParse(t.XJX2_GY, out decimal result) ? result : 0M).ToString(), XJX2_BY = list.Sum(t => decimal.TryParse(t.XJX2_BY, out decimal result) ? result : 0M).ToString(), XJX2_PZ = list.Sum(t => decimal.TryParse(t.XJX2_PZ, out decimal result) ? result : 0M).ToString() };
        }

        public static DataBase3PY_BZ_KH_Sum GetTotalDataBase3PY_BZ_KH_Sum(List<DataBase3PY_BZ_KH_Sum> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_BZ_KH_Sum { No = "合计", KHJE = 0.ToString() } : new DataBase3PY_BZ_KH_Sum { No = "合计", KHJE = list.Sum(t => decimal.TryParse(t.KHJE, out decimal khje) ? khje : 0M).ToString() };
        }

        public static DataBase3PY_BG GetTotalDataBase3PY_BG(List<DataBase3PY_BG> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_BG { No = "合计", SLGTS = 0.ToString(), YCQTS = 0.ToString(), XTGTS = 0.ToString(), SCQTS = 0.ToString(), JBGZE = 0.ToString(), JNGZE = 0.ToString(), JiaBGZE = 0.ToString(), BZKHGZ = 0.ToString(), JXGRKHGZ = 0.ToString(), JXJJGZ = 0.ToString(), CLGGRKH = 0.ToString(), PYJJ = 0.ToString(), PYKH = 0.ToString(), CSJJ = 0.ToString(), CSKH = 0.ToString(), HJ = 0.ToString() } : new DataBase3PY_BG { No = "合计", SLGTS = list.Sum(t => decimal.TryParse(t.SLGTS, out decimal d) ? d : 0M).ToString(), YCQTS = list.Average(t => decimal.TryParse(t.YCQTS, out decimal d) ? d : 0M).ToString(), XTGTS = list.Sum(t => decimal.TryParse(t.XTGTS, out decimal d) ? d : 0M).ToString(), SCQTS = list.Sum(t => decimal.TryParse(t.SCQTS, out decimal d) ? d : 0M).ToString(), JBGZE = list.Sum(t => decimal.TryParse(t.JBGZE, out decimal d) ? d : 0M).ToString(), JNGZE = list.Sum(t => decimal.TryParse(t.JNGZE, out decimal d) ? d : 0M).ToString(), JiaBGZE = list.Sum(t => decimal.TryParse(t.JiaBGZE, out decimal d) ? d : 0M).ToString(), BZKHGZ = list.Sum(t => decimal.TryParse(t.BZKHGZ, out decimal d) ? d : 0M).ToString(), JXGRKHGZ = list.Sum(t => decimal.TryParse(t.JXGRKHGZ, out decimal d) ? d : 0M).ToString(), JXJJGZ = list.Sum(t => decimal.TryParse(t.JXJJGZ, out decimal d) ? d : 0M).ToString(), CLGGRKH = list.Sum(t => decimal.TryParse(t.CLGGRKH, out decimal d) ? d : 0M).ToString(), PYJJ = list.Sum(t => decimal.TryParse(t.PYJJ, out decimal d) ? d : 0M).ToString(), PYKH = list.Sum(t => decimal.TryParse(t.PYKH, out decimal d) ? d : 0M).ToString(), CSJJ = list.Sum(t => decimal.TryParse(t.CSJJ, out decimal d) ? d : 0M).ToString(), CSKH = list.Sum(t => decimal.TryParse(t.CSKH, out decimal d) ? d : 0M).ToString(), HJ = list.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3PY_PS GetTotalDataBase3PY_PS(List<DataBase3PY_PS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3PY_PS { GH = "合计", Money = 0.ToString() } : new DataBase3PY_PS { GH = "合计", Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBaseGeneral_PMCPS GetTotalDataBaseGeneral_PMCPS(List<DataBaseGeneral_PMCPS> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_PMCPS { GH = "合计", Lt_Sl = 0.ToString(), Dt_Sl = 0.ToString(), Xj_Sl = 0.ToString(), G_Sl = 0.ToString(), Money = 0.ToString() } : new DataBaseGeneral_PMCPS { GH = "合计", Lt_Sl = list.Sum(t => decimal.TryParse(t.Lt_Sl, out decimal d) ? d : 0M).ToString(), Dt_Sl = list.Sum(t => decimal.TryParse(t.Dt_Sl, out decimal d) ? d : 0M).ToString(), Xj_Sl = list.Sum(t => decimal.TryParse(t.Xj_Sl, out decimal d) ? d : 0M).ToString(), G_Sl = list.Sum(t => decimal.TryParse(t.G_Sl, out decimal d) ? d : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBaseGeneral_KFSS GetTotalDataBaseGeneral_KFSS(List<DataBaseGeneral_KFSS> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_KFSS { No = "合计", Money = 0.ToString() } : new DataBaseGeneral_KFSS { No = "合计", SL = list.Sum(t => decimal.TryParse(t.SL, out decimal d) ? d : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_01_ZYJJHKH GetTotalDataBase3SC_01_ZYJJHKH(List<DataBase3SC_01_ZYJJHKH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_01_ZYJJHKH { No = "合计", Code = 0.ToString(), KYL = 0.ToString(), YJP = 0.ToString(), KH = 0.ToString(), JJ = 0.ToString(), HJ = 0.ToString() } : new DataBase3SC_01_ZYJJHKH { No = "合计", Code = list.Count.ToString(), KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M).ToString(), KH = list.Sum(t => decimal.TryParse(t.KH, out decimal d) ? d : 0M).ToString(), JJ = list.Sum(t => decimal.TryParse(t.JJ, out decimal d) ? d : 0M).ToString(), HJ = list.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_04_3DZY GetTotalDataBase3SC_04_3DZY(List<DataBase3SC_04_3DZY> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_04_3DZY { No = "合计", HSTS = 0.ToString(), HSGZ = 0.ToString() } : new DataBase3SC_04_3DZY { No = "合计", HSTS = list.Sum(t => decimal.TryParse(t.HSTS, out decimal d) ? d : 0M).ToString(), HSGZ = list.Sum(t => decimal.TryParse(t.HSGZ, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_05_KYBG GetTotalDataBase3SC_05_KYBG(List<DataBase3SC_05_KYBG> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_05_KYBG { No = "合计", GRKYL = 0.ToString(), CQ = 0.ToString(), JE = 0.ToString(), KH = 0.ToString() } : new DataBase3SC_05_KYBG { No = "合计", GRKYL = list.Sum(t => decimal.TryParse(t.GRKYL, out decimal d) ? d : 0M).ToString(), CQ = list.Sum(t => decimal.TryParse(t.CQ, out decimal d) ? d : 0M).ToString(), JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString(), KH = list.Sum(t => decimal.TryParse(t.KH, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_06_HSJYADL GetTotalDataBase3SC_06_HSJYADL(List<DataBase3SC_06_HSJYADL> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_06_HSJYADL { No = "合计", KYL = 0.ToString(), YJP = 0.ToString(), XBBHG_JJ = 0.ToString(), XBBHG_PS = 0.ToString(), LB_JJ = 0.ToString(), LB_PS = 0.ToString(), QX_JJ = 0.ToString(), QX_PS = 0.ToString() } : new DataBase3SC_06_HSJYADL { No = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M).ToString(), XBBHG_JJ = list.Sum(t => decimal.TryParse(t.XBBHG_JJ, out decimal d) ? d : 0M).ToString(), XBBHG_PS = list.Sum(t => decimal.TryParse(t.XBBHG_PS, out decimal d) ? d : 0M).ToString(), LB_JJ = list.Sum(t => decimal.TryParse(t.LB_JJ, out decimal d) ? d : 0M).ToString(), LB_PS = list.Sum(t => decimal.TryParse(t.LB_PS, out decimal d) ? d : 0M).ToString(), QX_JJ = list.Sum(t => decimal.TryParse(t.QX_JJ, out decimal d) ? d : 0M).ToString(), QX_PS = list.Sum(t => decimal.TryParse(t.QX_PS, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_08_TBSJQXS GetTotalDataBase3SC_08_TBSJQXS(List<DataBase3SC_08_TBSJQXS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_08_TBSJQXS { No = "合计", GD = "合计", QX = 0.ToString() } : new DataBase3SC_08_TBSJQXS { No = "合计", GD = "合计", QX = list.Sum(t => decimal.TryParse(t.QX, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_09_TBJJJL GetTotalDataBase3SC_09_TBJJJL(List<DataBase3SC_09_TBJJJL> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_09_TBJJJL { No = "合计", CHMC = "合计", BW = 0.ToString(), DZLC = 0.ToString() } : new DataBase3SC_09_TBJJJL { No = "合计", CHMC = "合计", BW = list.Sum(t => decimal.TryParse(t.BW, out decimal d) ? d : 0M).ToString(), DZLC = list.Sum(t => decimal.TryParse(t.DZLC, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_10_ZYJJADL GetTotalDataBase3SC_10_ZYJJADL(List<DataBase3SC_10_ZYJJADL> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_10_ZYJJADL { No = "合计", KYL = 0.ToString(), YJP = 0.ToString(), ZZ_JJ = 0.ToString(), ZZ_PS = 0.ToString(), ZZan_JJ = 0.ToString(), ZZan_PS = 0.ToString(), ZK_JJ = 0.ToString(), ZK_PS = 0.ToString(), ZZhang_JJ = 0.ToString(), ZZhang_PS = 0.ToString(), GB_JJ = 0.ToString(), GB_PS = 0.ToString(), HJ_JJ = 0.ToString(), HJ_PS = 0.ToString() } : new DataBase3SC_10_ZYJJADL { No = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M).ToString(), ZZ_JJ = list.Sum(t => decimal.TryParse(t.ZZ_JJ, out decimal d) ? d : 0M).ToString(), ZZ_PS = list.Sum(t => decimal.TryParse(t.ZZ_PS, out decimal d) ? d : 0M).ToString(), ZZan_JJ = list.Sum(t => decimal.TryParse(t.ZZan_JJ, out decimal d) ? d : 0M).ToString(), ZZan_PS = list.Sum(t => decimal.TryParse(t.ZZan_PS, out decimal d) ? d : 0M).ToString(), ZK_JJ = list.Sum(t => decimal.TryParse(t.ZK_JJ, out decimal d) ? d : 0M).ToString(), ZK_PS = list.Sum(t => decimal.TryParse(t.ZK_PS, out decimal d) ? d : 0M).ToString(), ZZhang_JJ = list.Sum(t => decimal.TryParse(t.ZZhang_JJ, out decimal d) ? d : 0M).ToString(), ZZhang_PS = list.Sum(t => decimal.TryParse(t.ZZhang_PS, out decimal d) ? d : 0M).ToString(), GB_JJ = list.Sum(t => decimal.TryParse(t.GB_JJ, out decimal d) ? d : 0M).ToString(), GB_PS = list.Sum(t => decimal.TryParse(t.GB_PS, out decimal d) ? d : 0M).ToString(), HJ_JJ = list.Sum(t => decimal.TryParse(t.HJ_JJ, out decimal d) ? d : 0M).ToString(), HJ_PS = list.Sum(t => decimal.TryParse(t.HJ_PS, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_12_JSYBG GetTotalDataBase3SC_12_JSYBG(List<DataBase3SC_12_JSYBG> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_12_JSYBG { No = "合计", GD = "合计", KYL = 0.ToString(), ZYQXS = 0.ToString(), BWQX = 0.ToString(), KYQX = 0.ToString(), JE = 0.ToString() } : new DataBase3SC_12_JSYBG { No = "合计", GD = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), ZYQXS = list.Sum(t => decimal.TryParse(t.ZYQXS, out decimal d) ? d : 0M).ToString(), BWQX = list.Sum(t => decimal.TryParse(t.BWQX, out decimal d) ? d : 0M).ToString(), KYQX = list.Sum(t => decimal.TryParse(t.KYQX, out decimal d) ? d : 0M).ToString(), JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_12_JSYBG GetTotalDataBase3SC_12_JSYBG(DateTime dateTime)
        {
            var list = new BaseDal<DataBase3SC_11_JSYKH>().GetList(t => t.TheYear == dateTime.Year && t.TheMonth == dateTime.Month).ToList();
            return (list == null || list.Count == 0) ? new DataBase3SC_12_JSYBG { No = "合计", GD = "生产管理部", KYL = 0.ToString(), ZYQXS = 0.ToString(), BWQX = 0.ToString(), KYQX = 0.ToString() } : new DataBase3SC_12_JSYBG { No = "合计", GD = "生产管理部", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), ZYQXS = list.Sum(t => decimal.TryParse(t.QXS, out decimal d) ? d : 0M).ToString(), BWQX = list.Sum(t => decimal.TryParse(t.BWQXS, out decimal d) ? d : 0M).ToString(), KYQX = list.Sum(t => decimal.TryParse(t.KYQXS, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBaseGeneral_RZBZ_Month GetTotalDataBaseGeneral_RZBZ_Month(List<DataBaseGeneral_RZBZ_Month> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_RZBZ_Month { No = "合计", BZTS = 0.ToString(), Money = 0.ToString() } : new DataBaseGeneral_RZBZ_Month { No = "合计", BZTS = list.Sum(t => decimal.TryParse(t.BZTS, out decimal d) ? d : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3SC_13_BG GetTotalDataBase3SC_13_BG(List<DataBase3SC_13_BG> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3SC_13_BG { No = "合计", GZ = "合计", XTTS = 0.ToString(), SLGTS = 0.ToString(), YCQTS = 0.ToString(), HJ_SCQTS = 0.ToString(), JBGZ = 0.ToString(), JBGZE = 0.ToString(), JSYKHGZ = 0.ToString(), KYJJ = 0.ToString(), KYKH = 0.ToString(), HSJJHKH = 0.ToString(), QZKH = 0.ToString(), BCJJHKH = 0.ToString(), SYGX = 0.ToString(), TBGXSKH = 0.ToString(), TBGXXKH = 0.ToString(), XCGKH = 0.ToString(), ZYJJHKH = 0.ToString(), HJ_Money = 0.ToString() } : new DataBase3SC_13_BG { No = "合计", GZ = "合计", XTTS = list.Sum(t => decimal.TryParse(t.XTTS, out decimal d) ? d : 0M).ToString(), SLGTS = list.Sum(t => decimal.TryParse(t.SLGTS, out decimal d) ? d : 0M).ToString(), YCQTS = list.Sum(t => decimal.TryParse(t.YCQTS, out decimal d) ? d : 0M).ToString(), HJ_SCQTS = list.Sum(t => decimal.TryParse(t.HJ_SCQTS, out decimal d) ? d : 0M).ToString(), JBGZ = list.Sum(t => decimal.TryParse(t.JBGZ, out decimal d) ? d : 0M).ToString(), JBGZE = list.Sum(t => decimal.TryParse(t.JBGZE, out decimal d) ? d : 0M).ToString(), JSYKHGZ = list.Sum(t => decimal.TryParse(t.JSYKHGZ, out decimal d) ? d : 0M).ToString(), KYJJ = list.Sum(t => decimal.TryParse(t.KYJJ, out decimal d) ? d : 0M).ToString(), KYKH = list.Sum(t => decimal.TryParse(t.KYKH, out decimal d) ? d : 0M).ToString(), HSJJHKH = list.Sum(t => decimal.TryParse(t.HSJJHKH, out decimal d) ? d : 0M).ToString(), QZKH = list.Sum(t => decimal.TryParse(t.QZKH, out decimal d) ? d : 0M).ToString(), BCJJHKH = list.Sum(t => decimal.TryParse(t.BCJJHKH, out decimal d) ? d : 0M).ToString(), SYGX = list.Sum(t => decimal.TryParse(t.SYGX, out decimal d) ? d : 0M).ToString(), TBGXSKH = list.Sum(t => decimal.TryParse(t.TBGXSKH, out decimal d) ? d : 0M).ToString(), TBGXXKH = list.Sum(t => decimal.TryParse(t.TBGXXKH, out decimal d) ? d : 0M).ToString(), XCGKH = list.Sum(t => decimal.TryParse(t.XCGKH, out decimal d) ? d : 0M).ToString(), ZYJJHKH = list.Sum(t => decimal.TryParse(t.ZYJJHKH, out decimal d) ? d : 0M).ToString(), HJ_Money = list.Sum(t => decimal.TryParse(t.HJ_Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_GYMJ_01_PMCDDGYJJJP GetTotalDataBase3CX_GYMJ_01_PMCDDGYJJJP(List<DataBase3CX_GYMJ_01_PMCDDGYJJJP> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_GYMJ_01_PMCDDGYJJJP { No = "合计", PZ = "合计", JHJPS = 0.ToString() } : new DataBase3CX_GYMJ_01_PMCDDGYJJJP { No = "合计", PZ = "合计", JHJPS = list.Sum(t => decimal.TryParse(t.JHJPS, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL GetTotalDataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL(List<DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL { No = "合计", LBMC = "合计", KYL = 0.ToString(), YJP = 0.ToString(), JJ = 0.ToString(), PS = 0.ToString() } : new DataBase3CX_GYMJ_03_PMCBJJYJLYB_ADL { No = "合计", LBMC = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0m).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0m).ToString(), JJ = list.Sum(t => decimal.TryParse(t.JJ, out decimal d) ? d : 0m).ToString(), PS = list.Sum(t => decimal.TryParse(t.PS, out decimal d) ? d : 0m).ToString() };
        }

        public static DataBase3CX_General_CXYB GetTotalDataBase3CX_General_CXYB(List<DataBase3CX_General_CXYB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_General_CXYB { No = "合计", CPBM = "合计", KYL = 0.ToString(), YJP = 0.ToString(), YJL = 0.ToString(), QX01 = 0.ToString(), QX02 = 0.ToString(), QX03 = 0.ToString(), QX04 = 0.ToString(), QX05 = 0.ToString(), QX06 = 0.ToString(), QX07 = 0.ToString(), QX08 = 0.ToString(), QX09 = 0.ToString(), QX10 = 0.ToString(), QX11 = 0.ToString(), QX12 = 0.ToString(), QX13 = 0.ToString(), QX14 = 0.ToString(), JJ = 0.ToString(), PS = 0.ToString() } : new DataBase3CX_General_CXYB { No = "合计", CPBM = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M).ToString(), YJL = list.Sum(t => decimal.TryParse(t.YJL, out decimal d) ? d : 0M).ToString(), QX01 = list.Sum(t => decimal.TryParse(t.QX01, out decimal d) ? d : 0M).ToString(), QX02 = list.Sum(t => decimal.TryParse(t.QX02, out decimal d) ? d : 0M).ToString(), QX03 = list.Sum(t => decimal.TryParse(t.QX03, out decimal d) ? d : 0M).ToString(), QX04 = list.Sum(t => decimal.TryParse(t.QX04, out decimal d) ? d : 0M).ToString(), QX05 = list.Sum(t => decimal.TryParse(t.QX05, out decimal d) ? d : 0M).ToString(), QX06 = list.Sum(t => decimal.TryParse(t.QX06, out decimal d) ? d : 0M).ToString(), QX07 = list.Sum(t => decimal.TryParse(t.QX07, out decimal d) ? d : 0M).ToString(), QX08 = list.Sum(t => decimal.TryParse(t.QX08, out decimal d) ? d : 0M).ToString(), QX09 = list.Sum(t => decimal.TryParse(t.QX09, out decimal d) ? d : 0M).ToString(), QX10 = list.Sum(t => decimal.TryParse(t.QX10, out decimal d) ? d : 0M).ToString(), QX11 = list.Sum(t => decimal.TryParse(t.QX11, out decimal d) ? d : 0M).ToString(), QX12 = list.Sum(t => decimal.TryParse(t.QX12, out decimal d) ? d : 0M).ToString(), QX13 = list.Sum(t => decimal.TryParse(t.QX13, out decimal d) ? d : 0M).ToString(), QX14 = list.Sum(t => decimal.TryParse(t.QX14, out decimal d) ? d : 0M).ToString(), JJ = list.Sum(t => decimal.TryParse(t.JJ, out decimal d) ? d : 0M).ToString(), PS = list.Sum(t => decimal.TryParse(t.PS, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_GYMJ_05_CJBG GetTotalDataBase3CX_GYMJ_05_CJBG(List<DataBase3CX_GYMJ_05_CJBG> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_GYMJ_05_CJBG { No = "合计", GH = "合计", ZJCS1 = 0.ToString(), ZJCS2 = 0.ToString(), ZJCS3 = 0.ToString(), ZJCS4 = 0.ToString(), MXS1 = 0.ToString(), MXS2 = 0.ToString(), MXS3 = 0.ToString(), MXS4 = 0.ToString(), YYJWJP = 0.ToString() } : new DataBase3CX_GYMJ_05_CJBG { No = "合计", GH = "合计", ZJCS1 = list.Sum(t => decimal.TryParse(t.ZJCS1, out decimal d) ? d : 0M).ToString(), ZJCS2 = list.Sum(t => decimal.TryParse(t.ZJCS2, out decimal d) ? d : 0M).ToString(), ZJCS3 = list.Sum(t => decimal.TryParse(t.ZJCS3, out decimal d) ? d : 0M).ToString(), ZJCS4 = list.Sum(t => decimal.TryParse(t.ZJCS4, out decimal d) ? d : 0M).ToString(), MXS1 = list.Sum(t => decimal.TryParse(t.MXS1, out decimal d) ? d : 0M).ToString(), MXS2 = list.Sum(t => decimal.TryParse(t.MXS2, out decimal d) ? d : 0M).ToString(), MXS3 = list.Sum(t => decimal.TryParse(t.MXS3, out decimal d) ? d : 0M).ToString(), MXS4 = list.Sum(t => decimal.TryParse(t.MXS4, out decimal d) ? d : 0M).ToString(), YYJWJP = list.Sum(t => decimal.TryParse(t.YYJWJP, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_GYMJ_07_BZFGY GetTotalDataBase3CX_GYMJ_07_BZFGY(List<DataBase3CX_GYMJ_07_BZFGY> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_GYMJ_07_BZFGY { No = "合计", BFZ = 0.ToString() } : new DataBase3CX_GYMJ_07_BZFGY { No = "合计", BFZ = list.Sum(t => decimal.TryParse(t.BFZ, out decimal d) ? d : 0m).ToString() };
        }

        public static DataBase3CX_General_JB GetTotalDataBase3CX_General_JB(List<DataBase3CX_General_JB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_General_JB { No = "合计", JBTS = 0.ToString(), Money = 0.ToString() } : new DataBase3CX_General_JB { No = "合计", JBTS = list.Sum(t => decimal.TryParse(t.JBTS, out decimal d) ? d : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_JSY_02MJJSYKH GetTotalDataBase3CX_JSY_02MJJSYKH(List<DataBase3CX_JSY_02MJJSYKH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_JSY_02MJJSYKH { No = "合计", PZ = "合计", GZKHJE = 0.ToString(), QXKHJE = 0.ToString(), Money = 0.ToString() } : new DataBase3CX_JSY_02MJJSYKH { No = "合计", PZ = "合计", GZKHJE = list.Sum(t => decimal.TryParse(t.GZKHJE, out decimal d) ? d : 0M).ToString(), QXKHJE = list.Sum(t => decimal.TryParse(t.QXKHJE, out decimal d) ? d : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_01JJKHTB GetTotalDataBase3CX_CX_01JJKHTB(List<DataBase3CX_CX_01JJKHTB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_01JJKHTB { No = "合计", GD = "合计", SGS = 0.ToString(), MXS = 0.ToString(), ZJCS = 0.ToString(), MXS2 = 0.ToString(), ZJCS2 = 0.ToString() } : new DataBase3CX_CX_01JJKHTB { No = "合计", GD = "合计", SGS = list.Sum(t => decimal.TryParse(t.SGS, out decimal d) ? d : 0M).ToString(), MXS = list.Sum(t => decimal.TryParse(t.MXS, out decimal d) ? d : 0M).ToString(), ZJCS = list.Sum(t => decimal.TryParse(t.ZJCS, out decimal d) ? d : 0M).ToString(), MXS2 = list.Sum(t => decimal.TryParse(t.MXS2, out decimal d) ? d : 0M).ToString(), ZJCS2 = list.Sum(t => decimal.TryParse(t.ZJCS2, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_01JJKHTB_Out GetTotalDataBase3CX_CX_01JJKHTB_Out(List<DataBase3CX_CX_01JJKHTB_Out> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_01JJKHTB_Out { No = "合计", BZBM = "合计", GH = 0.ToString() + "人" } : new DataBase3CX_CX_01JJKHTB_Out { No = "合计", BZBM = "合计", GH = list.Count.ToString() + "人" };
        }

        public static DataBase3CX_CX_02MXS GetTotalDataBase3CX_CX_02MXS(List<DataBase3CX_CX_02MXS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_02MXS { No = "合计", BZName = "合计", SGS = 0.ToString(), MXS = 0.ToString(), ZJCS = 0.ToString(), MXS2 = 0.ToString(), ZJCS2 = 0.ToString() } : new DataBase3CX_CX_02MXS { No = "合计", BZName = "合计", SGS = list.Sum(t => decimal.TryParse(t.SGS, out decimal d) ? d : 0M).ToString(), MXS = list.Sum(t => decimal.TryParse(t.MXS, out decimal d) ? d : 0M).ToString(), ZJCS = list.Sum(t => decimal.TryParse(t.ZJCS, out decimal d) ? d : 0M).ToString(), MXS2 = list.Sum(t => decimal.TryParse(t.MXS2, out decimal d) ? d : 0M).ToString(), ZJCS2 = list.Sum(t => decimal.TryParse(t.ZJCS2, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_03JJ GetTotalDataBase3CX_CX_03JJ(List<DataBase3CX_CX_03JJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_03JJ { No = "合计", BZName = "合计", KYL = 0.ToString(), YJP = 0.ToString(), PSS = 0.ToString(), JPS = 0.ToString(), DLSL = 0.ToString(), SGCS = 0.ToString(), MXS = 0.ToString(), ZJCS = 0.ToString(), MXS2 = 0.ToString(), ZJCS2 = 0.ToString(), CLKH = 0.ToString(), ZLKH = 0.ToString(), DJBDZH = 0.ToString(), SGJE = 0.ToString(), JJZE = 0.ToString(), GZZE = 0.ToString() } : new DataBase3CX_CX_03JJ { No = "合计", BZName = "合计", KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M).ToString(), PSS = list.Sum(t => decimal.TryParse(t.PSS, out decimal d) ? d : 0M).ToString(), JPS = list.Sum(t => decimal.TryParse(t.JPS, out decimal d) ? d : 0M).ToString(), DLSL = list.Sum(t => decimal.TryParse(t.DLSL, out decimal d) ? d : 0M).ToString(), SGCS = list.Sum(t => decimal.TryParse(t.SGCS, out decimal d) ? d : 0M).ToString(), MXS = list.Sum(t => decimal.TryParse(t.MXS, out decimal d) ? d : 0M).ToString(), ZJCS = list.Sum(t => decimal.TryParse(t.ZJCS, out decimal d) ? d : 0M).ToString(), MXS2 = list.Sum(t => decimal.TryParse(t.MXS2, out decimal d) ? d : 0M).ToString(), ZJCS2 = list.Sum(t => decimal.TryParse(t.ZJCS2, out decimal d) ? d : 0M).ToString(), CLKH = list.Sum(t => decimal.TryParse(t.CLKH, out decimal d) ? d : 0M).ToString(), ZLKH = list.Sum(t => decimal.TryParse(t.ZLKH, out decimal d) ? d : 0M).ToString(), DJBDZH = list.Sum(t => decimal.TryParse(t.DJBDZH, out decimal d) ? d : 0M).ToString(), SGJE = list.Sum(t => decimal.TryParse(t.SGJE, out decimal d) ? d : 0M).ToString(), JJZE = list.Sum(t => decimal.TryParse(t.JJZE, out decimal d) ? d : 0M).ToString(), GZZE = list.Sum(t => decimal.TryParse(t.GZZE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_03JJ_GR GetTotalDataBase3CX_CX_03JJ_GR(List<DataBase3CX_CX_03JJ_GR> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_03JJ_GR { No = "合计", BZBM = "合计", BZMC = "合计", SumMoney = 0.ToString(), Money = 0.ToString() } : new DataBase3CX_CX_03JJ_GR { No = "合计", BZBM = "合计", BZMC = "合计", SumMoney = list.Sum(t => decimal.TryParse(t.SumMoney, out decimal d) ? d : 0M).ToString(), Money = list.Sum(t => decimal.TryParse(t.Money, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_04SXBZ GetTotalDataBase3CX_CX_04SXBZ(List<DataBase3CX_CX_04SXBZ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_04SXBZ { No = "合计", GD = "合计", BZJE = 0.ToString() } : new DataBase3CX_CX_04SXBZ { No = "合计", GD = "合计", BZJE = list.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_05PXBZ GetTotalDataBase3CX_CX_05PXBZ(List<DataBase3CX_CX_05PXBZ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_05PXBZ { No = "合计", GD = "合计", BZJE = 0.ToString() } : new DataBase3CX_CX_05PXBZ { No = "合计", GD = "合计", BZJE = list.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_06ZXJ GetTotalDataBase3CX_CX_06ZXJ(List<DataBase3CX_CX_06ZXJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_06ZXJ { No = "合计", GW = "合计", SL = 0.ToString(), JE = 0.ToString() } : new DataBase3CX_CX_06ZXJ { No = "合计", GW = "合计", SL = list.Sum(t => decimal.TryParse(t.SL, out decimal d) ? d : 0M).ToString(), JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_07FLFK GetTotalDataBase3CX_CX_07FLFK(List<DataBase3CX_CX_07FLFK> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_07FLFK { No = "合计", GH = "合计", FLFK = 0.ToString() } : new DataBase3CX_CX_07FLFK { No = "合计", GH = "合计", FLFK = list.Sum(t => decimal.TryParse(t.FLFK, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_09SS GetTotalDataBase3CX_CX_09SS(List<DataBase3CX_CX_09SS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_09SS { No = "合计", CJ = "合计", SL = 0.ToString(), JE = 0.ToString() } : new DataBase3CX_CX_09SS { No = "合计", CJ = "合计", SL = list.Sum(t => decimal.TryParse(t.SL, out decimal d) ? d : 0M).ToString(), JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_11PSBZ GetTotalDataBase3CX_CX_11PSBZ(List<DataBase3CX_CX_11PSBZ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_11PSBZ { No = "合计", GH = "合计", HJJE = 0.ToString() } : new DataBase3CX_CX_11PSBZ { No = "合计", GH = "合计", HJJE = list.Sum(t => decimal.TryParse(t.HJJE, out decimal d) ? d : 0M).ToString() };
        }

        public static DataBase3CX_CX_12SFBZ GetTotalDataBase3CX_CX_12SFBZ(List<DataBase3CX_CX_12SFBZ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase3CX_CX_12SFBZ { No = "合计", SFGH = "合计", TDGH = "合计", TDJJ = 0.ToString(), SFBZ = 0.ToString() } : new DataBase3CX_CX_12SFBZ { No = "合计", SFGH = "合计", TDGH = "合计", TDJJ = list.Sum(t => decimal.TryParse(t.TDJJ, out decimal d) ? d : 0M).ToString(), SFBZ = list.Sum(t => decimal.TryParse(t.SFBZ, out decimal d) ? d : 0M).ToString() };
        }

        //public static DataBase3CX_BJLP_01_LPYB GetTotalDataBase3CX_BJLP_01_LPYB(List<DataBase3CX_BJLP_01_LPYB> list)
        //{
        //    return (list == null || list.Count == 0) ? new DataBase3CX_BJLP_01_LPYB { No = "合计", Factory = "合计", HJ = 0.ToString(), DL = 0.ToString(), JE = 0.ToString(), BPD = 0.ToString(), DTL = 0.ToString(), DQ = 0.ToString(), G = 0.ToString(), LTL = 0.ToString(), PL = 0.ToString(), SXL = 0.ToString(), XK = 0.ToString(), ZL = 0.ToString() } : new DataBase3CX_BJLP_01_LPYB { No = "合计", Factory = "合计", HJ = list.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M).ToString(), DL = list.Sum(t => decimal.TryParse(t.DL, out decimal d) ? d : 0M).ToString(), JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString(), BPD = list.Sum(t => decimal.TryParse(t.BPD, out decimal d) ? d : 0M).ToString(), DTL = list.Sum(t => decimal.TryParse(t.DTL, out decimal d) ? d : 0M).ToString(), DQ = list.Sum(t => decimal.TryParse(t.DQ, out decimal d) ? d : 0M).ToString(), G = list.Sum(t => decimal.TryParse(t.G, out decimal d) ? d : 0M).ToString(), LTL = list.Sum(t => decimal.TryParse(t.LTL, out decimal d) ? d : 0M).ToString(), PL = list.Sum(t => decimal.TryParse(t.PL, out decimal d) ? d : 0M).ToString(), SXL = list.Sum(t => decimal.TryParse(t.SXL, out decimal d) ? d : 0M).ToString(), XK = list.Sum(t => decimal.TryParse(t.XK, out decimal d) ? d : 0M).ToString(), ZL = list.Sum(t => decimal.TryParse(t.ZL, out decimal d) ? d : 0M).ToString() };
        //}

        /// <summary>
        /// 一厂仓储车间报合计 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1CC_CJB GetTotalDataBase1CC_CJB(List<DataBase1CC_CJB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1CC_CJB { No = "合计", GWMC = "合计", JJ = 0.ToString(), FHYZB = 0.ToString(), HJ = 0.ToString() } : new DataBase1CC_CJB { No = "合计", GWMC = "合计", JJ = list.Sum(t => decimal.TryParse(t.JJ, out decimal d) ? d : 0M).ToString(), FHYZB = list.Sum(t => decimal.TryParse(t.FHYZB, out decimal d) ? d : 0M).ToString(), HJ = list.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂仓储品管验货天数合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1CC_PGYH GetTotalDataBase1CC_PGYH(List<DataBase1CC_PGYH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1CC_PGYH { No = "合计", UserName = "合计", YHTS = 0.ToString() } : new DataBase1CC_PGYH { No = "合计", UserName = "合计", YHTS = list.Sum(t => decimal.TryParse(t.YHTS, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂学徒（月）合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBaseGeneral_XT GetTotalDataBaseGeneral_XT(List<DataBaseGeneral_XT> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_XT { No = "合计", GZ = "合计", BZJE = 0.ToString() } : new DataBaseGeneral_XT { No = "合计", GZ = "合计", BZJE = list.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂学徒（日）合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBaseGeneral_XTDay GetTotalDataBaseGeneral_XTDay(List<DataBaseGeneral_XTDay> list)
        {
            return (list == null || list.Count == 0) ? new DataBaseGeneral_XTDay { No = "合计", GZ = "合计", BZJE = 0.ToString() } : new DataBaseGeneral_XTDay { No = "合计", GZ = "合计", BZJE = list.Sum(t => decimal.TryParse(t.BZJE, out decimal d) ? d : 0M).ToString() };
        }
        /// <summary>
        /// 一厂原料统计合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1YL_YLTJ GetTotalDataBase1YL_YLTJ(List<DataBase1YL_YLTJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1YL_YLTJ { No = "合计", DW = "合计", SL = 0.ToString() } : new DataBase1YL_YLTJ { No = "合计", DW = "合计", SL = list.Sum(t => decimal.TryParse(t.SL, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂原料计件合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1YL_JJ GetTotalDataBase1YL_JJ(List<DataBase1YL_JJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1YL_JJ { No = "合计", LB = "合计", SL = 0.ToString(), JJJE = 0.ToString() } : new DataBase1YL_JJ { No = "合计", LB = "合计", SL = list.Sum(t => decimal.TryParse(t.SL, out decimal d) ? d : 0M).ToString(), JJJE = list.Sum(t => decimal.TryParse(t.JJJE, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂卸石膏计件合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1MJ_XSGJJ GetTotalDataBase1MJ_XSGJJ(List<DataBase1MJ_XSGJJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1MJ_XSGJJ { No = "合计", Money = 0.ToString(), DS = 0.ToString() } : new DataBase1MJ_XSGJJ { No = "合计", Money = list.Sum(t => decimal.TryParse(t.Money, out decimal result) ? result : 0).ToString(), DS = list.Sum(t => decimal.TryParse(t.DS, out decimal ds) ? ds : 0).ToString() };
        }

        /// <summary>
        /// 一厂日工提报合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1MJ_RGTB GetTotalDataBase1MJ_RGTB(List<DataBase1MJ_RGTB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1MJ_RGTB { No = "合计", TS = 0.ToString(), RGHJ = 0.ToString() } : new DataBase1MJ_RGTB { No = "合计", TS = list.Sum(t => decimal.TryParse(t.TS, out decimal result) ? result : 0).ToString(), RGHJ = list.Sum(t => decimal.TryParse(t.RGHJ, out decimal ds) ? ds : 0).ToString() };
        }

        /// <summary>
        /// 一厂大件月报合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1MJ_DJCJYB GetTotalDataBase1MJ_DJCJYB(List<DataBase1MJ_DJCJYB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1MJ_DJCJYB { No = "合计", SCLJ = 0.ToString(), BCLJ = 0.ToString(), PSLJ = 0.ToString() } : new DataBase1MJ_DJCJYB { No = "合计", SCLJ = list.Sum(t => decimal.TryParse(t.SCLJ, out decimal result) ? result : 0M).ToString(), BCLJ = list.Sum(t => decimal.TryParse(t.BCLJ, out decimal result) ? result : 0M).ToString(), PSLJ = list.Sum(t => decimal.TryParse(t.PSLJ, out decimal result) ? result : 0M).ToString() };
        }

        /// <summary>
        /// 一厂小件月报合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1MJ_XJCJYB GetTotalDataBase1MJ_XJCJYB(List<DataBase1MJ_XJCJYB> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1MJ_XJCJYB { No = "合计", SCSL = 0.ToString() } : new DataBase1MJ_XJCJYB { No = "合计", SCSL = list.Sum(t => decimal.TryParse(t.SCSL, out decimal result) ? result : 0M).ToString() };
        }

        /// <summary>
        /// 一厂模具运模计件金额合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1MJ_YMJJ GetTotalDataBase1MJ_YMJJ(List<DataBase1MJ_YMJJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1MJ_YMJJ { No = "合计", JE = 0.ToString() } : new DataBase1MJ_YMJJ { No = "合计", JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂PMC日合计
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DataBase1JB_XWRKHGP GetTotalDataBase1JB_XWRKHGP(List<DataBase1JB_XWRKHGP> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase1JB_XWRKHGP() { TypesName = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day };
            }
            return new DataBase1JB_XWRKHGP() { TypesName = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day, Unit = string.Empty, X1 = list.Sum(t => string.IsNullOrEmpty(t.X1) ? 0 : Convert.ToDecimal(t.X1)).ToString(), X2 = list.Sum(t => string.IsNullOrEmpty(t.X2) ? 0 : Convert.ToDecimal(t.X2)).ToString(), X3 = list.Sum(t => string.IsNullOrEmpty(t.X3) ? 0 : Convert.ToDecimal(t.X3)).ToString(), X4 = list.Sum(t => string.IsNullOrEmpty(t.X4) ? 0 : Convert.ToDecimal(t.X4)).ToString(), X5 = list.Sum(t => string.IsNullOrEmpty(t.X5) ? 0 : Convert.ToDecimal(t.X5)).ToString(), X6 = list.Sum(t => string.IsNullOrEmpty(t.X6) ? 0 : Convert.ToDecimal(t.X6)).ToString(), X7 = list.Sum(t => string.IsNullOrEmpty(t.X7) ? 0 : Convert.ToDecimal(t.X7)).ToString(), X8 = list.Sum(t => string.IsNullOrEmpty(t.X8) ? 0 : Convert.ToDecimal(t.X8)).ToString(), X9 = list.Sum(t => string.IsNullOrEmpty(t.X9) ? 0 : Convert.ToDecimal(t.X9)).ToString(), X10 = list.Sum(t => string.IsNullOrEmpty(t.X10) ? 0 : Convert.ToDecimal(t.X10)).ToString(), X11 = list.Sum(t => string.IsNullOrEmpty(t.X11) ? 0 : Convert.ToDecimal(t.X11)).ToString(), UnitPrice = "实际", L1 = list.Sum(t => string.IsNullOrEmpty(t.L1) ? 0 : Convert.ToDecimal(t.L1)).ToString(), L2 = list.Sum(t => string.IsNullOrEmpty(t.L2) ? 0 : Convert.ToDecimal(t.L2)).ToString(), L3 = list.Sum(t => string.IsNullOrEmpty(t.L3) ? 0 : Convert.ToDecimal(t.L3)).ToString(), L4 = list.Sum(t => string.IsNullOrEmpty(t.L4) ? 0 : Convert.ToDecimal(t.L4)).ToString(), L5 = list.Sum(t => string.IsNullOrEmpty(t.L5) ? 0 : Convert.ToDecimal(t.L5)).ToString(), L6 = list.Sum(t => string.IsNullOrEmpty(t.L6) ? 0 : Convert.ToDecimal(t.L6)).ToString(), L7 = list.Sum(t => string.IsNullOrEmpty(t.L7) ? 0 : Convert.ToDecimal(t.L7)).ToString(), L8 = list.Sum(t => string.IsNullOrEmpty(t.L8) ? 0 : Convert.ToDecimal(t.L8)).ToString(), L9 = list.Sum(t => string.IsNullOrEmpty(t.L9) ? 0 : Convert.ToDecimal(t.L9)).ToString(), L10 = list.Sum(t => string.IsNullOrEmpty(t.L10) ? 0 : Convert.ToDecimal(t.L10)).ToString(), L11 = list.Sum(t => string.IsNullOrEmpty(t.L11) ? 0 : Convert.ToDecimal(t.L11)).ToString() };
        }

        /// <summary>
        /// 一厂检包线内出勤日合计
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DataBase1JB_XWRYCQ GetTotalDataBase1JB_XWRYCQ(List<DataBase1JB_XWRYCQ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase1JB_XWRYCQ() { XW = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day };
            }
            return new DataBase1JB_XWRYCQ() { XW = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, TheDay = dateTime.Day, DGWGZ = list.Sum(t => string.IsNullOrEmpty(t.DGWGZ) ? 0 : Convert.ToDecimal(t.DGWGZ)).ToString(), DYSCQ = list.Sum(t => string.IsNullOrEmpty(t.DYSCQ) ? 0 : Convert.ToDecimal(t.DYSCQ)).ToString(), TBGZE = list.Sum(t => string.IsNullOrEmpty(t.TBGZE) ? 0 : Convert.ToDecimal(t.TBGZE)).ToString(), StudyDay = list.Sum(t => string.IsNullOrEmpty(t.StudyDay) ? 0M : Convert.ToDecimal(t.StudyDay)).ToString(), WorkDay = list.Sum(t => string.IsNullOrEmpty(t.WorkDay) ? 0M : Convert.ToDecimal(t.WorkDay)).ToString(), TotalGZ = list.Sum(t => string.IsNullOrEmpty(t.TotalGZ) ? 0M : Convert.ToDecimal(t.TotalGZ)).ToString(), TotalTBGZE = list.Sum(t => string.IsNullOrEmpty(t.TotalTBGZE) ? 0M : Convert.ToDecimal(t.TotalTBGZE)).ToString(), HJ = list.Sum(t => string.IsNullOrEmpty(t.HJ) ? 0M : Convert.ToDecimal(t.HJ)).ToString() };
        }

        /// <summary>
        /// 一厂检包线内定员合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1JB_XNDY GetTotalDataBase1JB_XNDY(List<DataBase1JB_XNDY> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1JB_XNDY { GW = "合计", SJBZ = 0.ToString() } : new DataBase1JB_XNDY { GW = "合计", SJBZ = list.Sum(t => decimal.TryParse(t.SJBZ, out decimal d) ? d : 0M).ToString()};
        }

        /// <summary>
        /// 一厂检包磨瓷冷补合计
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DataBase1JB_MCLBJJ GetTotalDataBase1JB_MCLBJJ(List<DataBase1JB_MCLBJJ> list, DateTime dateTime)
        {
            if (list == null || list.Count == 0)
            {
                return new DataBase1JB_MCLBJJ() { PZ = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month };
            }
            return new DataBase1JB_MCLBJJ() { PZ = "合计", TheYear = dateTime.Year, TheMonth = dateTime.Month, McCount = list.Sum(t => string.IsNullOrEmpty(t.McCount) ? 0 : Convert.ToDecimal(t.McCount)).ToString(), YkcpgCount = list.Sum(t => string.IsNullOrEmpty(t.YkcpgCount) ? 0 : Convert.ToDecimal(t.YkcpgCount)).ToString(), LbCount = list.Sum(t => string.IsNullOrEmpty(t.LbCount) ? 0 : Convert.ToDecimal(t.LbCount)).ToString(), McMoney = list.Sum(t => string.IsNullOrEmpty(t.McMoney) ? 0 : Convert.ToDecimal(t.McMoney)).ToString(), YkcpgMoney = list.Sum(t => string.IsNullOrEmpty(t.YkcpgMoney) ? 0 : Convert.ToDecimal(t.YkcpgMoney)).ToString(), LbMoney = list.Sum(t => string.IsNullOrEmpty(t.LbMoney) ? 0 : Convert.ToDecimal(t.LbMoney)).ToString(), Money = list.Sum(t => string.IsNullOrEmpty(t.Money) ? 0m : Convert.ToDecimal(t.Money)).ToString() };
        }

        /// <summary>
        /// 一厂检包开发试烧计件合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1JB_KFSS GetTotalDataBase1JB_KFSS(List<DataBase1JB_KFSS> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1JB_KFSS { No = "合计", LB = "合计", SL = 0.ToString(), JJJE = 0.ToString() } : new DataBase1JB_KFSS { No = "合计", LB = "合计", SL = list.Sum(t => decimal.TryParse(t.SL, out decimal d) ? d : 0M).ToString(), JJJE = list.Sum(t => decimal.TryParse(t.JJJE, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂烧成装窑计件和考核合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1SC_ZYJJHKH GetTotalDataBase1SC_ZYJJHKH(List<DataBase1SC_ZYJJHKH> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1SC_ZYJJHKH { No = "合计", Code = 0.ToString(), KYL = 0.ToString(), YJP = 0.ToString(), KH = 0.ToString(), JJ = 0.ToString(), HJ = 0.ToString() } : new DataBase1SC_ZYJJHKH { No = "合计", Code = list.Count.ToString(), KYL = list.Sum(t => decimal.TryParse(t.KYL, out decimal d) ? d : 0M).ToString(), YJP = list.Sum(t => decimal.TryParse(t.YJP, out decimal d) ? d : 0M).ToString(), KH = list.Sum(t => decimal.TryParse(t.KH, out decimal d) ? d : 0M).ToString(), JJ = list.Sum(t => decimal.TryParse(t.JJ, out decimal d) ? d : 0M).ToString(), HJ = list.Sum(t => decimal.TryParse(t.HJ, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂烧成车间其他计件明细合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1SC_QTJJ GetTotalDataBase1SC_QTJJ(List<DataBase1SC_QTJJ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1SC_QTJJ { No = "合计",JS=0.ToString(), JJJE = 0.ToString() } : new DataBase1SC_QTJJ { No = "合计", LB = "合计", JS = list.Sum(t => decimal.TryParse(t.JS, out decimal d) ? d : 0M).ToString(), JJJE = list.Sum(t => decimal.TryParse(t.JJJE, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 一厂烧成车间技术部实验补助合计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataBase1SC_JSBZ GetTotalDataBase1SC_JSBZ(List<DataBase1SC_JSBZ> list)
        {
            return (list == null || list.Count == 0) ? new DataBase1SC_JSBZ {  CJ= "合计", JS = 0.ToString(), JE = 0.ToString() } : new DataBase1SC_JSBZ { CJ = "合计", JS = list.Sum(t => decimal.TryParse(t.JS, out decimal d) ? d : 0M).ToString(), JE = list.Sum(t => decimal.TryParse(t.JE, out decimal d) ? d : 0M).ToString() };
        }

        /// <summary>
        /// 获得该月总共多少天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetMonthTotalDays(DateTime dateTime)
        {
            return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        }

        /// <summary>
        /// 计算指定日期的月工作日天数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWorkDays(DateTime dateTime)
        {
            DateTime dt = Convert.ToDateTime(dateTime.ToString("yyyy-MM-01"));
            int days = GetMonthTotalDays(dateTime);
            int weekDays = 0;
            for (int i = 0; i < days; i++)
            {
                // 判断是否为周六，周日，是则记录天数。
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        weekDays++;
                        break;

                    case DayOfWeek.Sunday:
                        weekDays++;
                        break;
                }
                dt = dt.AddDays(1);
            }
            return days - weekDays;
        }

        /// <summary>
        /// 计算指定日期，星期几的月总天数(日期的月的所有天数)
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="week">星期几</param>
        /// <returns></returns>
        public static int GetDaysByWeek(DateTime dateTime, DayOfWeek week)
        {
            DateTime dt = Convert.ToDateTime(dateTime.ToString("yyyy-MM-01"));
            int days = GetMonthTotalDays(dateTime);
            int weekDays = 0;
            for (int i = 0; i < days; i++)
            {
                if (dt.DayOfWeek == week)
                {
                    weekDays++;
                }
                dt = dt.AddDays(1);
            }
            return weekDays;
        }

        /// <summary>
        /// 日期到月底的剩余天数中，星期几有几天（包括日期的那一天）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static int GetDaysByTimeAndWeek(DateTime dateTime, DayOfWeek week)
        {
            DateTime dt = Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd"));
            int days = GetMonthTotalDays(dateTime) - dt.Day + 1;
            int weekDays = 0;
            for (int i = 0; i < days; i++)
            {
                if (dt.DayOfWeek == week)
                {
                    weekDays++;
                }
                dt = dt.AddDays(1);
            }
            return weekDays;
        }

        /// <summary>
        /// 三厂检包，验证出勤天数和（人员出勤日录入+叉车司机+辅助验货），验证失败时，将产生报警！！！
        /// </summary>
        /// <param name="userCode"></param>
        public static bool YZ3JB_CQ(string userCode, string userName, int year, int month, DateTime sysTime, string hrCode)
        {
            //var gz = new BaseDal<DataBaseGeneral_CQ>().Get(t => t.TheYear == year && t.TheMonth == month && t.UserCode == userCode && t.Dept == "检包车间" && (t.Position == "磨瓷工" || t.Position == "FQC"));
            //if (gz != null)
            //{
            //    return true;
            //}
            if (userCode == "M10846")
            {
            }

            var fzyh = new BaseDal<DataBase3JB_FZYH>().Get(t => t.TheYear == year && t.TheMonth == month);
            if (fzyh == null)
            {
                return true;
            }
            var user = fzyh.CreateUser;
            var day_XWRYCQ = new BaseDal<DataBase3JB_XWRYCQ>().GetList(t => t.TheYear == year && t.TheMonth == month && t.UserCode == userCode).ToList().Sum(t => (string.IsNullOrEmpty(t.WorkDay) ? 0m : Convert.ToDecimal(t.WorkDay)) + (string.IsNullOrEmpty(t.StudyDay) ? 0M : Convert.ToDecimal(t.StudyDay)));
            var day_XCWX = new BaseDal<DataBase3JB_XCSJ_WX>().GetList(t => t.TheYear == year && t.TheMonth == month && t.UserCode == userCode).ToList().Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays));
            var day_FZYH = new BaseDal<DataBase3JB_FZYH>().GetList(t => t.TheYear == year && t.TheMonth == month && t.UserCode == userCode && !t.IsXcOrWx).ToList().Sum(t => string.IsNullOrEmpty(t.SlgDays) ? 0M : Convert.ToDecimal(t.SlgDays));
            var day_SCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == year && t.TheMonth == month && t.UserCode == userCode && t.Dept == "检包车间").ToList().Sum(t => string.IsNullOrEmpty(t.DayTotal) ? 0M : Convert.ToDecimal(t.DayTotal));
            var day_YCQ = new BaseDal<DataBaseGeneral_CQ>().GetList(t => t.TheYear == year && t.TheMonth == month && t.UserCode == userCode && t.Dept == "检包车间").ToList().Sum(t => string.IsNullOrEmpty(t.DayYcq) ? 0M : Convert.ToDecimal(t.DayYcq));
            if (day_XWRYCQ + day_XCWX + day_FZYH <= 0)
            {
                return true;
            }
            int totalMonthDay = GetMonthTotalDays(new DateTime(year, month, 1));
            if (day_XWRYCQ + day_XCWX + day_FZYH > totalMonthDay)
            {
                BJ(user, sysTime, userCode, userName, year, month, day_XWRYCQ, day_XCWX, day_FZYH, totalMonthDay, hrCode);
            }
            if (day_SCQ == day_YCQ)
            {
                if (day_SCQ > day_XWRYCQ + day_XCWX + day_FZYH)
                {
                    //报警
                    BJ(user, sysTime, userCode, userName, year, month, day_YCQ, day_SCQ, day_XWRYCQ, day_XCWX, day_FZYH, hrCode);
                    return false;
                }
                else
                {
                    //通过
                    return true;
                }
            }
            else
            {
                if (day_SCQ >= day_XWRYCQ + day_XCWX + day_FZYH)
                {
                    //通过
                    return true;
                }
                else
                {
                    //报警
                    BJ(user, sysTime, userCode, userName, year, month, day_YCQ, day_SCQ, day_XWRYCQ, day_XCWX, day_FZYH, hrCode);
                    return false;
                }
            }
        }

        public static bool YZ3JB_XZF(string userCode, string userName, int year, int month, DateTime sysTime, decimal day_Xzf_Cq, decimal day_Cq_Scq, string hrCode)
        {
            var xzf = new BaseDal<DataBase3JB_XZF>().Get(t => t.TheYear == year && t.TheMonth == month);
            if (xzf == null)
            {
                return true;
            }
            if (day_Xzf_Cq > day_Cq_Scq)
            {
                //报警
                string uCode = xzf.CreateUser.Split('_')[0];
                var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = uCode, MsgTitle = "线长费出勤天数超过出勤的出勤合计天数", MsgClass = "线长费出勤天数超过出勤的出勤合计天数", Msg = string.Format("年：【{0}】，月：【{1}】，工号：【{2}】，姓名：【{3}】，线长费出勤天数：【{4}】，出勤中的出勤合计天数【{5}】。验证未通过！", year, month, userCode, userName, day_Xzf_Cq, day_Cq_Scq), IsDone = false, IsRead = false, CreateTime = sysTime, CreateUser = "系统报警" };
                new BaseDal<DataBaseMsg>().Add(msg);
                msg.ID = Guid.NewGuid();
                msg.UserCode = hrCode;
                new BaseDal<DataBaseMsg>().Add(msg);
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void BJ(string user, DateTime dateTime, string userCode, string userName, int year, int month, decimal day_XWRYCQ, decimal day_XCWX, decimal day_FZYH, int monthTotalDays, string hrcode)
        {
            string uCode = user.Split('_')[0];
            var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = uCode, MsgTitle = "出勤天数与检包导入天数错误", MsgClass = "出勤天数<人员出勤日录入+叉车外协+辅助验货", Msg = year.ToString() + "年，" + month + "月，工号:【" + userCode + "】，姓名【" + userName + "】，线位人员天数：" + day_XWRYCQ.ToString() + "天,叉车外协天数：" + day_XCWX.ToString() + "天，辅助验货天数：" + day_FZYH + "天大于本月自然天数：【" + monthTotalDays.ToString() + "】。验证未通过！", IsDone = false, IsRead = false, CreateTime = dateTime, CreateUser = "系统报警" };
            new BaseDal<DataBaseMsg>().Add(msg);
            msg.ID = Guid.NewGuid();
            msg.UserCode = hrcode;
            new BaseDal<DataBaseMsg>().Add(msg);
        }

        private static void BJ(string user, DateTime dateTime, string userCode, string userName, int year, int month, decimal day_YCQ, decimal day_SCQ, decimal day_XWRYCQ, decimal day_XCWX, decimal day_FZYH, string hrCode)
        {
            string uCode = user.Split('_')[0];
            var msg = new DataBaseMsg { ID = Guid.NewGuid(), UserCode = uCode, MsgTitle = "出勤天数与检包导入天数错误", MsgClass = "出勤天数<人员出勤日录入+叉车外协+辅助验货", Msg = year.ToString() + "年，" + month + "月，工号:【" + userCode + "】，姓名【" + userName + "】，出勤合计:" + day_SCQ.ToString() + "天，应出勤：" + day_YCQ + ",线位人员天数：" + day_XWRYCQ.ToString() + "天,叉车外协天数：" + day_XCWX.ToString() + "天，辅助验货天数：" + day_FZYH + "天。验证未通过！", IsDone = false, IsRead = false, CreateTime = dateTime, CreateUser = "系统报警" };
            new BaseDal<DataBaseMsg>().Add(msg);
            msg.ID = Guid.NewGuid();
            msg.UserCode = hrCode;
            new BaseDal<DataBaseMsg>().Add(msg);
        }

        public static bool IsUserCodeAndNameOK(string userCode, string userName, out string userName_ERP)
        {
            userName_ERP = string.Empty;
            if (string.IsNullOrEmpty(userCode))
            {
                return false;
            }

            if (userCode.Length == 6 && userCode.Substring(0, 1).ToUpper() == "M")
            {
                string sql = "SELECT cPsn_Name FROM hr_hi_person where cPsn_Num=@usercode";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ERPContext"].ConnectionString))
                {
                    object o = conn.ExecuteScalar(sql, new { usercode = userCode });
                    if (o != null)
                    {
                        userName_ERP = o.ToString();
                    }
                }
                if (string.IsNullOrEmpty(userName_ERP))
                {
                    return false;
                }
                else
                {
                    if (userName == userName_ERP)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                var listMonth = new BaseDal<DataBaseMonth>().Get(t => t.WorkshopName == "共用" && t.Classification == "人员编码" && t.ProductType == userCode);
                if (listMonth == null)
                {
                    return false;
                }
                else
                {
                    userName_ERP = listMonth.PostName;
                    if (listMonth.PostName == userName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsCanLogin(string userCode, string pwd, out BaseUser baseUser)
        {
            baseUser = null;
            bool result = false;
            if (userCode == "Admin" && pwd == "ADMIN")
            {
                baseUser = new BaseUser() { Code = userCode, Name = "超级管理员", DepName = "信息中心" };
                return true;
            }
            else
            {
                string sql = @"SELECT top 1 [user_id]
                                ,[user_show_id]
                                ,[user_name]
                                ,[password]
                                ,a.dept_id
                                ,b.dept_name
                          FROM [user_user] a
                          left join hr_dept b on a.dept_id=b.hr_dept_id and b.del_flag=0
                          where a.active_flag=1 and a.user_show_id='{0}'";
                sql = string.Format(sql, userCode);
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OAContext"].ConnectionString))
                {
                    dynamic tmp = conn.Query(sql).FirstOrDefault();
                    if (tmp != null)
                    {
                        if (tmp.password == StringHelper.MD5String(pwd))
                        {
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HHContext"].ConnectionString))
                            {
                                string mySql = "select * from BaseUsers where Code='" + userCode + "'";
                                baseUser = con.Query<BaseUser>(mySql).FirstOrDefault();
                                if (baseUser == null)
                                {
                                    //con.Execute("insert into BaseUsers (id,code,name,depid,depname,remark,lasttime,createtime,createUser) Values (newid(),@code,@name,@depid,@depname,@remark,getdate(),getdate(),@code)", new { code = userCode, name = tmp.user_name, depid = tmp.dept_id, depname = tmp.dept_name, remark = pwd });
                                    return false;
                                }
                                else
                                {
                                    con.Execute("update BaseUsers set lastTime=getdate() where code=@code", new { code = userCode });
                                    baseUser = con.Query<BaseUser>(mySql).FirstOrDefault();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}