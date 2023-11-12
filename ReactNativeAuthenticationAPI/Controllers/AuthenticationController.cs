using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReactNativeAuthenticationAPI.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        [Route("/Login")]
        [HttpPost]
        public Task<bool> Post([FromBody] Credentials credentials)
        {
            if(credentials.username == "test" && credentials.password == "test")
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
       
    }
}
