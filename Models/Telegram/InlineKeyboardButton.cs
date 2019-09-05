namespace CryptoTelegram.Models.Telegram
{
    public class InlineKeyboardButton
    {
        public InlineKeyboardButton(string text, string url = "", string callback_data = "")
        {
            this.text = text;
            this.url = url;
            this.callback_data = callback_data == "" ? text : callback_data;
        }

        public string text { get; set; }
        public string url { get; set; }
        public string callback_data { get; set; }
    }
}