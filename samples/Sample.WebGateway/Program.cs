
var b = WebApplication.CreateBuilder(args);
b.Services.AddControllers();
var app = b.Build();
app.MapControllers();
app.Run("https://localhost:5001");
