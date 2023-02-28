using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;

namespace ServiceLayer.Service.Interface
{
    public interface IEmployee
    {
        //Get All Employee
        List<Employee> GetAllEmployeeRepo();
        // Get Single Employee
        Employee GetSingleEmployee(int id);
        // Add Employee
        String AddEmployeeRepo(Employee employee);
        //Edit or Update Employee
        String UpdateEmployeeRepo(Employee employee);

        //Delete or Remove Employee
        string DeleteEmployeeRepo(int id);
    }
}
