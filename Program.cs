using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Photoforge_Server;
using Photoforge_Server.Data;
using Photoforge_Server.Services;
using Serilog;

Log.Logger = new LoggerConfiguration().
    WriteTo.Console().CreateLogger();

Log.Information("Starting web application");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PhotoforgeDbContext>(options => options.UseSqlServer(Config.DbConnectionString()).LogTo(Console.WriteLine, LogLevel.Information));


builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddTransient<IFileSystemService, FileSystemService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/images"
});
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