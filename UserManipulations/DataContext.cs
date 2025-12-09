using Microsoft.EntityFrameworkCore;
using Models.Dtos;

namespace UserManipulations;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserDto> Users { get; set; }
    //public List<User> Users { get => StaticData.Users; set => StaticData.Users = value; }
}

public class StaticData
{
    public static List<UserDto> Users { get; set; } =
    [
        new UserDto()
        {
            Id = Guid.NewGuid(),
            Name = "Петя Пяточкин",
            Email = "patochin228@gmail.com",
            Password = "12345"
        },
        new UserDto()
        {
            Id = Guid.NewGuid(),
            Name = "Андрей Викторович",
            Email = "the_king@gmail.com",
            Password = "password"
        }
    ];
}