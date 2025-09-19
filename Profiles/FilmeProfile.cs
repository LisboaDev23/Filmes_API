using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data.Models;
namespace FilmesApi.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<Filme, ReadFilmeDto>();
        CreateMap<IEnumerable<Filme>, IEnumerable<ReadFilmeDto>>();
    }
}
