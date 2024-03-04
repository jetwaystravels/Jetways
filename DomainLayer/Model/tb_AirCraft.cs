using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class tb_AirCraft
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("tb_booking")]
        public int AirlineID { get; set; }
        public string AirCraftName { get; set; }
        public string AirCraftDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Createdby { get; set; }
        public DateTime Modifieddate { get; set; }
        public string Modifyby { get; set; }
        public string Status { get; set; }
    }
}
