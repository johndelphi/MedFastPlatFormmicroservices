using AutoMapper;
using Medfast.Services.MedicationAPI;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", build =>
    {
        
        build.WithOrigins("http://localhost:3000", "http://localhost:59002")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


//enable cors for single domain
//multiple domain
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
