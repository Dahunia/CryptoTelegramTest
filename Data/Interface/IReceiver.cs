using System.Threading.Tasks;

namespace CryptoTelegram.Data.Interface
{
    public interface IReceiver {
        void Write(string info);
        Task WriteAsync(string info);
        string Read();
    }
}