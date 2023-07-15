using AutoMapper;
using Medfast.Services.MedicationAPI;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Repository;
using Medfast.Services.MedicationAPI.Repository.PharmacyRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// logger 
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code) 
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", build =>
    {
        build.WithOrigins("http://localhost:64787", "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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
