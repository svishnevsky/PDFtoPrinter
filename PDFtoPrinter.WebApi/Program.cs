WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
WebApplication app = builder.Build();
app.MapControllers();
app.Run();
