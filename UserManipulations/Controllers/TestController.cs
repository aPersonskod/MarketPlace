using Microsoft.AspNetCore.Mvc;

namespace UserManipulations.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Get() => "This is a test api, and it works !!!";
}