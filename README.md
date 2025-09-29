# Soccer Group Stage Simulator

A .NET application that simulates soccer group stage tournaments with realistic team strength-based outcomes.

## Overview

This application simulates a group stage tournament with 4 teams playing in a round-robin format (6 matches total). The simulation takes into account team strength based on player ratings, ensuring that stronger teams have better chances of winning and advancing to the knockout stage.

## Architecture

The solution follows clean architecture principles with separation of concerns:

### Projects Structure

- **SoccerSimulator.Core**: Contains business logic, entities, and services
- **SoccerSimulator.Console**: Console application entry point
- **SoccerSimulator.Tests**: Unit tests for core functionality

### Key Components

#### Entities
- `Team`: Represents a soccer team with players and calculated team strength
- `Player`: Individual player with name and strength rating
- `Match`: Represents a match with teams, scores, and status
- `TeamStanding`: Tracks team performance (wins, losses, points, goals, etc.)
- `Poule`: Represents a group of teams

#### Services
- `IMatchSimulator`: Simulates individual matches based on team strength
- `IGroupStageService`: Generates group matches and calculates standings
- `ITournamentService`: Orchestrates the entire tournament simulation
- `IDisplayService`: Handles output formatting and display

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

### Statistical Analysis
- Single simulation with detailed match results and standings
- Multiple simulation analysis (1000 runs) showing:
  - Qualification percentages by team strength
  - Position distribution statistics
  - Demonstration that stronger teams consistently perform better

## Sample Output

The simulator shows that team strength properly influences results:

```
QUALIFICATION STATISTICS:
Team                      Qualified  Percentage Strength
Manchester United         807        80.7%      84.2
Newcastle United          656        65.6%      76.4
Brighton & Hove           412        41.2%      69.2
Luton Town                125        12.5%      56.0
```

## Technical Implementation

### Key Design Decisions

1. **Immutable Records**: Used for entities to ensure data integrity
2. **Dependency Injection**: Proper IoC container setup for testability
3. **Interface Segregation**: Small, focused interfaces for each service
4. **Deterministic Testing**: Seeded random number generation for reliable tests
5. **Statistical Validation**: Tests verify that stronger teams win more often

### Extensibility Features

- **Pluggable Match Simulation**: Easy to replace the simulation algorithm
- **Multiple Display Formats**: Interface-based display service supports different output formats
- **Tournament Formats**: Architecture supports extending to different tournament types
- **Team Configuration**: Easy to modify team compositions and player strengths

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
dotnet run --project SoccerSimulator.Console
```

## Future Enhancements

This architecture is designed for extensibility:

1. **Web API**: The console app can be replaced with a REST API
2. **Database Integration**: Add repositories for persistent storage
3. **Advanced Simulation**: More sophisticated match simulation algorithms
4. **Multiple Groups**: Support for multiple groups and knockout stages
5. **Player Statistics**: Individual player performance tracking
6. **Real-time Updates**: Live match simulation with progressive updates

## Testing

The solution includes comprehensive unit tests covering:
- Match simulation accuracy
- Tournament generation logic
- Standings calculation
- Statistical distribution verification

Tests use deterministic seeding to ensure consistent results while validating that the simulation produces statistically sound outcomes.

## Architecture Benefits

- **Maintainability**: Clear separation of concerns and single responsibility principle
- **Testability**: Dependency injection and interface-based design enable easy unit testing
- **Extensibility**: Modular design allows for easy addition of new features
- **Performance**: Efficient algorithms with minimal memory allocation
- **Reliability**: Comprehensive test coverage ensures consistent behavior