using System.Runtime.CompilerServices;

namespace Application.Movie.Dtos;

public class GetMoviesFilter
{
    public int? Skip { get; set; } = 0;
    public int? Take { get; set; } = null; // null = без лимита
}