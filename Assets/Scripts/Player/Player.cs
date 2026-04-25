using UnityEngine;
using System;
using Football3D.Data;

namespace Football3D.Player
{
    /// <summary>
    /// Represents a football player with attributes, stats, and equipment
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private string playerId;
        [SerializeField] private string playerName;
        [SerializeField] private int number;
        [SerializeField] private PlayerPosition position;
        [SerializeField] private Team team;

        public PlayerStats Stats { get; set; } = new();
        public PlayerAttributes Attributes { get; set; } = new();
        public Equipment Equipment { get; set; } = new();

        private Rigidbody rb;
        private Vector3 velocity = Vector3.zero;
        private float currentStamina;
        private bool hasPopcorn = false;

        /// <summary>Player position on field</summary>
        public enum PlayerPosition
        {
            Goalkeeper,
            Defender,
            Midfielder,
            Forward
        }

        /// <summary>Event triggered when player shoots</summary>
        public static event Action<Player> OnPlayerShoot;
        /// <summary>Event triggered when player passes</summary>
        public static event Action<Player, Player> OnPlayerPass;
        /// <summary>Event triggered when player gets injured</summary>
        public static event Action<Player> OnPlayerInjury;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            currentStamina = Attributes.MaxStamina;
        }

        private void Update()
        {
            UpdateStamina();
        }

        private void FixedUpdate()
        {
            ApplyPhysics();
        }

        /// <summary>
        /// Move player in specified direction
        /// </summary>
        public void Move(Vector3 direction, bool sprinting = false)
        {
            float speed = sprinting ? Attributes.MaxSpeed * 1.5f : Attributes.MaxSpeed;
            
            if (currentStamina > 0)
            {
                velocity = direction.normalized * speed;
                if (rb != null)
                {
                    rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
                }
            }
        }

        /// <summary>
        /// Pass ball to another player
        /// </summary>
        public void Pass(Player targetPlayer)
        {
            if (targetPlayer == null) return;
            
            Debug.Log($"[Player] {playerName} passes to {targetPlayer.playerName}");
            OnPlayerPass?.Invoke(this, targetPlayer);
        }

        /// <summary>
        /// Shoot at goal
        /// </summary>
        public void Shoot(Vector3 shootDirection, float power)
        {
            if (power < 0 || power > 1) power = Mathf.Clamp01(power);
            
            Debug.Log($"[Player] {playerName} shoots with power {power}");
            OnPlayerShoot?.Invoke(this);
        }

        /// <summary>
        /// Update player stamina over time
        /// </summary>
        private void UpdateStamina()
        {
            if (velocity.magnitude > 0)
            {
                currentStamina -= Time.deltaTime * 5f;
                currentStamina = Mathf.Clamp(currentStamina, 0, Attributes.MaxStamina);
            }
            else
            {
                currentStamina += Time.deltaTime * 2f;
                currentStamina = Mathf.Clamp(currentStamina, 0, Attributes.MaxStamina);
            }
        }

        /// <summary>
        /// Apply physics to player movement
        /// </summary>
        private void ApplyPhysics()
        {
            // Friction/deceleration
            velocity *= 0.95f;
        }

        /// <summary>
        /// Get player stamina percentage (0-1)
        /// </summary>
        public float GetStaminaPercentage() => currentStamina / Attributes.MaxStamina;

        /// <summary>
        /// Get player ID
        /// </summary>
        public string GetPlayerId() => playerId;

        /// <summary>
        /// Get player name
        /// </summary>
        public string GetPlayerName() => playerName;

        /// <summary>
        /// Get player position
        /// </summary>
        public PlayerPosition GetPosition() => position;

        /// <summary>
        /// Get player team
        /// </summary>
        public Team GetTeam() => team;

        /// <summary>
        /// Simulate player injury
        /// </summary>
        public void Injure()
        {
            Debug.LogWarning($"[Player] {playerName} is injured!");
            OnPlayerInjury?.Invoke(this);
        }
    }

    /// <summary>
    /// Player statistics tracking
    /// </summary>
    [System.Serializable]
    public class PlayerStats
    {
        public int GoalsScored { get; set; }
        public int Assists { get; set; }
        public int Tackles { get; set; }
        public int Passes { get; set; }
        public int PassesAccurate { get; set; }
        public int Shots { get; set; }
        public int ShotsOnTarget { get; set; }
        public float Rating { get; set; }
    }

    /// <summary>
    /// Player attributes (skill-based stats)
    /// </summary>
    [System.Serializable]
    public class PlayerAttributes
    {
        [Range(1, 99)] public int Pace = 75;
        [Range(1, 99)] public int Shooting = 70;
        [Range(1, 99)] public int Passing = 75;
        [Range(1, 99)] public int Dribbling = 70;
        [Range(1, 99)] public int Defense = 60;
        [Range(1, 99)] public int Physical = 75;

        public float MaxSpeed => Pace / 10f;
        public float MaxStamina => Physical / 2f;
    }

    /// <summary>
    /// Player equipment
    /// </summary>
    [System.Serializable]
    public class Equipment
    {
        public string BootType { get; set; } = "Standard";
        public string ShirtNumber { get; set; } = "10";
        public string Gloves { get; set; } = "None";
        public float SpeedBonus { get; set; } = 0f;
        public float PowerBonus { get; set; } = 0f;
    }
}