namespace SoccerSimulator.Core.Repository.Entities;

public record Match(
    Guid Id,
    Team HomeTeam,
    Team VisitTeam,
    int HomeScore,
    int VisitScore,
    DateTime MatchDateTime,
    MatchStatus Status = MatchStatus.Scheduled
);

public enum MatchStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled
}