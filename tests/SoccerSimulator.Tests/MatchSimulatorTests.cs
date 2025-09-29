using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;

namespace SoccerSimulator.Tests;

public class MatchSimulatorTests
{
    private static readonly int FixedSeed = 123;

    [Fact]
    public void When_SimulateMatch_Should_ReturnsCompletedMatch()
    {
        // Arrange
        var simulator = new MatchSimulator(FixedSeed);
        var home = new Team(Guid.NewGuid(), "Team A", [new("Player 1", 75), new("Player 2", 80)]);
        var visit = new Team(Guid.NewGuid(), "Team B", [new("Player 3", 60), new("Player 4", 65)]);

        // Act
        var result = simulator.SimulateMatch(home, visit);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(MatchStatus.Completed, result.Status);
        
        Assert.Equal(home, result.HomeTeam);
        Assert.Equal(visit, result.VisitTeam);
        
        Assert.True(result.HomeScore >= 0);
        Assert.True(result.VisitScore >= 0);
    }

    [Fact]
    public void When_StrongerTeam_Should_WinMoreOften()
    {
        // Arrange
        var simulator = new MatchSimulator(42);
        var strongTeam = new Team(Guid.NewGuid(), "Strong Team", [
            new("Star Player", 90), 
            new("Good Player", 85)
        ]);
        var weakTeam = new Team(Guid.NewGuid(), "Weak Team", [
            new("Average Player", 50), 
            new("Below Average", 45)
        ]);

        // Act - simulate many matches
        int strongTeamWins = 0;
        int totalMatches = 1000;

        for (int i = 0; i < totalMatches; i++)
        {
            var match = simulator.SimulateMatch(strongTeam, weakTeam);
            if (match.HomeScore > match.VisitScore)
                strongTeamWins++;
        }

        // Assert - stronger team should win more than 60% of matches
        double winPercentage = (double)strongTeamWins / totalMatches;
        Assert.True(winPercentage > 0.6, $"Strong team should win more than 60% of matches, but won {winPercentage:P}");
    }
}
