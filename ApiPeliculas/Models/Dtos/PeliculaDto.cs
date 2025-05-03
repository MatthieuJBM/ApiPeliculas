namespace ApiPeliculas.Models.Dtos;

public class PeliculaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public int Duracion { get; set; }

    public string RutaImagen { get; set; }

    public enum TipoClasificaion
    {
        Siete,
        Trece,
        Dieciseis,
        Dieciocho
    }

    public TipoClasificaion Clasificaion { get; set; }
    public DateTime FechaCreacion { get; set; }

    // Relación con Categoría
    public int categoriaId { get; set; }
}