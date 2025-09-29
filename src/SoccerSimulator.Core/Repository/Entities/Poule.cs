using System.Collections.Frozen;

namespace SoccerSimulator.Core.Repository.Entities;

public record Poule(PouleName PouleName, FrozenSet<Team> Teams);

public enum PouleName { 
    A, B, C
}

