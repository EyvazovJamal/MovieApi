using Newtonsoft.Json;
using Domain.Common;
using Marten.Events.CodeGeneration;
using SharedKernel.Contracts;
using SharedKernel.Movie;

namespace Domain.Movie;

public class Movie :AggregateRoot
{
    public bool Adult { get; private set; }
    public string? BackdropPath { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string OriginalLanguage { get; private set; } = string.Empty;
    public string OriginalTitle { get; private set; } = string.Empty;
    public string? Overview { get; private set; }
    public string? PosterPath { get; private set; }
    public DateTimeOffset? ReleaseDate { get; private set; }
    public double VoteAverage { get; private set; }
    public int VoteCount { get; private set; }
    public int Runtime { get;private set; }
    
    [JsonConstructor]
    internal Movie(Guid id,
        bool adult, string? backdropPath, string title, string originalLanguage, 
        string originalTitle, string? overview, string? posterPath, 
        DateTimeOffset? releaseDate, double voteAverage, int voteCount,int runtime)
    {
        Id = id;
        Adult = adult;
        BackdropPath = backdropPath;
        Title = title;
        OriginalLanguage = originalLanguage;
        OriginalTitle = originalTitle;
        Overview = overview;
        PosterPath = posterPath;
        ReleaseDate = releaseDate;
        VoteAverage = voteAverage;
        VoteCount = voteCount;
        Runtime = runtime;
    }
    private Movie(Guid id,
        bool adult, string? backdropPath, string title, string originalLanguage, 
        string originalTitle, string? overview, string? posterPath, 
        DateTimeOffset? releaseDate, double voteAverage, int voteCount,int runtime,bool? t)
    
    {
        Id = id;
        Adult = adult;
        BackdropPath = backdropPath;
        Title = title;
        OriginalLanguage = originalLanguage;
        OriginalTitle = originalTitle;
        Overview = overview;
        PosterPath = posterPath;
        ReleaseDate = releaseDate;
        VoteAverage = voteAverage;
        VoteCount = voteCount;
        Runtime = runtime;

        AddDomainEvent(new MovieAddedToCinemaEvent(id,
            adult,
            backdropPath,
            title,
            originalLanguage,
            originalTitle,
            overview,
            posterPath,
            releaseDate,
            voteAverage,
            voteCount,
            runtime));
    }
    [MartenIgnore]
    public static Movie Create(
        Guid id,
        bool adult, string? backdropPath, string title, string originalLanguage, 
        string originalTitle, string? overview, string? posterPath, 
        DateTimeOffset? releaseDate, double voteAverage, int voteCount,int runtime)
    {
        return new Movie(
            id,
            adult, backdropPath, title, originalLanguage, 
            originalTitle, overview, posterPath, releaseDate, 
            voteAverage, voteCount,runtime,true
        );
    }    
    public void Apply(MovieAddedToCinemaEvent @event)
    {
        // Вызываем ваш уже готовый приватный метод сборки данных
        InternalApply(@event);
    }
    public override void ApplyEvent(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case MovieAddedToCinemaEvent e:
                InternalApply(e);
                break;
        }
    }

    private void InternalApply(MovieAddedToCinemaEvent e)
    {
        Id = e.MovieId;
        Adult = e.Adult;
        BackdropPath = e.BackdropPath;
        Title = e.Title;
        OriginalLanguage = e.OriginalLanguage;
        OriginalTitle = e.OriginalTitle;
        Overview = e.Overview;
        PosterPath = e.PosterPath;
        ReleaseDate = e.ReleaseDate;
        VoteAverage = e.VoteAverage;
        VoteCount = e.VoteCount;
        Runtime = e.Runtime;
    }

}