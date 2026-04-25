using UnityEngine;
using System.IO;
using Football3D.Core;
using Football3D.Data;
using UnityEngine.SceneManagement;

namespace Football3D.Data
{
    /// <summary>
    /// Manages game data persistence including player progression and match history
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        private string SAVE_PATH => Application.persistentDataPath + "/Football3D/";
        private const string PLAYER_DATA_FILE = "player_data.json";
        private const string CAREER_DATA_FILE = "career_data.json";
        private const string MATCH_HISTORY_FILE = "match_history.json";

        private void Awake()
        {
            if (!Directory.Exists(SAVE_PATH))
            {
                Directory.CreateDirectory(SAVE_PATH);
            }
        }

        /// <summary>
        /// Save player data
        /// </summary>
        public void SavePlayerData(Player.Player player)
        {
            Debug.Log($"[DataManager] Saving player data for {player.GetPlayerName()}");
            
            // TODO: Serialize and save player data
        }

        /// <summary>
        /// Load player data
        /// </summary>
        public Player.Player LoadPlayerData(string playerId)
        {
            Debug.Log($"[DataManager] Loading player data for {playerId}");
            
            // TODO: Load and deserialize player data
            return null;
        }

        /// <summary>
        /// Save career progress
        /// </summary>
        public void SaveCareerProgress(CareerData careerData)
        {
            Debug.Log("[DataManager] Saving career progress");
            
            // TODO: Serialize and save career data
        }

        /// <summary>
        /// Load career progress
        /// </summary>
        public CareerData LoadCareerProgress()
        {
            Debug.Log("[DataManager] Loading career progress");
            
            // TODO: Load and deserialize career data
            return null;
        }

        /// <summary>
        /// Record match statistics
        /// </summary>
        public void RecordMatch(Match match, Team winner)
        {
            Debug.Log($"[DataManager] Recording match: {match.HomeTeam.TeamName} vs {match.AwayTeam.TeamName}");
            
            // TODO: Save match to history
        }

        /// <summary>
        /// Get all saved games
        /// </summary>
        public string[] GetSavedGames()
        {
            if (!Directory.Exists(SAVE_PATH))
                return new string[0];
            
            return Directory.GetFiles(SAVE_PATH, "*.json");
        }

        /// <summary>
        /// Delete saved game
        /// </summary>
        public void DeleteSavedGame(string filename)
        {
            string path = Path.Combine(SAVE_PATH, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[DataManager] Deleted save: {filename}");
            }
        }
    }

    /// <summary>
    /// Career mode data
    /// </summary>
    [System.Serializable]
    public class CareerData
    {
        public Team OwnedTeam { get; set; }
        public int CurrentSeason { get; set; }
        public int LeaguePosition { get; set; }
        public int Budget { get; set; }
    }
}