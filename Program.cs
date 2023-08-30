using API_ESP_GW;
using API_ESP_GW.Clas;
using API_ESP_GW.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataBase>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var token = builder.Configuration.GetSection("AuthConfig:Token").Value;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPut("/codigo-barra",async (HttpContext httpContext,DataBase dbContext,double cb) =>
{
    var reqToken = httpContext.Request.Headers["Authorization"];

    if (string.IsNullOrWhiteSpace(reqToken.ToString()) || 
        !reqToken.ToString().StartsWith("Bearer ") || !reqToken.Equals(reqToken))
    {
        return Results.BadRequest(new ScanResponse() { Creado = false,Encontrado = false,Error = true});
    }
        

    var findCB = await dbContext.BarCodes.FirstOrDefaultAsync(bc => bc.CodeNumbers == cb);

    if (findCB == null)
    {
        var barcode = new BarCode(){ CodeNumbers = cb };
        await dbContext.BarCodes.AddAsync(barcode);
        await dbContext.SaveChangesAsync();
        return Results.Ok(new ScanResponse() { Creado=true,Encontrado = false, Error = false}) ;
    }
    else
    {
        return Results.Ok(new ScanResponse() { Creado = false, Encontrado = true, Error = false });
    }
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
