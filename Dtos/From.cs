namespace CryptoTelegram.Dtos 
{
public class From
{
    public long id { get; set; }
    public bool is_bot { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string username { get; set; }
    public string language_code { get; set; }

    public override string ToString() {
        return 
            $"\tfrom id: {id}, " +
            $"is_bot: {is_bot}, " +
            $"first_name: {first_name}, " +
            $"last_name: {last_name}, " +
            $"username: {username}, " +
            $"language_code: {language_code}";
    }
}
}