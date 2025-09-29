using SoccerSimulator.Core;
using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;
using System.Collections.Frozen;

namespace SoccerSimulator.ConsoleApp;

public class SoccerSimulatorApp
{
    private static readonly int MultipleSimulationsCount = 1000; 

    private readonly ITournamentService _tournamentService;
    private readonly IDisplayResults _displayService;

    public SoccerSimulatorApp(ITournamentService tournamentService, IDisplayResults displayService)
    {
        _tournamentService = tournamentService;
        _displayService = displayService;
    }

    public Task RunAsync()
    {
        Console.WriteLine("=== SOCCER GROUP STAGE SIMULATOR ===");
        Console.WriteLine();

        var poule = CreateSamplePoule();
        
        Console.WriteLine("Teams in Group A:");
        foreach (var team in poule.Teams.OrderBy(t => t.Name))
        {
            Console.WriteLine($"- {team.Name} (Team Strength: {team.TeamStrength:F1})");
        }
        Console.WriteLine();

       
        var result = _tournamentService.SimulateGroupStage(poule);
        _displayService.DisplayTournamentResult(result);

        // Run multiple simulations to show overall stats
        Console.WriteLine("\n" + new string('=', 80));
        Console.WriteLine("RUNNING 1000 SIMULATIONS TO SHOW TEAM STRENGTH IMPACT:");
        Console.WriteLine(new string('=', 80));
        
        RunMultipleSimulations(poule, MultipleSimulationsCount);
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
        
        return Task.CompletedTask;
    }

    private void RunMultipleSimulations(Poule poule, int simulationCount)
    {
        var qualificationStats = poule.Teams.ToDictionary(t => t.Name, t => 0);
        var positionStats = poule.Teams.ToDictionary(t => t.Name, t => new int[poule.Teams.Count]);

        for (int i = 0; i < simulationCount; i++)
        {
            var result = _tournamentService.SimulateGroupStage(poule);
            
            foreach (var qualifiedTeam in result.QualifiedTeams)
            {
                qualificationStats[qualifiedTeam.Name]++;
            }

            for (int pos = 0; pos < result.Standings.Count; pos++)
            {
                var teamName = result.Standings[pos].Team.Name;
                positionStats[teamName][pos]++;
            }
        }

        // Display results
        Console.WriteLine("QUALIFICATION STATS:");
        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"{"Team",-25} {"Qualified",-10} {"Percentage",-10} {"Strength",-8}");
        Console.WriteLine(new string('-', 60));

        var sortedTeams = poule.Teams.OrderByDescending(t => t.TeamStrength).ToList();
        foreach (var team in sortedTeams)
        {
            var qualifications = qualificationStats[team.Name];
            var percentage = (qualifications * 100.0) / simulationCount;
            Console.WriteLine($"{team.Name,-25} {qualifications,-10} {percentage:F1}%{"",-5} {team.TeamStrength:F1}");
        }

        Console.WriteLine();
        Console.WriteLine("POSITIONS OVERALL");
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"{"Team",-25} {"1st",-8} {"2nd",-8} {"3rd",-8} {"4th",-8} {"Strength",-8}");
        Console.WriteLine(new string('-', 80));

        foreach (var team in sortedTeams)
        {
            var positions = positionStats[team.Name];
            Console.WriteLine($"{team.Name,-25} " +
                $"{positions[0],-8} {positions[1],-8} {positions[2],-8} {positions[3],-8} " +
                $"{team.TeamStrength:F1}");
        }
    }

    private static Poule CreateSamplePoule()
    {
        return new Poule(
            PouleName.A,
            new List<Team>()
            {
                new(Guid.NewGuid(), "Manchester United", [
                    new("Marcus Rashford", 85),
                    new("Bruno Fernandes", 87),
                    new("Casemiro", 82),
                    new("Raphael Varane", 84),
                    new("David de Gea", 83)
                ]),
                
                new(Guid.NewGuid(), "Newcastle United", [
                    new("Alexander Isak", 78),
                    new("Bruno Guimaraes", 79),
                    new("Sven Botman", 76),
                    new("Nick Pope", 75),
                    new("Allan Saint-Maximin", 74)
                ]),
                
                new(Guid.NewGuid(), "Brighton & Hove", [
                    new("Kaoru Mitoma", 72),
                    new("Alexis Mac Allister", 70),
                    new("Lewis Dunk", 68),
                    new("Robert Sanchez", 69),
                    new("Solly March", 67)
                ]),
                
                new(Guid.NewGuid(), "Luton Town", [
                    new("Elijah Adebayo", 58),
                    new("Pelly Ruddock", 55),
                    new("Tom Lockyer", 57),
                    new("Thomas Kaminski", 56),
                    new("Alfie Doughty", 54)
                ])
            }.ToFrozenSet()
        );
    }
}
