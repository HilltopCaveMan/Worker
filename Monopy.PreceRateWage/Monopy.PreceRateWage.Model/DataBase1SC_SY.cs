using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 实验
    /// </summary>
    public class DataBase1SC_SY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int TheType { get; set; }

        public string No { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string CPMC { get; set; }

        /// <summary>
        /// 空白
        /// </summary>
        public string KB { get; set; }

        /// <summary>
        /// 开窑量
        /// </summary>
        public string KYL { get; set; }

        /// <summary>
        /// 产品等级——合格
        /// </summary>
        public string CPDJ_HG { get; set; }

        /// <summary>
        /// 产品等级——等外
        /// </summary>
        public string CPDJ_DW { get; set; }

        /// <summary>
        /// 产品等级——残
        /// </summary>
        public string CPDJ_C { get; set; }

        /// <summary>
        /// 产品等级——冷补修
        /// </summary>
        public string CPDJ_LBX { get; set; }

        /// <summary>
        /// 产品等级——回烧
        /// </summary>
        public string CPDJ_HS { get; set; }

        /// <summary>
        /// 产品等级——磨
        /// </summary>
        public string CPDJ_M { get; set; }

        /// <summary>
        /// 产品等级——磨修
        /// </summary>
        public string CPDJ_MX { get; set; }

        /// <summary>
        /// 产品等级——合格率
        /// </summary>
        public string CPDJ_HGL { get; set; }

        /// <summary>
        /// 原料缺陷——铜脏
        /// </summary>
        public string YLQX_01 { get; set; }

        /// <summary>
        /// 原料缺陷——铁脏
        /// </summary>
        public string YLQX_02 { get; set; }

        public string YLQX_03 { get; set; }
        public string YLQX_XJ { get; set; }
        public string YLQX_QXL { get; set; }
        public string CXQX_01 { get; set; }
        public string CXQX_02 { get; set; }
        public string CXQX_03 { get; set; }
        public string CXQX_04 { get; set; }
        public string CXQX_05 { get; set; }
        public string CXQX_06 { get; set; }
        public string CXQX_07 { get; set; }
        public string CXQX_08 { get; set; }
        public string CXQX_09 { get; set; }
        public string CXQX_10 { get; set; }
        public string CXQX_11 { get; set; }
        public string CXQX_12 { get; set; }
        public string CXQX_13 { get; set; }
        public string CXQX_14 { get; set; }
        public string CXQX_XJ { get; set; }
        public string CXQX_QXL { get; set; }
        public string XDB { get; set; }
        public string XJ_1 { get; set; }
        public string XJ_2 { get; set; }
        public string XJ_3 { get; set; }
        public string XJ_4 { get; set; }
        public string XJ_5 { get; set; }
        public string XJ_6 { get; set; }
        public string XJ_7 { get; set; }
        public string XJ_8 { get; set; }
        public string XJ_XJ { get; set; }
        public string XJ_QXL { get; set; }
        public string PYQX_01 { get; set; }
        public string PYQX_02 { get; set; }
        public string PYQX_03 { get; set; }
        public string PYQX_04 { get; set; }
        public string PYQX_05 { get; set; }
        public string PYQX_06 { get; set; }
        public string PYQX_07 { get; set; }
        public string PYQX_08 { get; set; }
        public string PYQX_09 { get; set; }
        public string PYQX_10 { get; set; }
        public string PYQX_11 { get; set; }
        public string PYQX_XJ { get; set; }
        public string PYQX_QXL { get; set; }
        public string HSQX_01 { get; set; }
        public string HSQX_02 { get; set; }
        public string HSQX_03 { get; set; }
        public string HSQX_XJ { get; set; }
        public string HSQX_QXL { get; set; }
        public string ZYQX_01 { get; set; }
        public string ZYQX_02 { get; set; }
        public string ZYQX_03 { get; set; }
        public string ZYQX_04 { get; set; }
        public string ZYQX_05 { get; set; }
        public string ZYQX_XJ { get; set; }
        public string ZYQX_QXL { get; set; }
        public string KYQX_01 { get; set; }
        public string KYQX_02 { get; set; }
        public string KYQX_03 { get; set; }
        public string KYQX_QXL { get; set; }
        public string SYQX_01 { get; set; }
        public string SYQX_02 { get; set; }
        public string SYQX_03 { get; set; }
        public string SYQX_04 { get; set; }
        public string SYQX_05 { get; set; }
        public string SYQX_06 { get; set; }
        public string SYQX_07 { get; set; }
        public string SYQX_XJ { get; set; }
        public string SYQX_QXL { get; set; }
        public string BW { get; set; }
        public string SCQXL { get; set; }
        public string CZ { get; set; }
        public string CZ_QXL { get; set; }
        public string BZ { get; set; }
        public string BZ_QXL { get; set; }
    }
}