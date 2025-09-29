using SoccerSimulator.Core.Repository.Entities;

namespace SoccerSimulator.Core.Services;

public interface IMatchSimulator
{
    Match SimulateMatch(Team home, Team visitor);
    Match SimulateMatch(Team home, Team visitor, DateTime matchDateTime);
}

public class MatchSimulator : IMatchSimulator
{
    private static readonly double homeMorale = 1.1;
    private static readonly double goalCap = 10;

    private readonly Random _random;

    public MatchSimulator()
    {
        _random = new Random();
    }

    public MatchSimulator(int seed)
    {
        _random = new Random(seed);
    }

    public Match SimulateMatch(Team home, Team visitor)
    {
        return SimulateMatch(home, visitor, DateTime.Now);
    }

    public Match SimulateMatch(Team home, Team visitor, DateTime matchDateTime)
    {
        var homeStrength = CalculateTeamStrength(home) * homeMorale;
        var visitorStrength = CalculateTeamStrength(visitor);

        var homeExpectedGoals = CalculateExpectedGoals(homeStrength, visitorStrength);
        var visitorExpectedGoals = CalculateExpectedGoals(visitorStrength, homeStrength);

        var homeScore = GenerateGoals(homeExpectedGoals);
        var visitorScore = GenerateGoals(visitorExpectedGoals);

        return new Match(
            Guid.NewGuid(),
            home,
            visitor,
            homeScore,
            visitorScore,
            matchDateTime,
            MatchStatus.Completed
        );
    }

    private static double CalculateTeamStrength(Team team)
    {
        if (team.Players.Count == 0)
        {
            return 50.0;
        }

        return team.Players.Average(p => p.Strength);
    }

    private double CalculateExpectedGoals(double attackingStrength, double defendingStrength)
    {
        // Base expected goals adjusted by strength ratio
        var strengthRatio = attackingStrength / Math.Max(defendingStrength, 1.0);
        var baseExpectedGoals = 1.5; // Average goals per team in a match

        return baseExpectedGoals * strengthRatio;
    }

    private int GenerateGoals(double expectedGoals)
    {
        var goals = 0;
        var probability = Math.Exp(-expectedGoals);
        var cumulativeProbability = probability;
        var randomValue = _random.NextDouble();

        while (randomValue > cumulativeProbability && goals < goalCap)
        {
            goals++;
            probability *= expectedGoals / goals;
            cumulativeProbability += probability;
        }

        return goals;
    }
}