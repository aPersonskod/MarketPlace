using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Dtos;
using Models.Interfaces;
using UserManipulations.Dtos;
using UserManipulations.Settings;

namespace UserManipulations.Controllers;

[ApiController]
[Route("[controller]")]
public class UserManipulationsController(IUserManipulations userManipulationsService, IOptions<AuthSettings> authSettings) : ControllerBase
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
        var userDto = await userManipulationsService.Authorize(credentials.Email, credentials.Password);
        if (userDto == null) return Unauthorized();
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, userDto.Role.ToString())
        };
        var jwt = new JwtSecurityToken(
            issuer: authSettings.Value.Issuer,
            audience: authSettings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: new SigningCredentials(authSettings.Value.SecurityKey, SecurityAlgorithms.HmacSha256)
        );
        var response = new 
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(jwt),
            userName = userDto.Name
        };
        return Ok(response);
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
    [Authorize]
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
    [Authorize]
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