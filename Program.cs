using Microsoft.EntityFrameworkCore;
using RoomManagerAPI.Data;
using RoomManagerAPI.Models;
using RoomManagerAPI.Services;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=salas.db"));

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    "minha-chave-super-secreta-123456"
                )
            ),

            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

app.MapGet("/salas", async (AppDbContext db) =>
{
    return await db.SalasReuniao.ToListAsync();
}).RequireAuthorization();

app.MapPost("/login", (
    LoginRequest login,
    TokenService tokenService
) =>
{
    if(login.Email == "teste@teste.com" 
       && login.Senha == "123")
    {
        var token = tokenService.GerarToken();

        return Results.Ok(new
        {
            token
        });
    }

    return Results.Unauthorized();
});

app.MapPost("/salas", async (
    AppDbContext db,
    SalaReuniao sala
) =>
{
    db.SalasReuniao.Add(sala);

    await db.SaveChangesAsync();

    return Results.Created($"/salas/{sala.Id}", sala);
}).RequireAuthorization();

app.MapPut("/salas/{id}", async (
    int id,
    AppDbContext db,
    SalaReuniao sala
) =>
{
    var salaExistente = await db.SalasReuniao.FindAsync(id);

    if (salaExistente == null)
    {
        return Results.NotFound();
    }

    salaExistente.Nome = sala.Nome;
    salaExistente.Capacidade = sala.Capacidade;
    salaExistente.PossuiProjetor = sala.PossuiProjetor;

    await db.SaveChangesAsync();

    return Results.Ok(salaExistente);
}).RequireAuthorization();

app.MapDelete("/salas/{id}", async (
    int id,
    AppDbContext db
) =>
{
    var sala = await db.SalasReuniao.FindAsync(id);

    if (sala == null)
    {
        return Results.NotFound();
    }

    db.SalasReuniao.Remove(sala);

    await db.SaveChangesAsync();

    return Results.NoContent();
}).RequireAuthorization();

app.Run();