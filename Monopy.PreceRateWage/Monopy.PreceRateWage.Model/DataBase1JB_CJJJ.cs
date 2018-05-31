using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monopy.PreceRateWage.Model
{
    public class DataBase1JB_CJJJ
    {
        public DataBase1JB_CJJJ()
        {
            Childs = new HashSet<DataBase1JB_CJJJ_Child>();
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public int TheYear { get; set; }
        public int TheMonth { get; set; }
        public string FactoryNo { get; set; }
        public string Code { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<DataBase1JB_CJJJ_Child> Childs { get; set; }
        public virtual ICollection<DataBase1JB_CJJJ_Result> Results { get; set; }
    }
}