using DDD_Demo;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac();
await builder.AddApplicationAsync<Module>();

var app = builder.Build();

await app.InitializeApplicationAsync();
await app.RunAsync();