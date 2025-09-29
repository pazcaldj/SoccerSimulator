using SoccerSimulator.Core;
using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;

namespace SoccerSimulator.ConsoleApp;

public class ConsoleDisplayService : IDisplayResults
{
    public void DisplayTournamentResult(TournamentResult result)
    {
        Console.WriteLine($"=== GROUP {result.Poule.PouleName} TOURNAMENT RESULTS ===");
        Console.WriteLine();

        DisplayMatches(result.Matches);
        Console.WriteLine();

        DisplayStandings(result.Standings);
        Console.WriteLine();
    }

    public void DisplayMatches(List<Match> matches)
    {
        Console.WriteLine("MATCH RESULTS:");
        Console.WriteLine("=" + new string('=', 60));

        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            Console.WriteLine($"Round {i + 1}: {match.HomeTeam.Name,-25} {match.HomeScore}-{match.VisitScore} {match.VisitTeam.Name}");
        }
    }

    public void DisplayStandings(List<TeamStanding> standings)
    {
        Console.WriteLine("GROUP STANDINGS:");
        Console.WriteLine("=" + new string('=', 80));
        Console.WriteLine($"{"Pos"} {"Team",-21} {"MP",-3} {"W",-3} {"D",-3} {"L",-3} {"GF",-3} {"GA",-3} {"GD",-4} {"Pts",-3}");
        Console.WriteLine(new string('-', 80));

        for (int i = 0; i < standings.Count; i++)
        {
            var standing = standings[i];

            Console.WriteLine($"{standing.Team.Name,-25} " +
                              $"{standing.MatchesPlayed,-3} {standing.Wins,-3} {standing.Draws,-3} {standing.Losses,-3} " +
                              $"{standing.GoalsFor,-3} {standing.GoalsAgainst,-3} {standing.GoalDifference,-4} {standing.Points,-3}");
        }
    }
}