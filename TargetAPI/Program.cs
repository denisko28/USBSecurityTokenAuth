using Microsoft.EntityFrameworkCore;
using TargetAPI;
using TargetAPI.Data;
using TargetAPI.Repositories.Abstract;
using TargetAPI.Repositories.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("MSSQLConnection");
builder.Services.AddDbContext<TargetDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();