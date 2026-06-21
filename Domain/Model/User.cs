namespace Model;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Wallet { get; set; }
    public string Role { get; set; }

    public static User CreateUser(string name, string email, string password, int wallet, Role role) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = password,
            Wallet = wallet,
            Role = role.ToString()
        };

    public void AddMoney(int money)
    {
        if (money < 0) throw new ArgumentException("Incorrect money amount");
        Wallet += money;
    }
    
    public void SpendMoney(int money)
    {
        if (money < 0) throw new ArgumentException("Incorrect money amount");
        if (Wallet < money) throw new ArgumentException("Not enough money");
        Wallet -= money;
    }
}

public enum Role
{
    user,
    admin
}