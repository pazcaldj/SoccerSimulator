using Microsoft.Extensions.DependencyInjection;
using SoccerSimulator.Core.Configuration;
using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;
using System.Collections.Frozen;

namespace SoccerSimulator.IntegrationTests;

public class SimulateGroupStageTests
{
    [Fact]
    public void Should_CompleteWorkflow_When_SimulatesTournamentSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSoccerSimulatorServices();
        var provider = services.BuildServiceProvider();

        var tournamentService = provider.GetRequiredService<ITournamentService>();

        var teams = new List<Team>
        {
            new(Guid.NewGuid(), "Team A", [new("Player 1", 80)]),
            new(Guid.NewGuid(), "Team B", [new("Player 2", 70)]),
            new(Guid.NewGuid(), "Team C", [new("Player 3", 60)]),
            new(Guid.NewGuid(), "Team D", [new("Player 4", 50)])
        };
        var poule = new Poule(PouleName.A, teams.ToFrozenSet());

        // Act
        var result = tournamentService.SimulateGroupStage(poule);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Matches.Count);
        Assert.Equal(4, result.Standings.Count);
        Assert.Equal(2, result.QualifiedTeams.Count);

        Assert.All(result.Matches, m => Assert.Equal(MatchStatus.Completed, m.Status));

        AssertStandingsAreSorted(result);

        Assert.Equal(result.Standings[0].Team, result.QualifiedTeams[0]);
        Assert.Equal(result.Standings[1].Team, result.QualifiedTeams[1]);
    }

    [Fact]
    public void When_MultipleSimulations_Should_SetStrongerTeamAsQualifiedMoreOften()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSoccerSimulatorServices();
        var provider = services.BuildServiceProvider();

        var tournamentService = provider.GetRequiredService<ITournamentService>();

        var strongTeam = new Team(Guid.NewGuid(), "Strong Team", [new("Star", 90)]);
        var weakTeam = new Team(Guid.NewGuid(), "Weak Team", [new("Rookie", 40)]);
        var mediumTeam1 = new Team(Guid.NewGuid(), "Medium Team 1", [new("Average", 65)]);
        var mediumTeam2 = new Team(Guid.NewGuid(), "Medium Team 2", [new("Ok-ish", 65)]);

        var teams = new List<Team> { strongTeam, mediumTeam1, mediumTeam2, weakTeam };
        var poule = new Poule(PouleName.A, teams.ToFrozenSet());

        int strongTeamQualifications = 0;
        int weakTeamQualifications = 0;
        int simulations = 100;

        for (int i = 0; i < simulations; i++)
        {
            var result = tournamentService.SimulateGroupStage(poule);

            if (result.QualifiedTeams.Contains(strongTeam))
            {
                strongTeamQualifications++;
            }
            if (result.QualifiedTeams.Contains(weakTeam))
            {
                weakTeamQualifications++;
            }
        }

        Assert.True(strongTeamQualifications > weakTeamQualifications);
    }

    private static void AssertStandingsAreSorted(TournamentResult result)
    {
        for (int i = 0; i < result.Standings.Count - 1; i++)
        {
            var current = result.Standings[i];
            var next = result.Standings[i + 1];

            Assert.True(current.Points >= next.Points ||
                       current.Points == next.Points && current.GoalDifference >= next.GoalDifference);
        }
    }
}