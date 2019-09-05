using System.IO;
using System.Text;
using System.Threading.Tasks;
using CryptoTelegram.Models.Settings;
using CryptoTelegram.Data.Interface;
using Microsoft.Extensions.Options;

namespace CryptoTelegram.Data.Work
{

    public class FileReceiver : IReceiver
    {
        public readonly string _fileName;

        public FileReceiver(IOptions<AppSettings> _appConfig) {
            _fileName = _appConfig.Value.FileLoggerName;
        }
        public FileReceiver(string fileName) => _fileName = fileName;
        public string Read()
        {
            throw new System.NotImplementedException();
        }

        public void Write(string info)
        {
            //Encoding.RegisterProvider(CodepPagesEncodingProvider.Instance);
            //Encoding.GetEncoding("windows-1251");
            using(StreamWriter sw = new StreamWriter(_fileName, true, Encoding.UTF8))
            {
                sw.WriteLine(info);
            }
        }
        public async Task WriteAsync(string info)
        {
            //Encoding.RegisterProvider(CodepPagesEncodingProvider.Instance);
            //Encoding.GetEncoding("windows-1251");
            using(StreamWriter sw = new StreamWriter(_fileName, true, Encoding.UTF8))
            {
                await sw.WriteAsync(info);
            }
        }
    }
}