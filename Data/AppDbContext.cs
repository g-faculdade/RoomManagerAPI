using Microsoft.EntityFrameworkCore;
using RoomManagerAPI.Models;

namespace RoomManagerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SalaReuniao> SalasReuniao => Set<SalaReuniao>();
}