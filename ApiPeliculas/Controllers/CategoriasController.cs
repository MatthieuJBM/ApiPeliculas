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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
}