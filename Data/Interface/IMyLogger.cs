using System.Threading.Tasks;

namespace CryptoTelegram.Data.Interface
{
    public interface IMyLogger
    {
        void WriteInformation(string info);
        Task WriteInformationAsync(string info);
    }
}