using System;
using System.Collections.Generic;
using UnityEngine;

namespace Football3D.Data
{
    /// <summary>
    /// Represents a football team with players and statistics
    /// </summary>
    [Serializable]
    public class Team
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public string CoachName { get; set; }
        public string StadiumName { get; set; }
        
        public List<Player.Player> Players { get; set; } = new();
        public Formation CurrentFormation { get; set; } = Formation.F442;
        public TeamStats Statistics { get; set; } = new();

        /// <summary>
        /// Get team formation as string
        /// </summary>
        public string GetFormationString() => CurrentFormation switch
        {
            Formation.F442 => "4-4-2",
            Formation.F433 => "4-3-3",
            Formation.F352 => "3-5-2",
            Formation.F541 => "5-4-1",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Football formations
    /// </summary>
    public enum Formation
    {
        F442, // 4 defenders, 4 midfielders, 2 forwards
        F433, // 4 defenders, 3 midfielders, 3 forwards
        F352, // 3 defenders, 5 midfielders, 2 forwards
        F541  // 5 defenders, 4 midfielders, 1 forward
    }

    /// <summary>
    /// Team statistics and records
    /// </summary>
    [Serializable]
    public class TeamStats
    {
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }

        /// <summary>
        /// Calculate goal difference
        /// </summary>
        public int GetGoalDifference() => GoalsFor - GoalsAgainst;

        /// <summary>
        /// Get total matches played
        /// </summary>
        public int GetTotalMatches() => Wins + Draws + Losses;

        /// <summary>
        /// Get win percentage
        /// </summary>
        public float GetWinPercentage()
        {
            int total = GetTotalMatches();
            return total > 0 ? (Wins / (float)total) * 100f : 0f;
        }
    }
}