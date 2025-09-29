using SoccerSimulator.Core.Repository.Entities;

namespace SoccerSimulator.Core.Services;

public interface IMatchSimulator
{
    Match SimulateMatch(Team home, Team visitor);
    Match SimulateMatch(Team home, Team visitor, DateTime matchDateTime);
}

public class MatchSimulator : IMatchSimulator
{
    private static readonly double HomeMorale = 1.1;
    private static readonly double GoalCap = 10;
    private static readonly double BaseExpectedGoal = 1;

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
        var homeStrength = CalculateTeamStrength(home) * HomeMorale;
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

    private static double CalculateExpectedGoals(double attackingStrength, double defendingStrength)
    {
        var strengthRatio = attackingStrength / Math.Max(defendingStrength, 1.0);
        return BaseExpectedGoal * strengthRatio;
    }

    private int GenerateGoals(double expectedGoals)
    {
        var goals = 0;
        var probability = Math.Exp(-expectedGoals);
        var cumulativeProbability = probability;
        var randomValue = _random.NextDouble();

        while (randomValue > cumulativeProbability && goals < GoalCap)
        {
            goals++;
            probability *= expectedGoals / goals;
            cumulativeProbability += probability;
        }

        return goals;
    }
}