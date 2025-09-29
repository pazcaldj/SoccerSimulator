using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;
using System.Collections.Frozen;

namespace SoccerSimulator.Tests;

public class MatchSimulatorTests
{
    [Fact]
    public void SimulateMatch_ReturnsCompletedMatch()
    {
        // Arrange
        var simulator = new MatchSimulator(123); // Use seed for deterministic results
        var homeTeam = new Team(Guid.NewGuid(), "Team A", [new("Player 1", 75), new("Player 2", 80)]);
        var awayTeam = new Team(Guid.NewGuid(), "Team B", [new("Player 3", 60), new("Player 4", 65)]);

        // Act
        var result = simulator.SimulateMatch(homeTeam, awayTeam);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(MatchStatus.Completed, result.Status);
        Assert.Equal(homeTeam, result.HomeTeam);
        Assert.Equal(awayTeam, result.VisitTeam);
        Assert.True(result.HomeScore >= 0);
        Assert.True(result.VisitScore >= 0);
    }

    [Fact]
    public void StrongerTeam_ShouldWinMoreOften()
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

public class GroupStageServiceTests
{
    [Fact]
    public void GenerateGroupMatches_Creates6MatchesFor4Teams()
    {
        // Arrange
        var matchSimulator = new MatchSimulator(123);
        var service = new GroupStageService(matchSimulator);
        var teams = new List<Team>
        {
            new(Guid.NewGuid(), "Team A", [new("Player 1", 75)]),
            new(Guid.NewGuid(), "Team B", [new("Player 2", 70)]),
            new(Guid.NewGuid(), "Team C", [new("Player 3", 65)]),
            new(Guid.NewGuid(), "Team D", [new("Player 4", 60)])
        };
        var poule = new Poule(PouleName.A, teams.ToFrozenSet());

        // Act
        var matches = service.GenerateGroupMatches(poule);

        // Assert
        Assert.Equal(6, matches.Count); // C(4,2) = 6 matches
        Assert.All(matches, m => Assert.Equal(MatchStatus.Completed, m.Status));
    }

    [Fact]
    public void CalculateStandings_SortsTeamsByPoints()
    {
        // Arrange
        var service = new GroupStageService(new MatchSimulator());
        var teamA = new Team(Guid.NewGuid(), "Team A", [new("Player 1", 75)]);
        var teamB = new Team(Guid.NewGuid(), "Team B", [new("Player 2", 70)]);
        
        var matches = new List<Match>
        {
            new(Guid.NewGuid(), teamA, teamB, 3, 1, DateTime.Now, MatchStatus.Completed) // A wins
        };

        // Act
        var standings = service.CalculateStandings(matches, [teamA, teamB]);

        // Assert
        Assert.Equal(2, standings.Count);
        Assert.Equal(teamA, standings[0].Team); // A should be first
        Assert.Equal(3, standings[0].Points); // 3 points for win
        Assert.Equal(teamB, standings[1].Team); // B should be second
        Assert.Equal(0, standings[1].Points); // 0 points for loss
    }
}
