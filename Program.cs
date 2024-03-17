using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Photoforge_Server;
using Photoforge_Server.Data;
using Photoforge_Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PhotoforgeDbContext>(o => o.UseSqlServer(Config.DbConnectionString()));
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IMailService, MailService>();
var mailConfig = builder.Configuration.GetSection("MailSettings");
builder.Services.AddSingleton(mailConfig);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseCors(p =>
{
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.WithOrigins("http://localhost:4200");
    p.Build();
});
app.MapControllers();

app.Run();
