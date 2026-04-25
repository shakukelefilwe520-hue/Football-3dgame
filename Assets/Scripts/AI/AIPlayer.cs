using UnityEngine;
using Football3D.Player;

namespace Football3D.AI
{
    /// <summary>
    /// AI controller for individual player behavior
    /// </summary>
    public class AIPlayer : MonoBehaviour
    {
        private Player player;
        private AIManager.DifficultyLevel difficulty;
        private float reactionTime = 0.5f;
        private Vector3 targetPosition;
        private Player markedOpponent;

        /// <summary>
        /// Initialize AI player
        /// </summary>
        public void Initialize(Player playerRef, AIManager.DifficultyLevel diffLevel)
        {
            player = playerRef;
            difficulty = diffLevel;
            
            reactionTime = difficulty switch
            {
                AIManager.DifficultyLevel.Easy => 1.0f,
                AIManager.DifficultyLevel.Medium => 0.5f,
                AIManager.DifficultyLevel.Hard => 0.25f,
                AIManager.DifficultyLevel.Expert => 0.1f,
                _ => 0.5f
            };
        }

        /// <summary>
        /// Update AI behavior each frame
        /// </summary>
        public void UpdateBehavior()
        {
            if (player == null) return;

            // Decide action based on difficulty
            DecideAction();
        }

        /// <summary>
        /// Decide what action to take
        /// </summary>
        private void DecideAction()
        {
            // TODO: Implement decision logic based on game state
            // Possible actions: move, pass, shoot, defend
        }

        /// <summary>
        /// Move to target position
        /// </summary>
        public void MoveTo(Vector3 position)
        {
            targetPosition = position;
            Vector3 direction = (position - transform.position).normalized;
            player.Move(direction);
        }

        /// <summary>
        /// Mark/track opponent
        /// </summary>
        public void MarkOpponent(Player opponent)
        {
            markedOpponent = opponent;
        }

        /// <summary>
        /// Pass to teammate
        /// </summary>
        public void PassTo(Player target)
        {
            if (target != null)
            {
                player.Pass(target);
            }
        }

        /// <summary>
        /// Take a shot
        /// </summary>
        public void Shoot()
        {
            player.Shoot(Vector3.forward, Random.Range(0.5f, 1.0f));
        }

        /// <summary>
        /// Set difficulty level
        /// </summary>
        public void SetDifficulty(AIManager.DifficultyLevel newDifficulty)
        {
            difficulty = newDifficulty;
            Initialize(player, difficulty);
        }
    }
}