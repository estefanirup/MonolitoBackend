using Microsoft.OpenApi.Models;
using MonolitoBackend.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


// ---------- Adicionado para rodar no Docker ----------
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// ---------- Serviços da aplicação (inclui DB, JWT, AutoMapper, repositórios e serviços) ----------
builder.Services.AddApplicationServices(builder.Configuration);

// ---------- Swagger ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Ex: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ---------- Dev Tools ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------- Aplicar Migrações ----------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MonolitoBackend.Infrastructure.Data.AppDbContext>();
    context.Database.Migrate();
}

// ---------- Middlewares ----------
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
