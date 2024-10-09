using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Interface;

namespace ServiceLayer.Service.Implementation
{
    public class AdminService : IAdmin
    {
        private readonly AppDbContext _context;
        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public Admin Login(string username, string password)
        {
            // Implement password hashing if needed
            return _context.tb_admin.FirstOrDefault(a => a.admin_email == username && a.admin_password == password);
        }





    }
}
