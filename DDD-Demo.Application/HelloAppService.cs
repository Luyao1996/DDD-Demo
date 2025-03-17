using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace DDD_Demo.Application;

public class HelloAppService:ApplicationService
{
    public async Task<string> SayHello2Async(string name)
    {
        return $"hello {name}";
    }
}