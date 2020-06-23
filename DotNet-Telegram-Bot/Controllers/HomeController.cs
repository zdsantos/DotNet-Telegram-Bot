using Microsoft.AspNetCore.Mvc;

namespace DotNetTelegramBot.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Ok!");
        }
    }
}
