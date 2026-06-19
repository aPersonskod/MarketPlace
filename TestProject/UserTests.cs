using Models;

namespace TestProject;

public class UserTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void Test_SpendMoney()
    {
        var user1 = User.CreateUser("Petya", "petya@gmail.com", "password", 50, Role.user);
        user1.SpendMoney(30);
        Assert.That(user1.Wallet, Is.EqualTo(20));
        var exception1 = Assert.Throws<ArgumentException>(() => user1.SpendMoney(30));
        var exception2 = Assert.Throws<ArgumentException>(() => user1.SpendMoney(-30));
        Assert.That(exception1.Message, Does.Contain("Not enough money"));
        Assert.That(exception2.Message, Does.Contain("Incorrect money amount"));
    }

    [Test]
    public void Test_Deposit()
    {
        var her = new List<User>();
        var her2 = her.Select(GetUserDto);
    }
    
    private string GetUserDto(User? user)
    {
        if (user == null) throw new Exception("Convert to dto error: user not found");
        if (!Enum.TryParse<Models.Role>(user.Role, out var role)) throw new Exception("Convert to dto error: Invalid role");
        return "dto";
    }
}