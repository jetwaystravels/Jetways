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
    public class CityService : ICity
    {
        private readonly AppDbContext _dbContext;

        public CityService(AppDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }    
        public List<City> GetAllCityRepo()
        {
            return this._dbContext.tblCityMaster.FromSqlRaw<City>("spGetAllCity").ToList();
        }
    }
}
