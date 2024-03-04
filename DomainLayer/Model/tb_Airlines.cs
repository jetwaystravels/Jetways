using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_Airlines
    {
        
        public int Id { get; set; }
        //[ForeignKey("tb_booking")]
        [Key]
        public int AirlineID { get; set; }
        public string AirlneName { get; set; }
        public string AirlineDescription { get; set; }
        public DateTime CreatedDate { get; set; }
       // public string CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime Modifieddate { get; set; }
        //public string Modifieddate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }
    }
}
