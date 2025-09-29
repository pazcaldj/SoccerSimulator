using SoccerSimulator.Core.Repository.Entities;

namespace SoccerSimulator.Core.Services;
public interface IGroupStageService
{
    List<Match> GenerateGroupMatches(Poule poule);
    List<TeamStanding> CalculateStandings(List<Match> matches, List<Team> teams);
    List<Team> GetQualifiedTeams(List<TeamStanding> standings, int qualificationSpots = 2);
}

public class GroupStageService : IGroupStageService
{
    private readonly IMatchSimulator _matchSimulator;

    public GroupStageService(IMatchSimulator matchSimulator)
    {
        _matchSimulator = matchSimulator;
    }

    public List<Match> GenerateGroupMatches(Poule poule)
    {
        var teams = poule.Teams.ToList();
        var matches = new List<Match>();
        var matchDate = DateTime.Today;

        // Make all machted to be played
        for (int i = 0; i < teams.Count; i++)
        {
            for (int j = i + 1; j < teams.Count; j++)
            {
                var match = _matchSimulator.SimulateMatch(teams[i], teams[j], matchDate);
                matches.Add(match);
                matchDate = matchDate.AddDays(1);
            }
        }

        return matches;
    }

    public List<TeamStanding> CalculateStandings(List<Match> matches, List<Team> teams)
    {
        var standings = teams.ToDictionary(t => t.Id, t => TeamStanding.CreateEmpty(t));

        foreach (var match in matches.Where(m => m.Status == MatchStatus.Completed))
        {
            standings[match.HomeTeam.Id] = UpdateStanding(standings[match.HomeTeam.Id], match, true);
            standings[match.VisitTeam.Id] = UpdateStanding(standings[match.VisitTeam.Id], match, false);
        }

        return SortStandings(standings.Values.ToList());
    }

    public List<Team> GetQualifiedTeams(List<TeamStanding> standings, int qualificationSpots = 2)
    {
        return standings
            .Take(qualificationSpots)
            .Select(s => s.Team)
            .ToList();
    }

    private static TeamStanding UpdateStanding(TeamStanding standing, Match match, bool isHome)
    {
        var teamScore = isHome ? match.HomeScore : match.VisitScore;
        var visitScore = isHome ? match.VisitScore : match.HomeScore;

        return standing with
        {
            MatchesPlayed = standing.MatchesPlayed + 1,
            GoalsFor = standing.GoalsFor + teamScore,
            GoalsAgainst = standing.GoalsAgainst + visitScore,
            Wins = CalculateWins(standing, teamScore, visitScore),
            Draws = CalculateDraws(standing, teamScore, visitScore),
            Losses = CalculateLosses(standing, teamScore, visitScore),
            Points = standing.Points + CalculateMatchPoints(teamScore, visitScore)
        };
    }

    private static int CalculateLosses(TeamStanding standing, int teamScore, int visitScore)
    {
        return teamScore < visitScore ? standing.Losses + 1 : standing.Losses;
    }

    private static int CalculateDraws(TeamStanding standing, int teamScore, int visitScore)
    {
        return teamScore == visitScore ? standing.Draws + 1 : standing.Draws;
    }

    private static int CalculateWins(TeamStanding standing, int teamScore, int visitScore)
    {
        return teamScore > visitScore ? standing.Wins + 1 : standing.Wins;
    }

    private static int CalculateMatchPoints(int teamScore, int visitScore)
    {
        if (teamScore == visitScore)
        {
            return 1;
        }

        if (teamScore > visitScore)
        {
            return 3;
        }

        return 0;
    }

    private static List<TeamStanding> SortStandings(List<TeamStanding> standings)
    {
        return standings.OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor)
            .ThenBy(s => s.GoalsAgainst)
            .ToList();
    }
}