using SoccerSimulator.Core.Repository.Entities;

namespace SoccerSimulator.Core.Services;

public interface ITournamentService
{
    TournamentResult SimulateGroupStage(Poule poule);
}

public class TournamentService : ITournamentService
{
    private readonly IGroupStageService _groupStageService;

    public TournamentService(IGroupStageService groupStageService)
    {
        _groupStageService = groupStageService;
    }

    public TournamentResult SimulateGroupStage(Poule poule)
    {
        var matches = _groupStageService.GenerateGroupMatches(poule);
        var teams = poule.Teams.ToList();
        var standings = _groupStageService.CalculateStandings(matches, teams);
        var qualifiedTeams = _groupStageService.GetQualifiedTeams(standings);

        return new TournamentResult(poule, matches, standings, qualifiedTeams);
    }
}