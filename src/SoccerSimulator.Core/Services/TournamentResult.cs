using SoccerSimulator.Core.Repository.Entities;

namespace SoccerSimulator.Core.Services;

public record TournamentResult(
    Poule Poule,
    List<Match> Matches,
    List<TeamStanding> Standings,
    List<Team> QualifiedTeams
);