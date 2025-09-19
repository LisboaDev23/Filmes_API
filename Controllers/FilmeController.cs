using AutoMapper;
using FilmesApi.Data.Context;
using FilmesApi.Data.Dtos;
using FilmesApi.Data.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        if (filmes.Count() == 0)
        {
            _logger.LogInformation("Nenhum filme cadastrado no banco!");
            return NoContent();
        }

        var filmesMapeados = _mapper.Map<List<ReadFilmeDto>>(filmes);

        return Ok(filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult? BuscarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null)
        {
            _logger.LogError($"Filme não encontrado pelo ID fornecido!{id}");
            return NotFound();
        }

        var filmeMapeado = _mapper.Map<ReadFilmeDto>(filme);

        _logger.LogInformation($"Filme encontrado pelo id:{id}.");

        return Ok(filmeMapeado);
    }

    [HttpPut("atualizar-filme/{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            _logger.LogError($"Filme não encontrado pelo id forncecido. ID: {id}");
            return NotFound($"Filme não encontrado pelo id forncecido. ID: {id}");
        }
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return Ok("Filme atualizado com sucesso!");
    }

    [HttpPatch]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            _logger.LogError($"Filme não encontrado pelo id forncecido. ID: {id}");
            return NotFound($"Filme não encontrado pelo id forncecido. ID: {id}");
        }

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme); 

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if(!TryValidateModel(filmeParaAtualizar))
        {
            _logger.LogError("Ocorreu um problema ao validar os dados, verifique os dados informados e tente novamente!");
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        _logger.LogInformation($"Filme {filme.Titulo} atualizado com sucesso.");
        return NoContent();
    }

    [HttpDelete("deletar-filme/{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null)
        {
            _logger.LogError($"Filme não encontrado pelo id forncecido. ID: {id}");
            return NotFound($"Filme não encontrado pelo id forncecido. ID: {id}");
        }
        _context.Filmes.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
