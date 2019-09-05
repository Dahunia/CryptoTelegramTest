using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CryptoTelegram.Data.Work.Interface;
using CryptoTelegram.Dtos;
using CryptoTelegram.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoTelegram.Data.Work
{
    public class TelegramApiRequest : ITelegramApiRequest
    {
        private readonly TelegramSettings _telegramConfig;
        private readonly ILogger<TelegramApiRequest> _logger;

        public TelegramApiRequest(
            IOptions<TelegramSettings> telegramConfig,
            ILogger<TelegramApiRequest> logger)
        {
            _telegramConfig = telegramConfig.Value;
            _logger = logger;
        }

        public async Task<byte[]> SendMessage(MessageForSendDto message) 
        {
            string url = GetUrl("sendMessage");
            return await SendRequest(url, message);
        }
        public async Task<UpdateForCreationDto> GetUpdate() 
        {
            var url = GetUrl("getUpdates");
            var lastUpdateId = _telegramConfig.LastUpdate;
            var query = HttpUtility.ParseQueryString("");
            
            if (lastUpdateId != 0) {
                query["offset"] = lastUpdateId.ToString();

                url += "?" + query.ToString();
            }

            var update = await GetRequest<UpdateForCreationDto>(url);

            _logger.LogInformation(url + "\n");
            
            if (update.result.Count() > 0 ) {
                 _telegramConfig.LastUpdate = update.result.LastOrDefault().update_id + 1;
            }
            else { 
                _telegramConfig.LastUpdate = 0;
            }

            return update;
        }   
        private string GetUrl(string method) {
            return _telegramConfig.BaseEndpoint
                .Replace("<token>", _telegramConfig.Token)
                .Replace("METHOD_NAME", method); 
        }

        private async Task<T> GetRequest<T>(string url) {
            
            var getRequest = new ApiGetingData<T>(_logger);

            var response = await getRequest.GetDataAsync(
                url,
                _telegramConfig.Proxies.First()
            );
    
            return response;
        } 

        private async Task<byte[]> SendRequest<T>(string url, T entity) {

            var sendRequest = new ApiGetingData<T>(_logger);

            return await sendRequest.SendDataAsync(
                entity,
                url//, 
            //    _telegramConfig.Proxies.First()
            );
        }
    }
}