using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers;

[Route("api/categorias")]
[ApiController]
public class CategoriasController : ControllerBase
{
    // Tenemos que crear toda la parte con el constructor e inyección de dependencias que nos va a permitir a obtener
    // los datos.
    private readonly ICategoriaRepositorio _ctRepo;
    private readonly IMapper _mapper;

    public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
    {
        _ctRepo = ctRepo;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetCategorias()
    {
        var listaCategorias = _ctRepo.GetCategorias();
        var listaCategoriasDto = new List<CategoriaDto>();
        foreach (var item in listaCategorias)
        {
            listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(item));
        }

        return Ok(listaCategoriasDto);
    }

    [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCategoria(int categoriaId)
    {
        var itemCategoria = _ctRepo.GetCategoria(categoriaId);

        if (itemCategoria == null)
        {
            return NotFound();
        }

        // Cada vez tenemos que exponer el DTO y no el nuestro modelo.
        var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
        return Ok(itemCategoriaDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (crearCategoriaDto == null)
        {
            return BadRequest(ModelState);
        }

        if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
        {
            ModelState.AddModelError("Nombre", $"La categoría ya existe");
            return StatusCode(404, ModelState);
        }

        var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

        if (!_ctRepo.CreateCategoria(categoria))
        {
            ModelState.AddModelError("", $"Algo salió mal guardando el registro {categoria.Nombre}");
            return StatusCode(404, ModelState);
        }

        return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
    }

    [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (categoriaDto == null || categoriaId != categoriaDto.Id)
        {
            return BadRequest(ModelState);
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        if (!_ctRepo.ActualizarCategoria(categoria))
        {
            ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}