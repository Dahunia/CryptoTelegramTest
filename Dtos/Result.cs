namespace CryptoTelegram.Dtos
{
    public class Result
    {
        public long update_id { get; set; }
        public Message message { get; set; }

        public override string ToString() {
            return $"update_id: {update_id}\n" +
                message.ToString();
        }
    }
}