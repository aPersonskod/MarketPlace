using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Dtos;
using Models.Extensions;
using Models.Interfaces;
using UserManipulations.Dtos;
using UserManipulations.Settings;

namespace UserManipulations.Controllers;

[ApiController]
[Route("[controller]")]
public class UserManipulationsController(
    IUserManipulations userManipulationsService,
    IOptions<AuthSettings> authSettings,
    IDistributedCache cache) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<IEnumerable<UserDto>> GetAll() => await userManipulationsService.Get();
    /*[HttpGet("{userId:guid}")]
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
    }*/
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        try
        {
            var userDto = await GetUser();
            if (userDto == null) return Unauthorized();
            return Ok(userDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    private async Task<UserDto?> GetUser()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        if (token == null) return null;
        var userDto = await cache.GetRecordAsync<UserDto>(token);
        return userDto ?? null;
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
        var response = new AuthorizationResponseDto(new JwtSecurityTokenHandler().WriteToken(jwt), userDto.Name);
        await cache.SetRecordAsync(response.AccessToken, userDto, TimeSpan.FromMinutes(10));
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
    public async Task<IActionResult> WalletReplenishment(int money)
    {
        try
        {
            var userDto = await GetUser();
            if (userDto == null) return Unauthorized();
            return Ok(await userManipulationsService.WalletReplenishment(userDto.Id, money));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> SpendMoney(int money)
    {
        try
        {
            var userDto = await GetUser();
            if (userDto == null) return Unauthorized();
            return Ok(await userManipulationsService.SpendMoney(userDto.Id, money));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}