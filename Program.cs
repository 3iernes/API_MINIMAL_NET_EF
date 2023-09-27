using API_ESP_GW;
using API_ESP_GW.Clas;
using API_ESP_GW.Clas.Requests;
using API_ESP_GW.Clas.Responses;
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


app.MapPut("/codigo-barra", async (DataBase dbContext, CodBarraRequest reqBody) =>
{

    if (string.IsNullOrWhiteSpace(reqBody.Token) || !reqBody.Token.Equals(token))
    {
        return Results.BadRequest(new ScanResponse()
        {
            Creado = false,
            Encontrado = false,
            Error = true,
            Mensaje = "Error en el token"
        });
    }


    var findCB = await dbContext.BarCodes.FirstOrDefaultAsync(bc => bc.CodeNumbers == reqBody.CB);

    if (findCB == null)
    {
        var barcode = new BarCode() { CodeNumbers = reqBody.CB };
        await dbContext.BarCodes.AddAsync(barcode);
        await dbContext.SaveChangesAsync();
        return Results.Ok(new ScanResponse() { Creado = true, Encontrado = false, Error = false });
    }
    else
    {
        findCB.Pos = findCB.Pos == 5 ? 0 : findCB.Pos + 1;
        return Results.Ok(new ScanResponse() { Creado = false, Encontrado = true, Error = false });
    }
})
.WithName("LecturaCodBarra")
.WithOpenApi();

app.Run();
