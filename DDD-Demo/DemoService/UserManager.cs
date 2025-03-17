using Volo.Abp.DependencyInjection;

namespace DDD_Demo.DemoService;

public class UserManager:IUserManager,ITransientDependency
{
    public string GetName()
    {
        return "老王";
    }
}