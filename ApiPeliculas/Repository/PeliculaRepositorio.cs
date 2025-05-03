using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repository;

public class PeliculaRepositorio : IPeliculaRepositorio
{
    private readonly ApplicationDbContext _bd;

    public PeliculaRepositorio(ApplicationDbContext bd)
    {
        _bd = bd;
    }

    public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
    {
        return _bd.Peliculas.Include(c => c.Categoria).Where(c => c.categoriaId == categoriaId).ToList();
    }

    public IEnumerable<Pelicula> BuscarPelicula(string nombre)
    {
        IQueryable<Pelicula> query = _bd.Peliculas;
        if (!string.IsNullOrEmpty(nombre))
        {
            query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));
        }

        return query.ToList();
    }


    public bool ActualizarPelicula(Pelicula pelicula)
    {
        pelicula.FechaCreacion = DateTime.Now;
        // Arreglar problema del PUT
        var peliculaExistente = _bd.Peliculas.Find(pelicula.Id);
        if (peliculaExistente != null)
        {
            _bd.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
        }
        else
        {
            _bd.Peliculas.Update(pelicula);
        }

        return Guardar();
    }

    public bool BorrarPelicula(Pelicula pelicula)
    {
        _bd.Peliculas.Remove(pelicula);
        return Guardar();
    }

    public bool CrearPelicula(Pelicula pelicula)
    {
        pelicula.FechaCreacion = DateTime.Now;
        _bd.Peliculas.Add(pelicula);
        return Guardar();
    }

    public bool ExistePelicula(int id)
    {
        return _bd.Peliculas.Any(p => p.Id == id);
    }

    public bool ExistePelicula(string nombre)
    {
        return _bd.Peliculas.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
    }

    public Pelicula GetPelicula(int peliculaId)
    {
        return _bd.Peliculas.FirstOrDefault(c => c.Id == peliculaId);
    }

    public ICollection<Pelicula> GetPeliculas()
    {
        return _bd.Peliculas.OrderBy(c => c.Nombre).ToList();
    }

    public bool Guardar()
    {
        return _bd.SaveChanges() >= 0;
    }
}