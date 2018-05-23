using System;

namespace Monopy.PreceRateWage.Model
{
    public class GZD
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public bool IsCheckCQDayOk { get; set; }

        /// <summary>
        /// 停发工资
        /// </summary>
        public bool IsTF { get; set; }

        /// <summary>
        /// 是否过程数据。true最终数据，false过程数据
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 计算方式:每次计算时需要判断是否已经计算过了，区分每个车间的工资计算方式是否已经计算。
        /// </summary>
        public string CalculationType { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 厂区
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string GD { get; set; }

        /// <summary>
        /// 工号（短工号）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 人员类别
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// OA工号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 弹性假天数
        /// </summary>
        public string DayTxj { get; set; }

        /// <summary>
        /// 加班出勤天数（检包-->带薪假天数）
        /// </summary>
        public string DayJBCQ { get; set; }

        /// <summary>
        /// 实出勤天数
        /// </summary>
        public string DaySCQ { get; set; }

        /// <summary>
        /// 应出勤
        /// </summary>
        public string DayYCQ { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string JiBenGZ { get; set; }

        /// <summary>
        /// 技能工资
        /// </summary>
        public string JiNengGZ { get; set; }

        /// <summary>
        /// 加班工资
        /// </summary>
        public string JiaBanGZ { get; set; }

        /// <summary>
        /// 工龄工资
        /// </summary>
        public string GLGZ { get; set; }

        /// <summary>
        /// 基本工资额
        /// </summary>
        public string JBGZE { get; set; }

        /// <summary>
        /// 工龄额
        /// </summary>
        public string GLE { get; set; }

        /// <summary>
        /// 技能工资额
        /// </summary>
        public string JNGZE { get; set; }

        /// <summary>
        /// 加班工资额1
        /// </summary>
        public string JBGZE1 { get; set; }

        /// <summary>
        /// 加班工资额2
        /// </summary>
        public string JBGZE2 { get; set; }

        /// <summary>
        /// 全勤奖
        /// </summary>
        public string QQJ { get; set; }

        /// <summary>
        /// 日工
        /// </summary>
        public string RG { get; set; }

        /// <summary>
        /// 弹性假补助
        /// </summary>
        public string TXJBZ { get; set; }

        /// <summary>
        /// 补发工资
        /// </summary>
        public string BFGZ { get; set; }

        /// <summary>
        /// 师傅计件
        /// </summary>
        public string SFJJ { get; set; }

        /// <summary>
        /// 计件工资1
        /// </summary>
        public string JJGZ1 { get; set; }

        /// <summary>
        /// 计件工资2
        /// </summary>
        public string JJGZ2 { get; set; }

        /// <summary>
        /// 计件工资3
        /// </summary>
        public string JJGZ3 { get; set; }

        /// <summary>
        /// 产量考核工资
        /// </summary>
        public string CLKHGZ { get; set; }

        /// <summary>
        /// 质量考核工资
        /// </summary>
        public string ZLKHGZ { get; set; }

        /// <summary>
        /// 个人考核工资
        /// </summary>
        public string GRKHGZ { get; set; }

        /// <summary>
        /// 班组考核工资
        /// </summary>
        public string BZKHGZ { get; set; }

        /// <summary>
        /// 品管验货
        /// </summary>
        public string PGYH { get; set; }

        /// <summary>
        /// 替班工资
        /// </summary>
        public string TBGZ { get; set; }

        /// <summary>
        /// 班长费（线长费）
        /// </summary>
        public string BZF { get; set; }

        /// <summary>
        /// 入职补助
        /// </summary>
        public string RZBZ { get; set; }

        /// <summary>
        /// 变产补助
        /// </summary>
        public string BCBZ { get; set; }

        /// <summary>
        /// 返坯奖罚
        /// </summary>
        public string FPJF { get; set; }

        /// <summary>
        /// 开发试烧补助
        /// </summary>
        public string KFSSBZ { get; set; }

        /// <summary>
        /// 工厂试烧补助
        /// </summary>
        public string GCSSBZ { get; set; }

        /// <summary>
        /// 退模补助
        /// </summary>
        public string TMBZ { get; set; }

        /// <summary>
        /// 计件补贴
        /// </summary>
        public string JJBT { get; set; }

        /// <summary>
        /// 交通补助
        /// </summary>
        public string JTBZ { get; set; }

        /// <summary>
        /// 租房补助
        /// </summary>
        public string ZFBZ { get; set; }

        /// <summary>
        /// 幼儿园补助
        /// </summary>
        public string YEYBZ { get; set; }

        /// <summary>
        /// 电话补贴
        /// </summary>
        public string DHBT { get; set; }

        /// <summary>
        /// 体检费报销
        /// </summary>
        public string TJFBX { get; set; }

        /// <summary>
        /// 介绍费
        /// </summary>
        public string JSF { get; set; }

        /// <summary>
        /// 车票报销
        /// </summary>
        public string CPBX { get; set; }

        /// <summary>
        /// 五星补助
        /// </summary>
        public string WXBZ { get; set; }

        /// <summary>
        /// 车间奖励
        /// </summary>
        public string JL { get; set; }

        /// <summary>
        /// 车间惩戒
        /// </summary>
        public string CJ { get; set; }

        /// <summary>
        /// 成型辅料奖罚
        /// </summary>
        public string CXFLJF { get; set; }

        /// <summary>
        /// 破损补助
        /// </summary>
        public string PSBZ { get; set; }

        /// <summary>
        /// 破损罚款
        /// </summary>
        public string PSFK { get; set; }

        /// <summary>
        /// 成检罚款
        /// </summary>
        public string CJFK { get; set; }

        /// <summary>
        /// 品管罚款
        /// </summary>
        public string PGFK { get; set; }

        /// <summary>
        /// 雇主责任险扣款
        /// </summary>
        public string GZZRXKK { get; set; }

        /// <summary>
        /// 打卡/离职扣款
        /// </summary>
        public string DK_LZKK { get; set; }

        /// <summary>
        /// 公司奖励
        /// </summary>
        public string GSJL { get; set; }

        /// <summary>
        /// 公司惩戒
        /// </summary>
        public string GSCJ { get; set; }

        /// <summary>
        /// 应发合计
        /// </summary>
        public string YFHJ { get; set; }

        /// <summary>
        /// 工会互助金
        /// </summary>
        public string GHHZJ { get; set; }

        /// <summary>
        /// 代扣公积金
        /// </summary>
        public string DKGJJ { get; set; }

        /// <summary>
        /// 代扣保险费
        /// </summary>
        public string DKBXF { get; set; }

        /// <summary>
        /// 个税
        /// </summary>
        public string GS { get; set; }

        /// <summary>
        /// 税后扣款
        /// </summary>
        public string SHKK { get; set; }

        /// <summary>
        /// 实发合计
        /// </summary>
        public string SFHJ { get; set; }
    }
}