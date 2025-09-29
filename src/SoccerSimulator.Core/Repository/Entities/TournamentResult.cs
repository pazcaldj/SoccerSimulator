namespace SoccerSimulator.Core.Repository.Entities;

public record TournamentResult(
    Poule Poule,
    List<Match> Matches,
    List<TeamStanding> Standings,
    List<Team> QualifiedTeams
);