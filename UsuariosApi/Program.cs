using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsuariosApi.Authorizantion;
using UsuariosApi.Data;
using UsuariosApi.Models;
using UsuariosApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("UsuarioConnection");

builder.Services.AddDbContext<UsuarioDbContext>
    (opts =>
    {
        opts.UseMySql(connString, ServerVersion.AutoDetect(connString));
    });

builder.Services
    .AddIdentity<Usuario, IdentityRole>() // Adiciona o Identity ao projeto
    .AddEntityFrameworkStores<UsuarioDbContext>() // Adiciona o Identity ao EntityFramework para armazenar os dados
    .AddDefaultTokenProviders(); // Adiciona o Identity ao EntityFramework para gerar tokens

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Adiciona o AutoMapper ao projeto

builder.Services.AddScoped<UsuarioService>(); // Adiciona o servi�o de cadastro ao projeto, AddScoped para que o servi�o seja instanciado uma vez por requisi��o
builder.Services.AddScoped<TokenService>(); // Adiciona o servi�o de token ao projeto, AddScoped para que o servi�o seja instanciado uma vez por requisi��o

builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>(); // Adiciona a autoriza��o ao projeto

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autentica��o padr�o
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters //opi��es de valida��o do token
    {
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do emissor
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9SAFAFWEFWE9FEWFWEFEW54F8WEF489WEF")), // Chave de assinatura
        ValidateAudience = false, // Valida o p�blico alvo do token
        ValidateIssuer = false, // Valida o emissor do token
        ClockSkew = TimeSpan.Zero // Define o tempo de toler�ncia para a valida��o do token
    };
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IdadeMinima", policy => policy.AddRequirements(new IdadeMinima(18)));
    //cria a policy que foi passada na controller AcessoController e adiciona a regra de idade m�nima com a classe IdadeMinima
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //alem de adicionar o middleware de autentica��o, � necess�rio adicionar o middleware de autoriza��o

app.UseAuthorization();

app.MapControllers();

app.Run();
