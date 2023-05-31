using Microsoft.EntityFrameworkCore;
using crud_wpf_mvvm_origin.Models;

namespace crud_wpf_mvvm_origin.Data;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("crud-wpf-mvvm-origin");
    }    
}