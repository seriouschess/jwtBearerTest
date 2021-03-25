using Microsoft.AspNetCore.Mvc;

namespace jwtBearerTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        public MainController(){}

        [HttpGet]
        [Route("authenticate")]
        public string Authenticate()
        {
            return "success";
        }
    }
}
