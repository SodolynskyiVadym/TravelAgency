var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("secret.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();
app.UseCors("DevCors");
app.MapControllers();

app.Run();