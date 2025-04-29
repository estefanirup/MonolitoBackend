using Microsoft.EntityFrameworkCore;
using MonolitoBackend.Infrastructure.Data;
using MonolitoBackend.Core.Repositories;
using MonolitoBackend.Core.Services;
using MonolitoBackend.Infrastructure.Repositories;
using AutoMapper;
using MonolitoBackend.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MonolitoBackend.Api.MappingProfile)); 

// Configuração do DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"\n\nCONNECTION STRING BEING USED:\n{connectionString}\n\n");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString));

// Registro dos repositórios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro dos services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicar migrações do banco de dados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();