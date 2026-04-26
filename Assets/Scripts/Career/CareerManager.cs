using UnityEngine;
using System.Collections.Generic;
using Football3D.Data;

namespace Football3D.Career
{
    /// <summary>
    /// Manages career mode progression, budget, transfers, and season tracking
    /// </summary>
    public class CareerManager : MonoBehaviour
    {
        private static CareerManager instance;
        public static CareerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<CareerManager>();
                return instance;
            }
        }

        [SerializeField] private float startingBudget = 5000000f;
        [SerializeField] private int matchesPerSeason = 38;
        [SerializeField] private int maxSeasons = 10;

        private Team playerTeam;
        private float availableBudget;
        private int currentSeason = 1;
        private int currentMatchday = 1;

        private List<SeasonRecord> seasonHistory = new List<SeasonRecord>();
        private List<MatchResult> matchHistory = new List<MatchResult>();
        private List<Achievement> achievements = new List<Achievement>();
        private Dictionary<string, PlayerContract> playerContracts = new Dictionary<string, PlayerContract>();

        private int totalWins = 0;
        private int totalDraws = 0;
        private int totalLosses = 0;
        private int totalGoalsFor = 0;
        private int totalGoalsAgainst = 0;

        public delegate void BudgetChangedDelegate(float newBudget);
        public static event BudgetChangedDelegate OnBudgetChanged;

        public delegate void TransferDelegate(Player player, float amount, bool isBuy);
        public static event TransferDelegate OnTransfer;

        public delegate void SeasonEndDelegate(SeasonRecord record);
        public static event SeasonEndDelegate OnSeasonEnd;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initialize career mode
        /// </summary>
        public void InitializeCareer(Team team)
        {
            playerTeam = team;
            availableBudget = startingBudget;
            currentSeason = 1;
            currentMatchday = 1;

            // Initialize contracts for all players
            foreach (Player player in team.Players)
            {
                PlayerContract contract = new PlayerContract
                {
                    PlayerId = player.PlayerId,
                    PlayerName = player.PlayerName,
                    WeeklySalary = 50000f,
                    ContractYears = 3,
                    JoinDate = System.DateTime.Now
                };
                playerContracts[player.PlayerId] = contract;
            }

            Debug.Log($"[CareerManager] Career started with team: {team.TeamName}, Budget: ${availableBudget:F0}");
        }

        /// <summary>
        /// Record match result
        /// </summary>
        public void RecordMatchResult(string opponentName, int teamGoals, int opponentGoals, bool isHome)
        {
            MatchResult result = new MatchResult
            {
                Season = currentSeason,
                Matchday = currentMatchday,
                OpponentName = opponentName,
                TeamGoals = teamGoals,
                OpponentGoals = opponentGoals,
                IsHome = isHome,
                Date = System.DateTime.Now
            };

            matchHistory.Add(result);

            // Update stats
            totalGoalsFor += teamGoals;
            totalGoalsAgainst += opponentGoals;

            if (teamGoals > opponentGoals)
            {
                totalWins++;
                Debug.Log($"[CareerManager] WIN vs {opponentName} {teamGoals}-{opponentGoals}");
            }
            else if (teamGoals < opponentGoals)
            {
                totalLosses++;
                Debug.Log($"[CareerManager] LOSS vs {opponentName} {teamGoals}-{opponentGoals}");
            }
            else
            {
                totalDraws++;
                Debug.Log($"[CareerManager] DRAW vs {opponentName} {teamGoals}-{opponentGoals}");
            }

            // Deduct salaries
            DeductWeeklySalaries();

            // Check for season end
            if (currentMatchday >= matchesPerSeason)
            {
                EndSeason();
            }
            else
            {
                currentMatchday++;
            }
        }

        /// <summary>
        /// Transfer player in (buy)
        /// </summary>
        public bool TransferPlayerIn(Player player, float transferFee)
        {
            if (availableBudget < transferFee)
            {
                Debug.LogWarning($"[CareerManager] Insufficient budget for {player.PlayerName}");
                return false;
            }

            availableBudget -= transferFee;
            playerTeam.Players.Add(player);

            // Add contract
            PlayerContract contract = new PlayerContract
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                WeeklySalary = 75000f,
                ContractYears = 3,
                JoinDate = System.DateTime.Now
            };
            playerContracts[player.PlayerId] = contract;

            Debug.Log($"[CareerManager] Bought {player.PlayerName} for ${transferFee:F0}");
            OnTransfer?.Invoke(player, transferFee, true);
            OnBudgetChanged?.Invoke(availableBudget);

            return true;
        }

        /// <summary>
        /// Transfer player out (sell)
        /// </summary>
        public bool TransferPlayerOut(string playerId, float transferFee)
        {
            Player playerToSell = playerTeam.Players.Find(p => p.PlayerId == playerId);
            
            if (playerToSell == null)
            {
                Debug.LogWarning($"[CareerManager] Player not found");
                return false;
            }

            availableBudget += transferFee;
            playerTeam.Players.Remove(playerToSell);
            playerContracts.Remove(playerId);

            Debug.Log($"[CareerManager] Sold {playerToSell.PlayerName} for ${transferFee:F0}");
            OnTransfer?.Invoke(playerToSell, transferFee, false);
            OnBudgetChanged?.Invoke(availableBudget);

            return true;
        }

        /// <summary>
        /// Update player salary
        /// </summary>
        public void UpdatePlayerSalary(string playerId, float newSalary)
        {
            if (playerContracts.ContainsKey(playerId))
            {
                playerContracts[playerId].WeeklySalary = newSalary;
                Debug.Log($"[CareerManager] Updated salary to ${newSalary:F0}");
            }
        }

        /// <summary>
        /// Deduct weekly salaries
        /// </summary>
        private void DeductWeeklySalaries()
        {
            float totalSalaries = 0f;
            foreach (var contract in playerContracts.Values)
            {
                totalSalaries += contract.WeeklySalary;
            }

            availableBudget -= totalSalaries;
            OnBudgetChanged?.Invoke(availableBudget);

            if (availableBudget < 0)
            {
                Debug.LogWarning("[CareerManager] Budget is negative!");
            }
        }

        /// <summary>
        /// End season and advance to next
        /// </summary>
        private void EndSeason()
        {
            SeasonRecord season = new SeasonRecord
            {
                Season = currentSeason,
                Wins = totalWins,
                Draws = totalDraws,
                Losses = totalLosses,
                GoalsFor = totalGoalsFor,
                GoalsAgainst = totalGoalsAgainst,
                Points = (totalWins * 3) + totalDraws
            };

            seasonHistory.Add(season);

            Debug.Log($"[CareerManager] Season {currentSeason} ended: {totalWins}W-{totalDraws}D-{totalLosses}L");
            OnSeasonEnd?.Invoke(season);

            // Check for achievements
            if (totalWins >= 28)
                AddAchievement("Champion", "Won the league with 28+ wins");
            if (totalGoalsFor > 80)
                AddAchievement("Offensive Machine", "Scored over 80 goals in a season");

            // Advance season
            if (currentSeason < maxSeasons)
            {
                currentSeason++;
                currentMatchday = 1;
                totalWins = 0;
                totalDraws = 0;
                totalLosses = 0;
                totalGoalsFor = 0;
                totalGoalsAgainst = 0;
            }
        }

        /// <summary>
        /// Add achievement
        /// </summary>
        public void AddAchievement(string title, string description)
        {
            Achievement achievement = new Achievement
            {
                Title = title,
                Description = description,
                UnlockDate = System.DateTime.Now
            };

            achievements.Add(achievement);
            Debug.Log($"[CareerManager] Achievement unlocked: {title}");
        }

        /// <summary>
        /// Get available budget
        /// </summary>
        public float GetAvailableBudget() => availableBudget;

        /// <summary>
        /// Get current season
        /// </summary>
        public int GetCurrentSeason() => currentSeason;

        /// <summary>
        /// Get current matchday
        /// </summary>
        public int GetCurrentMatchday() => currentMatchday;

        /// <summary>
        /// Get player team
        /// </summary>
        public Team GetPlayerTeam() => playerTeam;

        /// <summary>
        /// Get match history
        /// </summary>
        public List<MatchResult> GetMatchHistory() => matchHistory;

        /// <summary>
        /// Get season history
        /// </summary>
        public List<SeasonRecord> GetSeasonHistory() => seasonHistory;

        /// <summary>
        /// Get achievements
        /// </summary>
        public List<Achievement> GetAchievements() => achievements;

        /// <summary>
        /// Get player contract
        /// </summary>
        public PlayerContract GetPlayerContract(string playerId)
        {
            return playerContracts.ContainsKey(playerId) ? playerContracts[playerId] : null;
        }
    }

    /// <summary>
    /// Season record data
    /// </summary>
    public class SeasonRecord
    {
        public int Season;
        public int Wins;
        public int Draws;
        public int Losses;
        public int GoalsFor;
        public int GoalsAgainst;
        public int Points;
    }

    /// <summary>
    /// Match result data
    /// </summary>
    public class MatchResult
    {
        public int Season;
        public int Matchday;
        public string OpponentName;
        public int TeamGoals;
        public int OpponentGoals;
        public bool IsHome;
        public System.DateTime Date;
    }

    /// <summary>
    /// Player contract data
    /// </summary>
    public class PlayerContract
    {
        public string PlayerId;
        public string PlayerName;
        public float WeeklySalary;
        public int ContractYears;
        public System.DateTime JoinDate;
    }

    /// <summary>
    /// Achievement data
    /// </summary>
    public class Achievement
    {
        public string Title;
        public string Description;
        public System.DateTime UnlockDate;
    }
}