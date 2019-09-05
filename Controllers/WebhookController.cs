using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoTelegram.Data.Work;
using CryptoTelegram.Dtos;
using CryptoTelegram.Dtos.Markets.Binance;
using CryptoTelegram.Data.Interface;
using System;

namespace CryptoTelegram.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public const string Binance24hrUrl = "https://api.binance.com/api/v1/ticker/24hr?symbol=pair";
        private readonly ILogger<WebhookController> _logger;
        private readonly IMyLogger _filelogger;

        public WebhookController(ILogger<WebhookController> logger, IMyLogger filelogger)
        {
            _logger = logger;
            _filelogger = filelogger;
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageForCreationDto messageForCreationDto)
        {
            string symbol = messageForCreationDto.message.text;            
            string url = Binance24hrUrl.Replace("pair", symbol);
            try {        
                var get24hrTicker = new ApiGetingData<_24hrTickerDto>(_logger);
                var ticker = await get24hrTicker.GetDataAsync(url);
                await LogInformation(ticker.ToString());
            } catch(Exception ex) {
                await LogInformation(ex.Message);
            } finally {
                await LogInformation(messageForCreationDto.ToString());
            }
            return StatusCode(201);
        }

        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}