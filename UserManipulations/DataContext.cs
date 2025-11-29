using Models;

namespace UserManipulations;

public class DataContext
{
    public List<User> Users { get => StaticData.Users; set => StaticData.Users = value; }
}

public class StaticData
{
    public static List<User> Users { get; set; } =
    [
        new User()
        {
            Id = Guid.NewGuid(),
            Name = "Петя Пяточкин",
            Email = "patochin228@gmail.com",
            Password = "12345"
        },
        new User()
        {
            Id = Guid.NewGuid(),
            Name = "Андрей Викторович",
            Email = "the_king@gmail.com",
            Password = "password"
        }
    ];
}