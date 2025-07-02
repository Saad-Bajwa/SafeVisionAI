using Microsoft.EntityFrameworkCore;
using SafeVision_AI.API.Data;
using SafeVision_AI.API.Interfaces;
using SafeVision_AI.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("SafeVisionClient", policy => policy.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddControllers();
builder.Services.AddDbContext<SafeVisionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Custom Services
builder.Services.AddTransient<IEmailNotificationService, EmailNotificationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SafeVisionClient");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
