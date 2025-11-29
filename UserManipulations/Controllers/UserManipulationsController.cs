using Microsoft.AspNetCore.Mvc;
using Models;

namespace UserManipulations.Controllers;

[ApiController]
[Route("[controller]")]
public class UserManipulationsController(IUserManipulations userManipulationsService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<User>> Get() => await userManipulationsService.Get();
    [HttpGet("{userId:guid}")]
    public async Task<User?> Get(Guid userId) => await userManipulationsService.Get(userId);
    [HttpPut]
    public async Task<User> AddUser(User user) => await userManipulationsService.Add(user);
    [HttpPatch]
    public async Task<User?> UpdateUser(User user) => await userManipulationsService.Update(user);
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await userManipulationsService.Delete(userId);
        return Ok();
    }
    [HttpPost("[action]")]
    public async Task<User> WalletReplenishment(Guid userId, int money) 
        => await userManipulationsService.WalletReplenishment(userId, money);
    [HttpPost("[action]")]
    public async Task<User> SpendMoney(Guid userId, int money) 
        => await userManipulationsService.SpendMoney(userId, money);
}