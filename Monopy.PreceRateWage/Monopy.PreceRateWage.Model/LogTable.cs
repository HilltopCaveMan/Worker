using System;

namespace Monopy.PreceRateWage.Model
{
    public class LogTable
    {
        public Guid Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string LogType { get; set; }
        public string LogInfo { get; set; }
    }
}