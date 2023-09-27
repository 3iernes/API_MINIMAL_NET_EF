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
    //Valido el token
    if (string.IsNullOrWhiteSpace(reqBody.Token) || !reqBody.Token.Equals(token))
    {
        return Results.BadRequest(new ScanResponse()
        {
            Creado = false,
            Encontrado = false,
            Mensaje = "Error en el token",
            Activo = false,
            Error = true
        });
    }

    //Valido que no se ingrese el CB cero
    if (reqBody.CB == 0)
        return Results.BadRequest(new ScanResponse()
        {
            Creado = false,
            Encontrado = false,
            Error = true,
            Mensaje = "El código 0 no esta permitido.",
            Activo = false
        });

    //Busco el CB en la base de datos
    var findCB = await dbContext.BarCodes.FirstOrDefaultAsync(bc => bc.CodeNumbers == reqBody.CB);

    //Si no lo encuentro, lo creo
    if (findCB == null)
    {
        var barcode = new BarCode() { CodeNumbers = reqBody.CB, Active = true };
        await dbContext.BarCodes.AddAsync(barcode);
        await dbContext.SaveChangesAsync();
        return Results.Ok(new ScanResponse() { 
            Creado = true, 
            Encontrado = false,
            Activo = true,
            Mensaje = "CB registrado con exito.",
            Error = false,
        });
    }
    else
    {
        findCB.Pos = findCB.Pos == 5 ? 0 : findCB.Pos + 1;
        //Sino, aviso que lo encontre, por lo tanto no lo creo
        return Results.Ok(new ScanResponse() { 
            Creado = false,
            Encontrado = true,
            Activo = findCB.Active,
            Error = false 
        });
    }
})
.WithName("LecturaCodBarra")
.WithOpenApi();

app.MapPost("/codigo-barra", async (DataBase dbContext, CodBarraRequest reqBody) =>
{
    //Valido el token
    if (string.IsNullOrWhiteSpace(reqBody.Token) || !reqBody.Token.Equals(token))
    {
        return Results.BadRequest(new SwitchActiveRes()
        {
            Encontrado = false,
            Mensaje = "Error en el token",
            Activo = false,
            Error = true
        });
    }

    //Valido que no se ingrese el CB cero
    if (reqBody.CB == 0)
        return Results.BadRequest(new SwitchActiveRes()
        {
            Encontrado = false,
            Error = true,
            Mensaje = "El código 0 no esta permitido.",
            Activo = false
        });

    //Busco el CB en la base de datos
    var findCB = await dbContext.BarCodes.FirstOrDefaultAsync(bc => bc.CodeNumbers == reqBody.CB);

    //Si no lo encuentro, aviso
    if (findCB == null)
    {
        return Results.Ok(new SwitchActiveRes()
        {
            Encontrado = false,
            Activo = false,
            Mensaje = "No se encontro el codigo de barra al cual se quiere cambiar su prop activo.",
            Error = true,
        });
    }
    else
    {
        var prevActive = findCB.Active;

        findCB.Active = !findCB.Active;
        await dbContext.SaveChangesAsync();
        //Sino, aviso que lo encontre, por lo tanto no lo creo
        return Results.Ok(new SwitchActiveRes()
        {
            Encontrado = true,
            Activo = findCB.Active,
            Mensaje = $"Se cambio el valor active de {prevActive} a {findCB.Active} para el codigo de barras {findCB.CodeNumbers}",
            Error = false
        });
    }
})
.WithName("SwitchActiveCodBarra")
.WithOpenApi();

app.Run();
