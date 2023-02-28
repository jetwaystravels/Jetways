using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class Login
    {
        [Key]
        public string login { get; set; }
        public string password { get; set; }
    }
}
