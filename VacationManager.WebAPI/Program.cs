using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using VacationManager.Domain.Configurations.Helper;
using VacationManager.Domain.Configurations.Jwt;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfaceService.Authentification;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Services;
using VacationManager.Domain.Services.Authentification;
using VacationManager.Infrastructure.Data;
using VacationManager.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Cors
string? corsOrigin = builder.Configuration.GetSection("CorsOrigin").Get<string>();

builder.Services.AddCors(o =>
{
    o.AddPolicy(
        name: "backend.WebApi",
        p => p.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.AddDbContext<VacationManagerDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionVacationManager")));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configuration de l'authentification et du format de token
//builder.Services.AddSingleton<JwtSettings>(builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>());

var jwtSettings = builder.Services.BuildServiceProvider().GetService<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Ajout des services nécessaires à l'injection de dépendances de VacationsCalendar (congé)
builder.Services.AddScoped<IVacationsCalendarService, VacationsCalendarService>();
builder.Services.AddScoped<IVacationsCalendarRepository, VacationsCalendarRepository>();

// Ajout des services nécessaires à l'injection de dépendances de Vacations (congé)
builder.Services.AddScoped<IVacationsService, VacationsService>();
builder.Services.AddScoped<IVacationsRepository, VacationsRepository>();


var holidays = new List<DateTime>();

// Ajout des services nécessaires à l'injection de dépendances de Vacations Balance (solde de congé)
builder.Services.AddScoped<IVacationsBalanceService>(provider =>
    new VacationsBalanceService(
        provider.GetService<IVacationsRepository>(),
        provider.GetService<IUsersRepository>(),
        provider.GetService<IOptions<VacationOptions>>(),
        provider.GetService<IVacationsBalanceRepository>(),
        holidays // Passer la liste de dates ici
    )
);

builder.Services.AddScoped<IVacationsBalanceRepository, VacationsBalanceRepository>();

// Ajout des services nécessaires à l'injection de dépendances de Vacations Report (rapport de congé)
builder.Services.AddScoped<IVacationsReportService, VacationReportService>();
builder.Services.AddScoped<IVacationsReportRepository, VacationsReportRepository>();

// Ajout des services nécessaires à l'injection de dépendances de Roles
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();

// Ajout des services nécessaires à l'injection de dépendances de Users
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddControllers();

//Configuration du solde initial de congé
builder.Services.Configure<VacationOptions>(options =>
{
    options.InitialBalance = 23;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//configuration de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Vacation Manager API",
        Version = "V1.0.0",
        Description = "<h3>API pour la gestion d'une application web de gestion des congés pour les employés et les gestionnaires au sein d'une entreprise, cas de : <strong>INFI SOFTWARE</strong></h3>",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Site Officiel de INFI",
            Url = new Uri("http://www.infisoftware.net/fr/")
        },
        License = new OpenApiLicense
        {
            Name = "Site Officiel de SPVIE",
            Url = new Uri("https://www.spvie.com/")
        }
    });
    //Pour utiliser le fichier XML
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Ajouter le token ainsi : \"Bearer xxxx\" où xxxx est votre token d'authentification",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

//Pour utiliser l'UI de swagger à la racine de l'appplication
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.InjectStylesheet("/swagger-ui/custom.css");
});

app.UseCors("backend.WebApi");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
