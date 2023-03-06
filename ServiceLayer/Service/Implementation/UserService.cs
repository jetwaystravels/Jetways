using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Interface;

namespace ServiceLayer.Service.Implementation
{
    public class UserService : IUser
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }    
        //Get All User
        public string AddUserRepo(User user)
        {
            try
            {
                this._dbContext.Add(user);
                this._dbContext.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            
        }

        public string DeleteUserRepo(int id)
        {
            try
            {
                var user =this._dbContext.TblUsers.Find(id);
                if (user != null) 
                { 
                    this._dbContext.TblUsers.Remove(user);
                    this._dbContext.SaveChanges();
                    return "User Successfully Removed";
                }
                else
                return "No Record Found";
            }
            catch (Exception ex)
            {
                return ex.Message;  
            }
        }

        public List<User> GetAllUserRepo()
        {
            //return this._dbContext.TblUsers.ToList();
            return this._dbContext.TblUsers.FromSqlRaw<User>("spGetAllUser").ToList();
        }

        public User GetSingleUser(int id)
        {
            //return this._dbContext.TblUsers.Where(x => x.UserId == id).FirstOrDefault();
            return this._dbContext.TblUsers.Find(id);// only when id is primary key
            
        }

        public string UpdateUserRepo(User user)
        {
            try
            {
                var userValue = this._dbContext.TblUsers.Find(user.UserId);
                if (userValue != null) 
                {
                    userValue.UserName= user.UserName;
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
