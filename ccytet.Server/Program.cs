using ccytet.Server.Data;
using ccytet.Server.Models;
using ccytet.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ccytet.Server.Tokens;
using AutoMapper;
using ccytet.Server.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.}// AutoMapper Service
builder.Services.AddAutoMapper(typeof(Program));
var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});

//inject jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer              = false,    // Validar el emisor
        ValidateAudience            = false,    // Validar la audiencia
        ValidateLifetime            = true,     // Validar el tiempo de vida del token
        ValidateIssuerSigningKey    = true,     // Validar la firma

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yPkCqn4kSWLtaJwXvN2jGzpQRyTZ3gdXkt7FeBJP"))
    };
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString(builder.Environment.IsProduction() ? "Production" : "Development")));
builder.Services.AddIdentity<AspNetUser, IdentityRole>(options =>
{
    options.Password.RequiredLength         = 1;
    options.Password.RequireLowercase       = false;
    options.Password.RequireUppercase       = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit           = false;
    options.Password.RequiredUniqueChars    = 0;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();
builder.Services.AddSingleton<JwtHandler>();

//Scope Services
builder.Services.AddScoped<AspNetUserService, AspNetUserService>();
builder.Services.AddScoped<NoticiasService, NoticiasService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors( x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(o => true).AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
