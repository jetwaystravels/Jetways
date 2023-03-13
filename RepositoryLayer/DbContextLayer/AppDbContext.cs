using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.DbContextLayer
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions con) : base(con)
        {

        }
        public DbSet<User> TblUsers { get; set; }  
        public DbSet<Employee> TblEmployee { get;set; }
        public DbSet<Login> TblLogin { get; set; }

        public DbSet<City> tblCityMaster { get; set; }

    }
}
