using FilmesApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Data.Context;

public class FilmeContext : DbContext
{
    public FilmeContext(DbContextOptions<FilmeContext> options)
        : base(options)
    {

    }

    public DbSet<Filme> Filmes { get; set; }

}
