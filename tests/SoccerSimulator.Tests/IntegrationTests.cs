using Microsoft.Extensions.DependencyInjection;
using SoccerSimulator.Core.Configuration;
using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;
using System.Collections.Frozen;

namespace SoccerSimulator.Tests;

public class IntegrationTests
{
    [Fact]
    public void CompleteWorkflow_SimulatesTournamentSuccessfully()
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
        Assert.Equal(6, result.Matches.Count); // 4 teams = 6 matches
        Assert.Equal(4, result.Standings.Count); // 4 teams in standings
        Assert.Equal(2, result.QualifiedTeams.Count); // 2 qualified teams
        Assert.All(result.Matches, m => Assert.Equal(MatchStatus.Completed, m.Status));
        
        // Verify standings are properly sorted (first team should have most points or better metrics)
        for (int i = 0; i < result.Standings.Count - 1; i++)
        {
            var current = result.Standings[i];
            var next = result.Standings[i + 1];
            
            // Current team should be better than or equal to next team
            Assert.True(current.Points >= next.Points ||
                       (current.Points == next.Points && current.GoalDifference >= next.GoalDifference));
        }
        
        // Qualified teams should be the top 2 from standings
        Assert.Equal(result.Standings[0].Team, result.QualifiedTeams[0]);
        Assert.Equal(result.Standings[1].Team, result.QualifiedTeams[1]);
    }

    [Fact]
    public void MultipleSimulations_ProduceConsistentResults()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSoccerSimulatorServices();
        var provider = services.BuildServiceProvider();

        var tournamentService = provider.GetRequiredService<ITournamentService>();

        var strongTeam = new Team(Guid.NewGuid(), "Strong Team", [new("Star", 90)]);
        var weakTeam = new Team(Guid.NewGuid(), "Weak Team", [new("Rookie", 40)]);
        var mediumTeam1 = new Team(Guid.NewGuid(), "Medium Team 1", [new("Average", 65)]);
        var mediumTeam2 = new Team(Guid.NewGuid(), "Medium Team 2", [new("Regular", 65)]);

        var teams = new List<Team> { strongTeam, mediumTeam1, mediumTeam2, weakTeam };
        var poule = new Poule(PouleName.A, teams.ToFrozenSet());

        // Act - run multiple simulations
        int strongTeamQualifications = 0;
        int weakTeamQualifications = 0;
        int simulations = 100;

        for (int i = 0; i < simulations; i++)
        {
            var result = tournamentService.SimulateGroupStage(poule);
            
            if (result.QualifiedTeams.Contains(strongTeam))
                strongTeamQualifications++;
            if (result.QualifiedTeams.Contains(weakTeam))
                weakTeamQualifications++;
        }

        // Assert - strong team should qualify more often than weak team
        Assert.True(strongTeamQualifications > weakTeamQualifications,
            $"Strong team qualified {strongTeamQualifications} times vs weak team {weakTeamQualifications} times");
        
        // Strong team should qualify in majority of simulations
        Assert.True(strongTeamQualifications > simulations * 0.6,
            $"Strong team should qualify in >60% of simulations, but qualified in {strongTeamQualifications}/{simulations}");
    }
}