using System;
using System.Collections.Generic;
using Football3D.Player;

namespace Football3D.Data
{
    /// <summary>
    /// Represents a football match with teams, score, and events
    /// </summary>
    [Serializable]
    public class Match
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public float ElapsedTime { get; set; }
        public float MatchDuration { get; set; }
        public List<MatchEvent> Events { get; set; } = new();
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// Represents an event that occurred during a match
    /// </summary>
    [Serializable]
    public class MatchEvent
    {
        public enum EventType { Goal, Tackle, Foul, CornerKick, Throw, Pass, Shot, Injury }
        
        public EventType Type { get; set; }
        public float TimeOfEvent { get; set; }
        public string PlayerName { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents statistics from a completed match
    /// </summary>
    [Serializable]
    public class MatchStats
    {
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int HomePossession { get; set; }
        public int AwayPossession { get; set; }
        public int HomeShotsOnTarget { get; set; }
        public int AwayShotsOnTarget { get; set; }
        public int HomeFouls { get; set; }
        public int AwayFouls { get; set; }
        public DateTime MatchDate { get; set; }
        public float MatchDuration { get; set; }
    }
}