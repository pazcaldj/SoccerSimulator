using SoccerSimulator.Core.Repository.Entities;
using SoccerSimulator.Core.Services;
using System.Collections.Frozen;

namespace SoccerSimulator.Tests;

public class GroupStageServiceTests
{
    private static readonly int FixedSeed = 123;

    [Fact]
    public void When_GenerateGroupMatches_Should_Creates6MatchesFor4Teams()
    {
        // Arrange
        var matchSimulator = new MatchSimulator(FixedSeed);
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
        Assert.Equal(6, matches.Count);
        Assert.All(matches, m => Assert.Equal(MatchStatus.Completed, m.Status));
    }

    [Fact]
    public void When_CalculateStandings_Should_SortsTeamsByPoints()
    {
        // Arrange
        var service = new GroupStageService(new MatchSimulator());
        var teamA = new Team(Guid.NewGuid(), "Team A", [new("Player 1", 75)]);
        var teamB = new Team(Guid.NewGuid(), "Team B", [new("Player 2", 70)]);
        
        var matches = new List<Match>
        {
            new(Guid.NewGuid(), teamA, teamB, 3, 1, DateTime.Now, MatchStatus.Completed)
        };

        // Act
        var standings = service.CalculateStandings(matches, [teamA, teamB]);

        // Assert
        Assert.Equal(2, standings.Count);
        
        Assert.Equal(teamA, standings[0].Team); 
        Assert.Equal(3, standings[0].Points);

        Assert.Equal(teamB, standings[1].Team);
        Assert.Equal(0, standings[1].Points);
    }
}
