using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Data.Master;
using BCMCH.OTM.Domain.Contract.Master;
using BCMCH.OTM.Domain.Master;
using BCMCH.OTM.Infrastucture.AppSettings;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using BCMCH.OTM.Infrastucture.IOC;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<IMasterDomainService, MasterDomainService>();
//builder.Services.AddScoped<IMasterDataAccess, MasterDataAccess>();
//builder.Services.AddScoped<ISqlDbHelper, SqlDbHelper>();
//builder.Services.AddScoped<IConnectionStrings, ConnectionStrings>();
// Add services to the container.
//builder.Services.Configure<ConnectionStrings>(
//                builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>());

var conn = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();

builder.Services.AddControllers();
//builder.Configuration.GetConnectionString("ConnectionStrings");
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
