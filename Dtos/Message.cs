namespace CryptoTelegram.Dtos
{
    public class Message
    {
        public long message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }

        public override string ToString() {
            return $"\tmessage_id: {message_id}\n" +
                chat.ToString() + "\n" +
                from.ToString() + "\n" +
                $"\tdate: {date}\n" +
                $"\ttext: {text}\n";
        }
    }
}