using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private ILogger<FilmeController> _logger;
    private static int id = 0;

    public FilmeController(ILogger<FilmeController> logger)
    {
        _logger = logger;
    }

    [HttpPost("adicionar-filme")]
    public IActionResult AdicionaFilme([FromBody] Filme filme)
    {
        filme.Id = ++id;
        filmes.Add(filme);
        if (!filmes.Contains(filme))
        {
            _logger.LogError($"Erro ao adicionar o filme {filme.Titulo}.");
            return BadRequest();
        }
        _logger.LogInformation($"Filme {filme.Titulo} adicionado com sucesso.");
        return CreatedAtAction(nameof(BuscarFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet("listar-filmes")]
    public IActionResult ListarFilmes([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        if (filmes.Count() == 0) return NoContent();
        return Ok(filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult? BuscarFilmePorId(int id)
    {
        var filme = filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }
}
