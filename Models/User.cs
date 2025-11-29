namespace Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Nick { get; set; }
    public string Password { get; set; }
    public int Wallet { get; set; }
}