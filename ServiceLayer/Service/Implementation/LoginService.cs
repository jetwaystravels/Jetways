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

    
    public class LoginService:Ilogin
    {
        private readonly AppDbContext _dbContext;

        public LoginService(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public List<Login> GetAllLoginRepo()
        {
            return this._dbContext.TblLogin.ToList();
        }
    }
}
