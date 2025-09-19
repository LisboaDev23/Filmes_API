using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private ILogger<FilmeController> _logger;

    public FilmeController(ILogger<FilmeController> logger, FilmeContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("adicionar-filme")]
    public IActionResult AdicionaFilme([FromBody] Filme filme)
    {
        _context.Filmes.Add(filme);
        _context.SaveChanges();

        _logger.LogInformation($"Filme {filme.Titulo} adicionado com sucesso.");

        return CreatedAtAction(nameof(BuscarFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet("listar-filmes")]
    public IActionResult ListarFilmes([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var filmes = _context.Filmes;

        if (filmes.Count() == 0) return NoContent();

        return Ok(filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult? BuscarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        return Ok(filme);
    }
}
