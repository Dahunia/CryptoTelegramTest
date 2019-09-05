using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoTelegram.Data.Work;
using CryptoTelegram.Dtos;
using CryptoTelegram.Dtos.Markets.Binance;
using CryptoTelegram.Data.Interface;
using System;
using Microsoft.Extensions.Options;
using CryptoTelegram.Data.Work.Interface;
using CryptoTelegram.Models.Settings;

namespace CryptoTelegram.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public const string Binance24hrUrl = "https://api.binance.com/api/v1/ticker/24hr?symbol=pair";
        private readonly ILogger<WebhookController> _logger;
        private readonly IMyLogger _filelogger;
        private readonly TelegramSettings _telegramConfig;
        private readonly ITelegramApiRequest _telegramRequest;
        public WebhookController(
            ILogger<WebhookController> logger, 
            IMyLogger filelogger,
            IOptions<TelegramSettings> telegramConfig,
            ITelegramApiRequest telegramRequest)
        {
            _logger = logger;
            _filelogger = filelogger;
            _telegramRequest = telegramRequest;
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageForCreationDto messageForCreationDto)
        {
            var messageForSend = CreateMessageForSend(messageForCreationDto.message);
            var response = await _telegramRequest.SendMessage(messageForSend);   
            return StatusCode(201);
        }
          private MessageForSendDto CreateMessageForSend(Message message) {
            var messageForSend = new MessageForSendDto() {
                chat_id = message.chat.id
            };
            switch (message.text.ToLower()) {
                case "/start":
                    messageForSend.text = "Наберите валютную пару Binance";
                    break;
                default:
                    messageForSend.text =$"Вы мне написли {message.text}. Вас зовут {message.from.username}";
                    break;
                /*
                  //string symbol = messageForCreationDto.message.text;            
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
                 */
            }
            return messageForSend;
        }

        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}