using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Interfaces;
using UserManipulations.Dtos;

namespace UserManipulations.Controllers;

[ApiController]
[Route("[controller]")]
public class UserManipulationsController(IUserManipulations userManipulationsService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<UserDto>> Get() => await userManipulationsService.Get();
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> Get(Guid userId)
    {
        try
        {
            return Ok(await userManipulationsService.Get(userId));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Authorize([FromBody]CredentialsDto credentials)
    {
        try
        {
            return Ok(await userManipulationsService.Authorize(credentials.email, credentials.password));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> AddUser(UserDto userDto)
    {
        try
        {
            return Ok(await userManipulationsService.Add(userDto));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateUser(UserDto userDto)
    {
        try
        {
            return Ok(await userManipulationsService.Update(userDto));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            await userManipulationsService.Delete(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> WalletReplenishment(Guid userId, int money)
    {
        try
        {
            return Ok(await userManipulationsService.WalletReplenishment(userId, money));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SpendMoney(Guid userId, int money)
    {
        try
        {
            return Ok(await userManipulationsService.SpendMoney(userId, money));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}