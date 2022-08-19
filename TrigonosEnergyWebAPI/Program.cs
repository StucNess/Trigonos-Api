using AutoMapper;
using Core.Interface;
using LogicaTrigonos.Data;
using LogicaTrigonos.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TrigonosEnergy.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddDbContext<TrigonosDBContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("TrigonosConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("APIInstrucciones", new OpenApiInfo()
    {
        Title = "API TRGNS",
        Version = "v1",
        Description = "Backend .NET CORE 6",
        Contact = new OpenApiContact()
        {
            Email = "TRIGONOS@bluetreeam.com",
            Name  = "Equipo Informatico TRGNS",
            Url = new Uri("https://bluetreeam.com/?lang=es")
        },
        License = new OpenApiLicense()
        {
            Name="MIT License",
            Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
        }
        
    });
    options.SwaggerDoc("APIComboBox", new OpenApiInfo()
    {
        Title = "API TRGNS",
        Version = "v1",
        Description = "Backend .NET CORE 6",
        Contact = new OpenApiContact()
        {
            Email = "TRIGONOS@bluetreeam.com",
            Name = "Equipo Informatico TRGNS",
            Url = new Uri("https://bluetreeam.com/?lang=es")
        },
        License = new OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
        }

    });
    options.SwaggerDoc("APIParticipantes", new OpenApiInfo()
    {
        Title = "API TRGNS",
        Version = "v1",
        Description = "Backend .NET CORE 6",
        Contact = new OpenApiContact()
        {
            Email = "TRIGONOS@bluetreeam.com",
            Name = "Equipo Informatico TRGNS",
            Url = new Uri("https://bluetreeam.com/?lang=es")
        },
        License = new OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
        }

    });
    var archivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var ruta = Path.Combine(AppContext.BaseDirectory, archivo);
    options.IncludeXmlComments(ruta);

});


//CODIGO PARA AMENTAR EL TAMA�O DEL BUFFER
//builder.Services.AddControllers(options => options.MaxIAsyncEnumerableBufferLimit = 900000);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("CorsRule");
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsRule");
// Lineas para la documentacion
//app.UseSwagger(options =>
//{
//    options.SerializeAsV2 = true;
//});
app.UseAuthorization();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/APIInstrucciones/swagger.json", "API Instrucciones");
    options.SwaggerEndpoint("/swagger/APIComboBox/swagger.json", "API ComboBox");
    options.SwaggerEndpoint("/swagger/APIParticipantes/swagger.json", "API Participantes");
    options.RoutePrefix = string.Empty;
});
app.MapControllers();

app.Run();

