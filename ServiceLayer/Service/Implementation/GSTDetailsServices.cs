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
    public class GSTDetailsServices : IGSTDetails
    {
        private readonly AppDbContext _dbContext;
        public GSTDetailsServices(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public string PostGSTDetailsRepo(GSTDetails details)
        {
            try
            {
                this._dbContext.Add(details);
                this._dbContext.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
