using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;

namespace OnionArchitectureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            this._user = user;
        }

        // Get All User
        [HttpGet]
         [Route("getall")]
        public IActionResult GetAllUserRecords() 
        {
            var response=this._user.GetAllUserRepo();
            return Ok(response);    
        }
        // Get Single user
        [HttpGet]
        [Route("getSingleUser")]
        public IActionResult GetSingleUser(int id)
        {
            var response = this._user.GetSingleUser(id);
            return Ok(response);
        }
        // Add new User
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(User user)
        {
            var response = this._user.AddUserRepo(user);
            return Ok(response);
        }
        // remove user
        [HttpDelete("removeUser")]
        public IActionResult DeleteUser(int id)
        {
            var response = this._user.DeleteUserRepo(id);
            return Ok(response);
        }
        // update user
        [HttpPut("Edit")]
        public IActionResult UpdateUser(User user)
        {
            var response = this._user.UpdateUserRepo(user);
            return Ok(response);
        }
    }
}
