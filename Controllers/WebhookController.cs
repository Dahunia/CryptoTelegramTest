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
using CryptoTelegram.Models.Telegram;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            var messageForSend = await CreateMessageForSend(messageForCreationDto.message);
            await LogInformation(messageForCreationDto.ToString());

            var response = await _telegramRequest.SendMessage(messageForSend);   
            return StatusCode(201);
        }
          private async Task<MessageForSendDto> CreateMessageForSend(Message message) {
            var messageForSend = new MessageForSendDto() {
                chat_id = message.chat.id
            };
            switch (message.text.ToLower()) {
                case "/start":
                    messageForSend.text = "Наберите валютную пару или выберите из предложенных.";
                    break;
                case "/help":
                    messageForSend.text = "Здесь будет описание возможностей";
                    break;
                case "/menu":
                    messageForSend.text = "Проверка меню";
                    messageForSend.reply_markup = GetButtons(message.chat.id);
                    break;
                case "/remove":
                    messageForSend.text = "Удаление клавиатуры";
                    messageForSend.reply_markup = JsonConvert.SerializeObject(new TelegramRemoveButtons());
                    break;
                case "/inline":
                    messageForSend.text = "inline menu";
                    messageForSend.reply_markup = GetInlineButtons(message.chat.id);
                    break;
                default:
                    string symbol = message.text.ToUpper();
                    string url = Binance24hrUrl.Replace("pair", symbol);
                    try {        
                        var get24hrTicker = new ApiGetingData<_24hrTickerDto>(_logger);
                        var ticker = await get24hrTicker.GetDataAsync(url);

                        messageForSend.text = ticker.ToString();

                        await LogInformation(ticker.ToString());
                    } catch(Exception ex) {
                        await LogInformation(ex.Message);

                        messageForSend.text = $"Пары на Binance {message.text} не существует";
                    }
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

            private string GetButtons(long chat_id)
        {
            List<string> keyboardButton1 = new List<string>() { "/start", "/help", "/remove" };
            List<string> keyboradButton2 = new List<string>() { "Maybe" };
            List<string> keyboradButton3 = new List<string>() { "1", "2", "3" };
            List<List<string>> keyboard = new List<List<string>>() {
                keyboardButton1,
                keyboradButton2,
                keyboradButton3
            };
            return JsonConvert.SerializeObject(new TelegramButtons(keyboard));
        }
        private string GetInlineButtons(long chat_id) {
            var key1 = new InlineKeyboardButton("url1_1", "ya.ru");
            var key2 = new InlineKeyboardButton("url1_2", "yandex.ru");
            List<InlineKeyboardButton> keyboardButtons = new List<InlineKeyboardButton>() {
                key1,
                key2
            };
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>() {
                keyboardButtons
            };
            var inlineMarkup = new InlineKeyboardMarkup(buttons);

            return JsonConvert.SerializeObject(inlineMarkup);
        }

        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}