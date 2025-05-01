using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository;

public interface ICategoriaRepositorio
{
    ICollection<Categoria> GetCategorias();
    Categoria GetCategoria(int categoriaId);
    bool ExisteCategoria(int id);
    bool ExisteCategoria(string nombre);
    bool CreateCategoria(Categoria categoria);
    bool ActualizarCategoria(Categoria categoria);
    bool BorrarCategoria(Categoria categoria);
    bool Guardar();
}