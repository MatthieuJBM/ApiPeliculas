using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers;

[Route("api/peliculas")]
[ApiController]
public class PeliculasController : ControllerBase
{
    private readonly IPeliculaRepositorio _pelRepo;
    private readonly IMapper _mapper;

    public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
    {
        _pelRepo = pelRepo;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetPeliculas()
    {
        var listaPeliculas = _pelRepo.GetPeliculas();
        var listaPeliculasDto = new List<PeliculaDto>();
        foreach (var item in listaPeliculas)
        {
            listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(item));
        }

        return Ok(listaPeliculasDto);
    }

    [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPelicula(int peliculaId)
    {
        var itemPelicula = _pelRepo.GetPelicula(peliculaId);

        if (itemPelicula == null)
        {
            return NotFound();
        }

        var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
        return Ok(itemPeliculaDto);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PeliculaDto))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CrearPelicula([FromBody] CrearPeliculaDto crearPeliculaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (crearPeliculaDto == null)
        {
            return BadRequest(ModelState);
        }

        if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
        {
            ModelState.AddModelError("Nombre", $"La pelicula ya existe");
            return StatusCode(404, ModelState);
        }

        var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);

        if (!_pelRepo.CrearPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salió mal guardando el registro {pelicula.Nombre}");
            return StatusCode(404, ModelState);
        }

        return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
    }

    [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (peliculaDto == null || peliculaId != peliculaDto.Id)
        {
            return BadRequest(ModelState);
        }

        var peliculaExistente = _pelRepo.GetPelicula(peliculaId);

        if (peliculaExistente == null)
        {
            return NotFound($"No se encontró la pelicula con ID ${peliculaId}");
        }

        var pelicula = _mapper.Map<Pelicula>(peliculaDto);

        if (!_pelRepo.ActualizarPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salió mal actualizando el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult BorrarPelicula(int peliculaId)
    {
        if (!_pelRepo.ExistePelicula(peliculaId))
        {
            return NotFound();
        }

        var pelicula = _pelRepo.GetPelicula(peliculaId);

        if (!_pelRepo.BorrarPelicula(pelicula))
        {
            ModelState.AddModelError("", $"Algo salió mal borrano el registro {pelicula.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetPeliculasEnCategoria(int categoriaId)
    {
        var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);

        if (listaPeliculas == null)
        {
            return NotFound();
        }

        var itemPeliculas = new List<PeliculaDto>();
        foreach (var pelicula in listaPeliculas)
        {
            itemPeliculas.Add(_mapper.Map<PeliculaDto>(pelicula));
        }

        return Ok(itemPeliculas);
    }

    [HttpGet("BuscarPelicula")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult BuscarPelicula(string nombre)
    {
        try
        {
            var resultado = _pelRepo.BuscarPelicula(nombre);
            if (resultado.Any())
            {
                return Ok(_mapper.Map<List<PeliculaDto>>(resultado));
            }

            return NotFound();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación.");
        }
    }
}