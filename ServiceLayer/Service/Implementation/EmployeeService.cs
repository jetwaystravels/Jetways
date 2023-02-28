using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Interface;

namespace ServiceLayer.Service.Implementation
{
    public class EmployeeService:IEmployee
    {
        private readonly AppDbContext _dbContext;

        public EmployeeService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public string AddEmployeeRepo(Employee employee)
        {
            try
            {
                this._dbContext.Add(employee);
                this._dbContext.SaveChanges();
                return "Successfully Get All Employee";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteEmployeeRepo(int id)
        {
            try
            {
                var employee = this._dbContext.TblEmployee.Find(id);
                if (employee != null)
                {
                    this._dbContext.TblEmployee.Remove(employee);
                    this._dbContext.SaveChanges();
                    return "Employee Successfully Removed";
                }
                else
                    return "No Record Found";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public List<Employee> GetAllEmployeeRepo()
        {
            return this._dbContext.TblEmployee.ToList();
        }

        public Employee GetSingleEmployee(int id)
        {
            //return this._dbContext.TblEmployee.Where(x => x.UserId == id).FirstOrDefault();
            return this._dbContext.TblEmployee.Find(id);// only when id is primary key
        }

        public string UpdateEmployeeRepo(Employee employee)
        {
            try
            {
                var empValue = this._dbContext.TblEmployee.Find(employee.EmployeeId);
                if (empValue != null)
                {
                    empValue.EmployeeName = employee.EmployeeName;
                    this._dbContext.SaveChanges();
                    return "Successfully updated";
                }
                else
                    return "No Record Found";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
