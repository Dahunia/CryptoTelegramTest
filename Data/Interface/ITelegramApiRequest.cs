using System.Threading.Tasks;
using CryptoTelegram.Dtos;

namespace CryptoTelegram.Data.Work.Interface
{
    public interface ITelegramApiRequest
    {
        //Task<UpdateForCreationDto> GetUpdate();
        Task<byte[]> SendMessage(MessageForSendDto message);
    }
}