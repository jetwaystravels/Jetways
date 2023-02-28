using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class User
    {
        [Key] 
        public int UserId { get; set; }
        public string UserName { get; set; }
        public String UserPhone { get; set; }
        public string UserEmailId { get; set; }
        public string UserAddress { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
