using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;

namespace ServiceLayer.Service.Interface
{
    public interface IUser
    {
        //Get All User
        List<User> GetAllUserRepo();
        // Get Single User
        User GetSingleUser(int id); 
        // Add User
        String AddUserRepo(User user);
        //Edit or Update User
        String UpdateUserRepo(User user);

        //Delete or Remove User
        string DeleteUserRepo(int id);
    }
}
