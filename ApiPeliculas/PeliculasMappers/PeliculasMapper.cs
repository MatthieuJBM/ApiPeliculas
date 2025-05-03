using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper;

public class PeliculasMapper : Profile
{
    public PeliculasMapper()
    {
        // ReverseMap hace que se comunica en ambas direcciones.
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaDto>().ReverseMap();
        CreateMap<Pelicula, CrearPeliculaDto>().ReverseMap();
    }
}