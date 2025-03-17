using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Swashbuckle;

namespace DDD_Demo.Application;

[DependsOn(typeof(AbpSwashbuckleModule))]
public class Module:AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        ConfigureRoute(services);
        ConfigureSwagger(services);
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        
        if (env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        app.UseStaticFiles();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API");
        });
        
        base.OnApplicationInitialization(context);
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.HideAbpEndpoints();
                options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
                {
                    Description = "用户Token验证",
                    Name = "token", //jwt默认的参数名称
                    In = ParameterLocation.Header, //jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                
                var cuttentDir = AppContext.BaseDirectory; // 当前目录
                // 获取下面所有的xml文件
                string[] xmlFiles = Directory.GetFiles(cuttentDir, "*.xml", SearchOption.TopDirectoryOnly);  
                foreach (var file in xmlFiles)
                {
                    string relativeFileName = Path.GetFileName(file);
                    var path = Path.Combine(AppContext.BaseDirectory, relativeFileName);
                    Console.WriteLine($"include xml path : {path}");
                    options.IncludeXmlComments(path, true);
                }
            }
        );
    }

    private void ConfigureRoute(IServiceCollection services)
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // 自动将应用服务转换为API控制器
            // 如何不加这个option 则： /api/app/hello/say-hello2
            // 加了之后：/api/demoApi/hello/sayhello2
            options.ConventionalControllers.Create(typeof(Module).Assembly, opt =>
            {
                opt.RootPath = "demoApi";
                opt.UrlControllerNameNormalizer = name =>name.ControllerName.ToLower(); // 控制器名转换
                opt.UrlActionNameNormalizer = name =>name.ActionNameInUrl.ToLower(); // 方法名转换
            });
        });
    }
}