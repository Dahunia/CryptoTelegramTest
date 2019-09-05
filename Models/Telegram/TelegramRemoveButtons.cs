namespace CryptoTelegram.Models.Telegram
{
    /* ReplyKeyboardRemove */
    public class TelegramRemoveButtons
    {
        public TelegramRemoveButtons()
        {
            remove_keyboard = true;
        }

        public bool remove_keyboard { get; set; }
    }
}