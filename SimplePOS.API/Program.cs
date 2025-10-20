using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimplePOS.API.Middlewares;
using SimplePOS.Business.Extensions;
using SimplePOS.Business.Mapping;
using SimplePOS.Infrastructure.Data;
using SimplePOS.Infrastructure.Extensions;
using SimplePOS.Infrastructure.Identity;
using SimplePOS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

//Definir la politica CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//QuestPDF licencia
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

//Registro de AutoMapper
builder.Services.AddAutoMapper(mc => {mc.AddProfile(new SimplePOSProfile());});

//Agrega servicios de la capa Infrastructure (DbContext + Identity)
builder.Services.AddInfrastructureServices(builder.Configuration);

//Agrega servicios de la capa Business
builder.Services.AddBusinessServices(); 

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Documentacion Swagger con configuración personalizada
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "SimplePOS API",
        Description = @"<b>API para la gestión de un sistema de punto de venta simple.</b><br/>
                        <b>Usuario Admin de prueba:</b><br/> 
                        <b>Usuario:</b> admin@simplepos.com<br/>
                        <b>Contraseña:</b> Admin123*",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ariel Cedeño",
            Url = new Uri("https://example.com/support"),
            Email = "ariel.jcr98@outlook.es"
        },
    });

    var baseDir = AppContext.BaseDirectory;

    var apiXml = Path.Combine(baseDir, "SimplePOS.API.xml");
    var businessXml = Path.Combine(baseDir, "SimplePOS.Business.xml");

   
    options.IncludeXmlComments(apiXml, includeControllerXmlComments: true);
    options.IncludeXmlComments(businessXml);

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "\"Introduce el token JWT con el esquema 'Bearer'. Ejemplo: **Bearer eyJhbGci...**\"",
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    //Aplica migraciones para crear la base de datos al iniciar la aplicación
    context.Database.Migrate();

    //Seed de datos iniciales
    await DbInitializer.SeedAsync(context, userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//Middleware de manejo de errores global
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

//Habilita el servicio de archivos estáticos (necesario para wwwroot)
app.UseStaticFiles();

app.Run();
