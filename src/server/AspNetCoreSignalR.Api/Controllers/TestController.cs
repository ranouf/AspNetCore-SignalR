using AspNetCoreSignalR.Api.Hubs.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSignalR.Api.Controllers
{
    public class TestController : Controller
    {
        [HttpPost]
        public void asdasd([FromBody]UsersDto dto)
        {

        }
    }
}
