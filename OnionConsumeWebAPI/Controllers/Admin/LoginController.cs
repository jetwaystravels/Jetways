using System.Net.Http;
using System.Text;
using System.Text.Json;
using DomainLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnionConsumeWebAPI.Extensions;

namespace OnionConsumeWebAPI.Controllers.Admin
{
   
    public class LoginController : Controller
    {
              
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> UserLogin()
        {
			
			return View();
		}

         [HttpPost]
        public async Task<IActionResult> UserLogin(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                var loginRequest = new { Username = username, Password = password };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5225/api/Admin/LogIn", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonResult = JObject.Parse(result);

                    //var token = jsonResult["token"].ToString();
                    //var adminUsername = jsonResult["username"].ToString();

					//Store the token in session or cookie
					//HttpContext.Session.SetString("JwtToken", adminUsername);
					
					return Redirect("http://localhost:5202/");
                }

                ViewBag.ErrorMessage = "Invalid login credentials";
                return View();
            }



        }

        public IActionResult Dashboard()
        {
            return View();
        }
      
        public IActionResult Corporate()
        {
			return View();
        }


    }
}
