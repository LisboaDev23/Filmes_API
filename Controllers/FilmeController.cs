using AutoMapper;
using FilmesApi.Data.Context;
using FilmesApi.Data.Dtos;
using FilmesApi.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private ILogger<FilmeController> _logger;
    private IMapper _mapper;

    public FilmeController(ILogger<FilmeController> logger, FilmeContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("adicionar-filme")]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        
        Filme filme = _mapper.Map<Filme>(filmeDto);

        _context.Filmes.Add(filme);
        _context.SaveChanges();

        _logger.LogInformation($"Filme {filmeDto.Titulo} adicionado com sucesso.");

        return CreatedAtAction(nameof(BuscarFilmePorId), new { id = filme.Id }, filmeDto);
    }

    [HttpGet("listar-filmes")]
    public IActionResult ListarFilmes([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var filmes = _context.Filmes;

        if (filmes.Count() == 0) return NoContent();

        var filmesMapeados = _mapper.Map<IEnumerable<ReadFilmeDto>>(filmes);

        return Ok(filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult? BuscarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        var filmeMapeado = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeMapeado);
    }
}
