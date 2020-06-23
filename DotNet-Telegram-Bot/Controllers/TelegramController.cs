using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using DotNetTelegramBot.Services;

namespace DotNetTelegramBot.Controllers
{
    [Route("api/webhook")]
    public class TelegramController : Controller
    {
        private readonly TelegramService _telegramService;

        public TelegramController(TelegramService telegram)
        {
            _telegramService = telegram;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            await _telegramService.EchoAsync(update);
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _telegramService.SetWebHook();
            return Ok();
        }
    }
}
