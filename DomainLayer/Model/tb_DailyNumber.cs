using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_DailyNumber
    {

        [Key]

        //public DateTime DateGenerated { get; set; }

        //   public int? LastGeneratedNumber { get; set; }

        [StringLength(50)]
        public string Autogenratednumber { get; set; }
    }
}
