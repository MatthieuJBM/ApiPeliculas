using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos;

public class CategoriaDto
{
    public int Id { get; set; }

    // No lo hemos hecho en categoría directamente, porque allí no se lo está exponiendo.
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "La longitud del campo es {100} caracteres")]
    public string Nombre { get; set; }

    public DateTime FechaCreacion { get; set; }
}