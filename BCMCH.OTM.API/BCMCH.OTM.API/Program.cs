using BCMCH.OTM.API.Security;
using BCMCH.OTM.Infrastucture.AppSettings;
using BCMCH.OTM.Infrastucture.IOC;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependency(builder.Configuration);
var conn = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("BearerAuthentication")
                  .AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("BearerAuthentication", null);
builder.Services.AddHttpClient();

var app = builder.Build();





// cors added by sreejith
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
});
// cors added by sreejith




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
