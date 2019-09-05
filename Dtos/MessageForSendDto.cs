namespace CryptoTelegram.Dtos
{
    public class MessageForSendDto
    {
        public long chat_id { get; set; }
        public string text { get; set; }
        //public TelegramButtons reply_markup { get; set; }
        public string reply_markup { get; set; }
    }
}