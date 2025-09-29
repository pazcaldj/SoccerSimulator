using SoccerSimulator.Core.Repository.Entities;

namespace SoccerSimulator.Core;

public interface IDisplayResults
{
    void DisplayMatches(List<Match> matches);
    void DisplayStandings(List<TeamStanding> standings);
    void DisplayTournamentResult(TournamentResult result);
    void DisplayQualifiedTeams(List<Team> qualifiedTeams);
}