using AutoMapper;
using Core.Interface;
using LogicaTrigonos.Data;
using LogicaTrigonos.Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TrigonosEnergy.DTO;
using Microsoft.Extensions;
using TrigonosEnergyWebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddDbContext<TrigonosDBContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("TrigonosConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddScoped<IRepositoryUsuario,UsuarioRepository>();
//Agregando dependencia del toke

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;
    //options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});

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
    options.SwaggerDoc("APIUsuarios", new OpenApiInfo()
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


    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
            {
                Description = "Autenticación JWT (Bearer)",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
     
        
            });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                 
                }

            }, new List<string>()
        }

        });
    });


//CODIGO PARA AMENTAR EL TAMAÑO DEL BUFFER
//builder.Services.AddControllers(options => options.MaxIAsyncEnumerableBufferLimit = 900000);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseCors("CorsRule");
//    app.UseSwagger();
//    app.UseSwaggerUI();

//}

app.UseSwaggerUI();
app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//Para la autenticacion y autorizacion
app.UseAuthorization();
app.UseAuthentication();
app.UseCors("CorsRule");
// Lineas para la documentacion
//app.UseSwagger(options =>
//{
//    options.SerializeAsV2 = true;
//});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/APIInstrucciones/swagger.json", "API Instrucciones");
    options.SwaggerEndpoint("/swagger/APIComboBox/swagger.json", "API ComboBox");
    options.SwaggerEndpoint("/swagger/APIParticipantes/swagger.json", "API Participantes");
    options.SwaggerEndpoint("/swagger/APIUsuarios/swagger.json", "API Usuarios");
    options.RoutePrefix = string.Empty;
});
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");
app.MapControllers();
app.Run();

