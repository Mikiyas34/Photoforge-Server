using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Photoforge_Server;
using Photoforge_Server.Data;
using Photoforge_Server.Models;
using Photoforge_Server.Services;
using Serilog;

Log.Logger = new LoggerConfiguration().
    WriteTo.Console().CreateLogger();

Log.Information("Starting web application");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PhotoforgeDbContext>(options => options.UseSqlServer(Config.DbConnectionString()).LogTo(Console.WriteLine, LogLevel.Information));


builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<PhotoforgeDbContext>();

builder.Services.AddAuthorization();


builder.Services.AddCors(p => p.AddPolicy("policy", policy =>
{
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
    //policy.AllowAnyOrigin();
    policy.WithOrigins("http://localhost:4200");
    policy.Build();
}));

builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IMailService, MailService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseCors("policy");

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{

    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

    },


    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/images",
});



app.UseRouting();
// app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<User>();

app.MapControllers();

app.Run();