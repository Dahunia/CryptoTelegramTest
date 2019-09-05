namespace CryptoTelegram.Dtos
{
    public class Chat
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }

        public override string ToString() {
            return $"\tchat id: {id}, " +
                $"first_name: {first_name}, " +
                $"last_name: {last_name}, " +
                $"type: {type}";
        }
    }
}