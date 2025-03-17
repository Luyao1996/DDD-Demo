using DDD_Demo.DemoService;
using Microsoft.AspNetCore.Mvc;

namespace DDD_Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    public IUserManager _UserManager { get; set; }

    [HttpGet("sayHello")]
    public async Task<string> SayHelloAsync(string name) => $"hello {name},I am {_UserManager.GetName()}";
}