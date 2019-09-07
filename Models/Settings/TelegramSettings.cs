using System.Collections.Generic;

namespace CryptoTelegram.Models.Settings
{
    public class TelegramSettings
    {
        public bool WithProxy { get; set; }
        public string BaseEndpoint { get; set; }
        public string Token { get; set; }
        //public Dictionary<string, int> Proxies { get; set; }
        public List<string> Proxies { get; set; }
        public long LastUpdate { get; set; }
    }
}