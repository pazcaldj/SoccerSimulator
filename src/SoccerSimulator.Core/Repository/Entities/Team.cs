namespace SoccerSimulator.Core.Repository.Entities;

public record Team (Guid Id, string Name, List<Player> Players)
{
    public double TeamStrength => Players.Count != 0 ? Players.Average(p => p.Strength) : 50.0;
}
