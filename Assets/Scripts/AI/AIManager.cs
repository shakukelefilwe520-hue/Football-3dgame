using UnityEngine;
using System.Collections.Generic;
using Football3D.Data;
using Football3D.Player;

namespace Football3D.AI
{
    /// <summary>
    /// Manages AI behavior for opponent teams
    /// </summary>
    public class AIManager : MonoBehaviour
    {
        public enum DifficultyLevel { Easy, Medium, Hard, Expert }

        [SerializeField] private DifficultyLevel difficulty = DifficultyLevel.Medium;
        [SerializeField] private float decisionInterval = 0.5f;

        private Team aiTeam;
        private List<AIPlayer> aiPlayers = new();
        private Formation currentFormation = Formation.F442;
        private float lastDecisionTime = 0f;

        /// <summary>
        /// Initialize AI for specified team
        /// </summary>
        public void InitializeAI(Team team)
        {
            Debug.Log($"[AIManager] Initializing AI for {team.TeamName} at {difficulty} difficulty");
            aiTeam = team;
            aiPlayers.Clear();

            // Create AI player controllers
            foreach (var player in team.Players)
            {
                var aiPlayer = player.gameObject.AddComponent<AIPlayer>();
                aiPlayer.Initialize(player, difficulty);
                aiPlayers.Add(aiPlayer);
            }
        }

        /// <summary>
        /// Update AI behavior
        /// </summary>
        private void Update()
        {
            if (aiTeam == null) return;

            if (Time.time - lastDecisionTime >= decisionInterval)
            {
                UpdateAIBehavior();
                lastDecisionTime = Time.time;
            }
        }

        /// <summary>
        /// Update all AI players behavior
        /// </summary>
        public void UpdateAIBehavior()
        {
            foreach (var aiPlayer in aiPlayers)
            {
                aiPlayer.UpdateBehavior();
            }
        }

        /// <summary>
        /// Change formation
        /// </summary>
        public void ChangeFormation(Formation newFormation)
        {
            Debug.Log($"[AIManager] Changing formation to {newFormation}");
            currentFormation = newFormation;
            // Reposition players based on new formation
        }

        /// <summary>
        /// Set difficulty level
        /// </summary>
        public void SetDifficulty(DifficultyLevel newDifficulty)
        {
            Debug.Log($"[AIManager] Setting difficulty to {newDifficulty}");
            difficulty = newDifficulty;
            
            foreach (var aiPlayer in aiPlayers)
            {
                aiPlayer.SetDifficulty(difficulty);
            }
        }

        /// <summary>
        /// Get current formation
        /// </summary>
        public Formation GetFormation() => currentFormation;
    }
}