using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Interface;

namespace ServiceLayer.Service.Implementation
{
    public class CredentialServices :ICredential
    {
        private readonly AppDbContext _dbContext;

        public CredentialServices(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public List<_credentials> GetAllCredentialRepo()
        {
            var dataLogin = _dbContext.tblflightlogin.ToList();
            return dataLogin;
            //return this._dbContext.tblflightlogin.FirstOrDefault();
        }
    }
}
