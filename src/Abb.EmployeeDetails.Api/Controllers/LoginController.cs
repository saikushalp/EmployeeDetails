using Abb.EmployeeDetails.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Abb.EmployeeDetails.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("token")]
        public async Task<IActionResult> GetToken()
        {
            var tokenDetails = _configuration.GetSection("AuthToken").Get<AuthToken>();
            var content = new StringContent(JsonConvert.SerializeObject(tokenDetails), 
                Encoding.UTF8, "application/json");
            using (HttpClient client =  new HttpClient())
            {
                client.BaseAddress = new Uri(tokenDetails.EndPoint);                
                var httpResponseMessage = await client.PostAsync("/oauth/token", content);
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                return Ok(result);
            }                      
        }
    }
}