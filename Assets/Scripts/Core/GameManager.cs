using UnityEngine;
using System;
using System.Collections.Generic;
using Football3D.Data;
using Football3D.Network;
using Football3D.AI;
using Football3D.Audio;
using Football3D.UI;

namespace Football3D.Core
{
    /// <summary>
    /// Central game manager that orchestrates all game systems and manages game state.
    /// Implements Singleton pattern for global access.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>Singleton instance of GameManager</summary>
        public static GameManager Instance { get; private set; }

        /// <summary>Current game state enumeration</summary>
        public enum GameState
        {
            Menu,
            MatchSetup,
            MatchStart,
            Playing,
            Paused,
            MatchEnd,
            CareerMode
        }

        [SerializeField] private float matchDuration = 5400f; // 90 minutes
        [SerializeField] private bool isMultiplayer = false;

        private GameState currentState = GameState.Menu;
        private float matchElapsedTime = 0f;
        private Match currentMatch;
        private Team homeTeam;
        private Team awayTeam;
        private int homeScore = 0;
        private int awayScore = 0;

        // References to other managers
        private NetworkManager networkManager;
        private AudioManager audioManager;
        private UIManager uiManager;
        private AIManager aiManager;
        private DataManager dataManager;

        /// <summary>Event triggered when game state changes</summary>
        public static event Action<GameState> OnGameStateChanged;
        /// <summary>Event triggered when match time updates</summary>
        public static event Action<float> OnMatchTimeUpdated;
        /// <summary>Event triggered when goal is scored</summary>
        public static event Action<Team, int> OnGoalScored;
        /// <summary>Event triggered when match ends</summary>
        public static event Action<Team> OnMatchEnded;

        private void Awake()
        {
            // Implement singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }

        private void Start()
        {
            Debug.Log("[GameManager] Game initialized");
        }

        private void Update()
        {
            if (currentState == GameState.Playing)
            {
                UpdateMatchTime();
            }
        }

        /// <summary>
        /// Initialize all game managers
        /// </summary>
        private void InitializeManagers()
        {
            networkManager = GetComponent<NetworkManager>();
            audioManager = GetComponent<AudioManager>();
            uiManager = GetComponent<UIManager>();
            aiManager = GetComponent<AIManager>();
            dataManager = GetComponent<DataManager>();

            if (networkManager == null) gameObject.AddComponent<NetworkManager>();
            if (audioManager == null) gameObject.AddComponent<AudioManager>();
            if (uiManager == null) gameObject.AddComponent<UIManager>();
            if (aiManager == null) gameObject.AddComponent<AIManager>();
            if (dataManager == null) gameObject.AddComponent<DataManager>();
        }

        /// <summary>
        /// Start a new match with specified configuration
        /// </summary>
        public void StartMatch(Team home, Team away, bool multiplayer = false)
        {
            Debug.Log($"[GameManager] Starting match: {home.TeamName} vs {away.TeamName}");

            homeTeam = home;
            awayTeam = away;
            isMultiplayer = multiplayer;
            homeScore = 0;
            awayScore = 0;
            matchElapsedTime = 0f;

            currentMatch = new Match
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeScore = 0,
                AwayScore = 0,
                ElapsedTime = 0f,
                MatchDuration = matchDuration
            };

            SetGameState(GameState.MatchStart);
            audioManager.PlayMusic("match_background", true);

            if (!isMultiplayer)
            {
                aiManager.InitializeAI(awayTeam);
            }
            else
            {
                networkManager.SynchronizeGameState();
            }
        }

        /// <summary>
        /// Update match time and check for match end
        /// </summary>
        private void UpdateMatchTime()
        {
            matchElapsedTime += Time.deltaTime;
            OnMatchTimeUpdated?.Invoke(matchElapsedTime);

            if (matchElapsedTime >= matchDuration)
            {
                EndMatch(DetermineWinner());
            }
        }

        /// <summary>
        /// Pause the current match
        /// </summary>
        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
                Time.timeScale = 0f;
                Debug.Log("[GameManager] Game paused");
            }
        }

        /// <summary>
        /// Resume the current match
        /// </summary>
        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
                Time.timeScale = 1f;
                Debug.Log("[GameManager] Game resumed");
            }
        }

        /// <summary>
        /// End the current match
        /// </summary>
        public void EndMatch(Team winner)
        {
            SetGameState(GameState.MatchEnd);
            Time.timeScale = 0f;

            Debug.Log($"[GameManager] Match ended. Winner: {(winner != null ? winner.TeamName : "Draw")}");

            // Record match statistics
            dataManager.RecordMatch(currentMatch, winner);

            // Trigger event
            OnMatchEnded?.Invoke(winner);

            // Show results UI
            uiManager.ShowMatchResults(homeTeam, awayTeam, homeScore, awayScore, winner);
        }

        /// <summary>
        /// Score a goal for specified team
        /// </summary>
        public void ScoreGoal(Team scoringTeam)
        {
            if (scoringTeam == homeTeam)
            {
                homeScore++;
                currentMatch.HomeScore = homeScore;
            }
            else if (scoringTeam == awayTeam)
            {
                awayScore++;
                currentMatch.AwayScore = awayScore;
            }

            audioManager.PlaySFX("goal_sound", 1.0f);
            OnGoalScored?.Invoke(scoringTeam, (scoringTeam == homeTeam ? homeScore : awayScore));

            Debug.Log($"[GameManager] Goal! {scoringTeam.TeamName} - Score: {homeScore}:{awayScore}");
        }

        /// <summary>
        /// Determine the winner of the match
        /// </summary>
        private Team DetermineWinner()
        {
            if (homeScore > awayScore) return homeTeam;
            if (awayScore > homeScore) return awayTeam;
            return null; // Draw
        }

        /// <summary>
        /// Change the current game state
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                OnGameStateChanged?.Invoke(currentState);
                Debug.Log($"[GameManager] Game state changed to: {currentState}");
            }
        }

        /// <summary>
        /// Get current game state
        /// </summary>
        public GameState GetGameState() => currentState;

        /// <summary>
        /// Get current match elapsed time in seconds
        /// </summary>
        public float GetMatchElapsedTime() => matchElapsedTime;

        /// <summary>
        /// Get match time as formatted string (MM:SS)
        /// </summary>
        public string GetFormattedMatchTime()
        {
            int minutes = (int)(matchElapsedTime / 60);
            int seconds = (int)(matchElapsedTime % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }

        /// <summary>
        /// Get current match information
        /// </summary>
        public Match GetCurrentMatch() => currentMatch;

        /// <summary>
        /// Get home team
        /// </summary>
        public Team GetHomeTeam() => homeTeam;

        /// <summary>
        /// Get away team
        /// </summary>
        public Team GetAwayTeam() => awayTeam;

        /// <summary>
        /// Get current score
        /// </summary>
        public (int home, int away) GetScore() => (homeScore, awayScore);

        /// <summary>
        /// Return to main menu
        /// </summary>
        public void ReturnToMenu()
        {
            Time.timeScale = 1f;
            SetGameState(GameState.Menu);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}