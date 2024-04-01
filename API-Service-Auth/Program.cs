using API_Service_Auth.Configs;
using API_Service_Auth.Models.Requests;
using API_Service_Auth.Models.Responses;
using API_Service_Auth.Services; // Assuming AuthService is in this namespace
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();

// Konfigurasi JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Register AuthService as a singleton
builder.Services.AddSingleton<AuthService>();
builder.Services.AddMvc();

var app = builder.Build();

// Gunakan autentikasi dan otorisasi
app.UseAuthentication();
app.UseAuthorization();

var generatorDB = new GeneratorDB();
generatorDB.EnsureDatabaseCreated();

app.MapPost("/login", async (LoginRequest request) => 
{
 

    var services = new AuthService(builder.Configuration);
    var response = await services.Login(request);
    if (!response.Success)
    {
        return Results.BadRequest(response);
    }
    return Results.Ok(response);
}).Accepts<LoginRequest>("application/json").Produces<LoginResponse>(StatusCodes.Status200OK);

app.Run();
