using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaBecas.Api.Configuration;
using SistemaBecas.Api.Data;
using SistemaBecas.Api.Repositories;
using SistemaBecas.Api.Repositories.Documentacion;
using SistemaBecas.Api.Repositories.DocumentacionRepository;
using SistemaBecas.Api.Repositories.EstudianteRepository;
using SistemaBecas.Api.Repositories.LoginRepository;
using SistemaBecas.Api.Repositories.RecuperacionPassword;
using SistemaBecas.Api.Repositories.RegistroRepository;
using SistemaBecas.Api.Services.Documentacion;
using SistemaBecas.Api.Services.Email;
using SistemaBecas.Api.Services.JwtService;
using SistemaBecas.Api.Services.LoginService;
using SistemaBecas.Api.Services.RecuperacionPassword;
using SistemaBecas.Api.Services.RegistroService;
using SistemaBecas.Api.Services.SupabaseStorage;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Entity Framework Core
builder.Services.AddDbContext<BecasDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));
//JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

Console.WriteLine("==============================");
Console.WriteLine($"JWT KEY: {!string.IsNullOrWhiteSpace(jwtKey)}");
Console.WriteLine($"JWT ISSUER: [{jwtIssuer}]");
Console.WriteLine($"JWT AUDIENCE: [{jwtAudience}]");
Console.WriteLine("==============================");


//Repositorios
builder.Services.AddScoped<DbConnectionBecas>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IRegistroRepository, RegistroRepository>();
builder.Services.AddScoped<IRecuperacionPasswordRepository, RecuperacionPasswordRepository>();
builder.Services.AddScoped<IDocumentacionRepository, DocumentacionRepository>();


//Servicios
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRegistroService, RegistroService>();
builder.Services.AddScoped<IRecuperacionPasswordService, RecuperacionPasswordService>();
builder.Services.AddScoped<IDocumentacionService, DocumentacionService>();
builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
builder.Services.AddHttpClient<
    ISupabaseStorageService,
    SupabaseStorageService>();

//Configuracion Email
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();


//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            ),

            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("==============================");
                Console.WriteLine("JWT ERROR");
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine("==============================");

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("==============================");
                Console.WriteLine("JWT VALIDADO CORRECTAMENTE");

                foreach (var claim in context.Principal!.Claims)
                {
                    Console.WriteLine(
                        $"CLAIM: {claim.Type} = {claim.Value}");
                }

                Console.WriteLine("==============================");

                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                Console.WriteLine("==============================");
                Console.WriteLine("JWT CHALLENGE");
                Console.WriteLine($"ERROR: {context.Error}");
                Console.WriteLine(
                    $"DESCRIPTION: {context.ErrorDescription}");
                Console.WriteLine("==============================");

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
