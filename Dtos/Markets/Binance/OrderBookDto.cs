using System.Collections.Generic;
using System.Linq;

namespace CryptoTelegram.Dtos.Markets.Binance
{
    public class OrderBookDto
    {
        public string lastUpdateId { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }

        public override string ToString() {
            string result = "";
            
            result += "-продают:\n";
            bids.ForEach( bid => {
                result += $"\t\tЦена: {bid.First()} Количество: {bid.Last()}\n";
            });

            result += "+покупают\n";
            bids.ForEach( asks => {
                result += $"\t\tЦена: {asks.First()} Количество: {asks.Last()}\n";
            });

            return result;
        }
    }
}