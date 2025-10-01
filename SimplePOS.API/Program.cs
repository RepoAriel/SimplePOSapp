using AutoMapper;
using SimplePOS.Business.Extensions;
using SimplePOS.Business.Mapping;
using SimplePOS.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
