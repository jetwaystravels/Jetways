using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class Admin
    {
        [Key]
        public int admin_id { get; set; }
        public string admin_name { get; set; }
        public string admin_email { get; set; }
        public string admin_password { get; set; }
        public string admin_image { get; set; }
    }
}
