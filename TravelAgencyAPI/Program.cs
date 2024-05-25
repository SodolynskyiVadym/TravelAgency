using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI;
using TravelAgencyAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Rework this part of the code
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]);
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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