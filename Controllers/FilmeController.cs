using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private ILogger<FilmeController> _logger;

    public FilmeController(ILogger<FilmeController> logger)
    {
        _logger = logger;
    }

    [HttpPost("/adicionar-filme")]
    public void AdicionaFilme([FromBody] Filme filme)
    {
        filmes.Add(filme);
        _logger.LogInformation($"Filme {filme.Titulo} adicionado com sucesso.");
    }
}
