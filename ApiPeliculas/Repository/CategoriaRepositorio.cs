using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Repository;

public class CategoriaRepositorio : ICategoriaRepositorio
{
    // Ese ApplicationDbContext contiene el acceso a cada una de las entidades.
    // En este caso por el momento solamente de la categoria.
    private readonly ApplicationDbContext _bd;

    public CategoriaRepositorio(ApplicationDbContext bd)
    {
        _bd = bd;
    }

    public bool ActualizarCategoria(Categoria categoria)
    {
        categoria.FechaCreacion = DateTime.Now;
        // Arreglar problema del PUT
        var categoriaExistente = _bd.Categorias.Find(categoria.Id);
        if (categoriaExistente != null)
        {
            _bd.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
        }
        else
        {
            _bd.Categorias.Update(categoria);
        }
        
        return Guardar();
    }

    public bool BorrarCategoria(Categoria categoria)
    {
        _bd.Categorias.Remove(categoria);
        return Guardar();
    }

    public bool CreateCategoria(Categoria categoria)
    {
        categoria.FechaCreacion = DateTime.Now;
        _bd.Categorias.Add(categoria);
        return Guardar();
    }

    public bool ExisteCategoria(int id)
    {
        return _bd.Categorias.Any(c => c.Id == id);
    }

    public bool ExisteCategoria(string nombre)
    {
        return _bd.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
    }

    public Categoria GetCategoria(int categoriaId)
    {
        return _bd.Categorias.FirstOrDefault(c => c.Id == categoriaId);
    }

    public ICollection<Categoria> GetCategorias()
    {
        return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
    }

    public bool Guardar()
    {
        return _bd.SaveChanges() >= 0;
    }
}