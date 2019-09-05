
using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CryptoTelegram.Data.Work
{
    public class ApiGetingData<T>
    {
    
        private readonly ILogger _logger;
        public ApiGetingData(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<T> GetDataAsync(
            string url,
            string proxy = null)
        {
            WebClient web = new WebClient();
            web.Proxy = new WebProxy(proxy);

            var data_byte = await web.DownloadDataTaskAsync(url);
            var data_str = System.Text.Encoding.UTF8.GetString(data_byte);

            return JsonConvert.DeserializeObject<T>(data_str);
        }
        public async Task<byte[]> SendDataAsync(
            T entity,
            string url,
            string proxy = null) 
        {
            WebClient web = new WebClient();
            web.Proxy = new WebProxy(proxy);

            var parameters = GetParameters(entity);
            
            _logger.LogInformation($"Was send url: {url} with count of parameters: {parameters.Count}");
            return await web.UploadValuesTaskAsync(url, parameters);
        }
        private NameValueCollection GetParameters(T entity) {
            var parameters = new NameValueCollection();
            Type type = entity.GetType();

            foreach(PropertyInfo property in type.GetProperties()) 
            {
                _logger.LogInformation($"Added {property.Name} " +$"with value: {property.GetValue(entity, null)?.ToString()}");

                parameters.Add(
                    property.Name, 
                    property.GetValue(entity, null)?.ToString()
                    /*  property.PropertyType.ToString().StartsWith("System") || property.GetValue(entity, null) == null ?
                        property.GetValue(entity, null)?.ToString() :
                        JsonConvert.SerializeObject(property.GetValue(entity, null)) */
                );
            }

            return parameters;
        }
        public Task<T> GetDataAsync(string urlApi, string[] parameters, string proxy)
        {
            throw new Exception();
            //url = url.Replace("pair", pair);
            // .Replace("&limit=count", limit != "" ? $"&limit={limit}" : "");
        }
    }
}

/* 
if (property.PropertyType.ToString().StartsWith("System") || property.GetValue(entity, null) == null) {
    _logger.LogInformation($"Added {property.Name} " +
        $"with value: {property.GetValue(entity, null)?.ToString()}");
} else {
    _logger.LogInformation($"Added {property.Name} " +
        $"with value: {JsonConvert.SerializeObject(property.GetValue(entity, null))}");
} */
