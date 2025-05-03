namespace ApiPeliculas.Models.Dtos;

public class CrearPeliculaDto
{
    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public int Duracion { get; set; }

    public string RutaImagen { get; set; }

    public enum CrearTipoClasificaion
    {
        Siete,
        Trece,
        Dieciseis,
        Dieciocho
    }

    public CrearTipoClasificaion Clasificaion { get; set; }

    // Relación con Categoría
    public int categoriaId { get; set; }
}