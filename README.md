# Soccer Group Stage Simulator

A .NET application that simulates soccer group stage tournaments with realistic team strength-based outcomes using clean architecture principles.

## Overview

This application simulates a group stage tournament with 4 teams playing in a round-robin format (6 matches total). The simulation takes into account team strength based on player ratings, ensuring that stronger teams have better chances of winning and advancing to the knockout stage.

## Project Structure

The solution is organized into the following projects:

```
SoccerSimulator/
├── src/
│   └── SoccerSimulator.Core/           # Core business logic and entities
│       ├── Repository/Entities/        # Domain entities and models
│       ├── Services/                   # Business logic services
│       └── Configuration/              # Dependency injection setup
├── dev/
│   └── SoccerSimulator.ConsoleApp/     # Console application
└── tests/
    ├── SoccerSimulator.Tests/          # Unit tests
    └── SoccerSimulator.IntegrationTests/ # Integration tests
```

## Key Components

### Entities
- `Team`: Represents a soccer team with players and calculated team strength
- `Player`: Individual player with name and strength rating
- `Match`: Represents a match with teams, scores, and status
- `TeamStanding`: Tracks team performance (wins, losses, points, goals, etc.)
- `Poule`: Represents a group of teams
- `TournamentResult`: Contains complete tournament outcome data

### Services
- `IMatchSimulator`: Simulates individual matches based on team strength
- `IGroupStageService`: Generates group matches and calculates standings
- `ITournamentService`: Orchestrates the entire tournament simulation
- `IDisplayResults`: Handles output formatting and display

## Features

### Match Simulation
- **Strength-based outcomes**: Teams with higher average player strength have better chances of winning
- **Home advantage**: 10% boost for the home team
- **Realistic scoring**: Uses statistical distribution to generate believable scorelines
- **Poisson distribution**: Approximates real-world goal scoring patterns

### Tournament Rules
- **Round-robin format**: Each team plays every other team once (6 matches for 4 teams)
- **Standard points system**: 3 points for win, 1 for draw, 0 for loss
- **FIFA ranking criteria**: Teams are sorted by:
  1. Points
  2. Goal difference
  3. Goals scored
  4. Goals conceded
  5. Head-to-head results (basic implementation)
  6. Team name (final tie-breaker)

### Output Features
- **Single simulation**: Detailed match results, standings with qualification indicators, and qualified teams
- **Statistical analysis**: 1000-run simulation showing:
  - Qualification percentages by team strength
  - Position distribution statistics
  - Demonstration that stronger teams consistently perform better

## Sample Output

### Single Simulation
```
=== GROUP A TOURNAMENT RESULTS ===

MATCH RESULTS:
=============================================================
Round 1: Manchester United         2-1 Newcastle United
Round 2: Manchester United         1-0 Brighton & Hove
Round 3: Manchester United         3-0 Luton Town
...

GROUP STANDINGS:
=================================================================================
Pos Team                      MP  W   D   L   GF  GA  GD   Pts
--------------------------------------------------------------------------------
1*  Manchester United         3   3   0   0   6   1   5    9
2*  Brighton & Hove           3   2   0   1   5   1   4    6
3   Newcastle United          3   1   0   2   5   5   0    3
4   Luton Town                3   0   0   3   2   11  -9   0

* = Qualified for knockout stage

TEAMS ADVANCING TO KNOCKOUT STAGE:
=========================================
1. Manchester United
2. Brighton & Hove
```

### Statistical Analysis
```
QUALIFICATION STATISTICS:
Team                      Qualified  Percentage Strength
Manchester United         814        81.4%      84.2
Newcastle United          641        64.1%      76.4
Brighton & Hove           400        40.0%      69.2
Luton Town                145        14.5%      56.0
```

## Running the Application

### Prerequisites
- .NET 9.0 SDK

### Commands
```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Run the console application
dotnet run --project dev/SoccerSimulator.ConsoleApp
```

## Testing

The solution includes comprehensive testing:

### Unit Tests
- Match simulation accuracy and determinism
- Tournament generation logic
- Standings calculation and sorting
- Statistical distribution verification

### Integration Tests
- Complete workflow testing
- Service integration validation
- Multiple simulation consistency checks

All tests use deterministic seeding to ensure consistent results while validating that the simulation produces statistically sound outcomes that favor stronger teams.