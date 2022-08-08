using AutoMapper;
using Core.Interface;
using LogicaTrigonos.Data;
using LogicaTrigonos.Logic;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();


//CODIGO PARA AMENTAR EL TAMAÑO DEL BUFFER
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
app.UseAuthorization();

app.MapControllers();

app.Run();

