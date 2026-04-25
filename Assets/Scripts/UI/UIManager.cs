using UnityEngine;
using System.Collections.Generic;
using Football3D.Data;

namespace Football3D.UI
{
    /// <summary>
    /// Manages all UI screens and HUD elements
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas mainCanvas;
        private Dictionary<string, UIScreen> screens = new();

        private void Awake()
        {
            if (mainCanvas == null)
            {
                mainCanvas = FindObjectOfType<Canvas>();
            }
        }

        /// <summary>
        /// Show a UI screen
        /// </summary>
        public void ShowScreen(string screenName)
        {
            Debug.Log($"[UIManager] Showing screen: {screenName}");
            
            if (screens.TryGetValue(screenName, out var screen))
            {
                screen.Show();
            }
        }

        /// <summary>
        /// Hide a UI screen
        /// </summary>
        public void HideScreen(string screenName)
        {
            Debug.Log($"[UIManager] Hiding screen: {screenName}");
            
            if (screens.TryGetValue(screenName, out var screen))
            {
                screen.Hide();
            }
        }

        /// <summary>
        /// Update HUD with match information
        /// </summary>
        public void UpdateHUD(int homeScore, int awayScore, string timeFormatted)
        {
            // TODO: Update HUD elements with current game state
        }

        /// <summary>
        /// Show match results
        /// </summary>
        public void ShowMatchResults(Team homeTeam, Team awayTeam, int homeScore, int awayScore, Team winner)
        {
            Debug.Log($"[UIManager] Showing match results: {homeTeam.TeamName} {homeScore}:{awayScore} {awayTeam.TeamName}");
            
            // TODO: Display results screen
            string result = winner != null ? $"{winner.TeamName} wins!" : "Draw!";
            Debug.Log(result);
        }

        /// <summary>
        /// Show notification message
        /// </summary>
        public void ShowNotification(string message)
        {
            Debug.Log($"[UIManager] Notification: {message}");
            // TODO: Display notification to player
        }

        /// <summary>
        /// Register UI screen
        /// </summary>
        public void RegisterScreen(string name, UIScreen screen)
        {
            screens[name] = screen;
        }
    }

    /// <summary>
    /// Base class for UI screens
    /// </summary>
    public abstract class UIScreen : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}