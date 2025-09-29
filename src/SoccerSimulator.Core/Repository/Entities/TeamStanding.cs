namespace SoccerSimulator.Core.Repository.Entities;

public record TeamStanding(
    Team Team,
    int MatchesPlayed,
    int Wins,
    int Draws,
    int Losses,
    int GoalsFor,
    int GoalsAgainst,
    int Points
)
{
    public int GoalDifference => GoalsFor - GoalsAgainst;
    
    public static TeamStanding CreateEmpty(Team team) => 
        new(team, 0, 0, 0, 0, 0, 0, 0);
}