using System;

namespace Monopy.PreceRateWage.Model
{
    /// <summary>
    /// 三工厂XXXX年XX月成型车间高压面具班长费表
    /// </summary>
    public class DataBase3CX_GYMJ_07_BZFGY
    {
        public Guid Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }

        /// <summary>
        /// 高压面具;高压水箱等。。。
        /// </summary>
        public string TheType { get; set; }

        public string No { get; set; }
        public string GH { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string BFZ { get; set; }
    }
}