using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class Employee
    {
        [Key]
        //employee id
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeDept { get; set; }
        public String EmployeeMob { get; set; }
        public string EmployeeAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
