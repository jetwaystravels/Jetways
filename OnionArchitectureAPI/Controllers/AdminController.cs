using Azure;
using DomainLayer.Model;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Interface;
using OnionArchitectureAPI.Models;

namespace OnionArchitectureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;

        public AdminController(IAdmin admin)
        {
            this._admin = admin;
        }


        [HttpGet("LogIn")]
        public IActionResult Login()
        {
            return View();
        }
       
       
        //[Route("LogIn")]
        [HttpPost("LogIn")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            var admin = _admin.Login(loginRequest.Username, loginRequest.Password);

            if (admin != null)
            {
                // Optionally set session or token here
                return Ok(new { message = "Login successful", username = admin.admin_name });
            }
            else
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }
        }

       




        //public class LoginRequest
        //{
        //    public string Username { get; set; }
        //    public string Password { get; set; }
        //}

    }
}
