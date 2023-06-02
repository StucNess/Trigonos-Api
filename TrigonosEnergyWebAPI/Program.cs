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
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(options => options.Limits.KeepAliveTimeout = TimeSpan.FromDays(1));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddScoped<ITokenService, TokenService>();

// Add services to the container.
builder.Services.AddDbContext<TrigonosDBContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("TrigonosConnection")));
builder.Services.AddDbContext<SecurityDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("TrigonosConnection"));
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddScoped(typeof(IGenericSecurityRepository<>), (typeof(GenericSecurityRepository<>)));
builder.Services.AddScoped(typeof(IGenericRepositoryRole<>), (typeof(GenericSecRespositoryRol<>)));

builder.Services.AddScoped<IRepositoryUsuario,UsuarioRepository>();

builder.Services.AddScoped<IRepositoryRolesUser, RolesUsuarioRepository>();
//builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
//var builder = WebApplication.CreateBuilder(args);



var builder2 = builder.Services.AddIdentityCore<Usuarios>();
//builder2.Services.TryAddSingleton<ISystemClock, SystemClock>();
builder2 = new IdentityBuilder(builder2.UserType, builder2.Services);
builder2.AddRoles<Rol>();
builder2.AddEntityFrameworkStores<SecurityDbContext>();
builder2.AddSignInManager<SignInManager<Usuarios>>();
builder2.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token:Key"])),
        ValidIssuer = builder.Configuration["AppSettings:Token:Issuer"],
        ValidateIssuer = true,
        ClockSkew = TimeSpan.Zero,
        ValidateAudience = false,
    };
});
builder.Services.AddAuthorization();
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
    options.SwaggerDoc("APIFacturacionCl", new OpenApiInfo()
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
    options.SwaggerDoc("APIRol", new OpenApiInfo()
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
    options.SwaggerDoc("APIEmpresas", new OpenApiInfo()
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
    options.SwaggerDoc("APIBanks", new OpenApiInfo()
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
    options.SwaggerDoc("APINominas", new OpenApiInfo()
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
    options.SwaggerDoc("APIDesconformidades", new OpenApiInfo()
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



var scope = app.Services.CreateScope();
var services1 = scope.ServiceProvider;
var logger = services1.GetRequiredService<ILoggerFactory>();

//var userManager = services1.GetRequiredService<UserManager<Usuarios>>();
//var RoleManager = services1.GetRequiredService<RoleManager<IdentityRole>>();
//var identityContext = services1.GetRequiredService<SecurityDbContext>();
//await identityContext.Database.MigrateAsync();
//await SecurityDbContextData.SeedUserAsync(userManager, RoleManager);


app.UseSwaggerUI();
app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//Para la autenticacion y autorizacion


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
    options.SwaggerEndpoint("/swagger/APIFacturacionCl/swagger.json", "API FacturacionCl");
    options.SwaggerEndpoint("/swagger/APIUsuarios/swagger.json", "API Usuarios");
    options.SwaggerEndpoint("/swagger/APIRol/swagger.json", "API Rol");
    options.SwaggerEndpoint("/swagger/APIEmpresas/swagger.json", "API Empresas");
    options.SwaggerEndpoint("/swagger/APIBanks/swagger.json", "API Banks");
    options.SwaggerEndpoint("/swagger/APINominas/swagger.json", "API Nominas");
    options.SwaggerEndpoint("/swagger/APIDesconformidades/swagger.json", "API Desconformidades");

    options.RoutePrefix = string.Empty;
});
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<DeChunkerMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

