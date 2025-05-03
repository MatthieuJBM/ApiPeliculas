using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    // Aquí pasar todas las entidades (modelos)
    public DbSet<Categoria> Categorias { get; set; }
    
    public DbSet<Pelicula> Peliculas { get; set; }
}