using Microsoft.EntityFrameworkCore;
using RoomManagerAPI.Data;
using RoomManagerAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=salas.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

app.MapGet("/salas", async (AppDbContext db) =>
{
    return await db.SalasReuniao.ToListAsync();
});

app.MapPost("/salas", async (
    AppDbContext db,
    SalaReuniao sala
) =>
{
    db.SalasReuniao.Add(sala);

    await db.SaveChangesAsync();

    return Results.Created($"/salas/{sala.Id}", sala);
});

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
});

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
});

app.Run();