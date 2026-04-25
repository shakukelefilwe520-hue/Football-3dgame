using UnityEngine;
using System.Collections.Generic;

namespace Football3D.Audio
{
    /// <summary>
    /// Manages all audio including music and sound effects
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private float masterVolume = 1.0f;
        [SerializeField] private float musicVolume = 0.7f;
        [SerializeField] private float sfxVolume = 1.0f;

        private AudioSource musicSource;
        private Dictionary<string, AudioClip> audioClips = new();
        private Dictionary<string, AudioSource> activeSounds = new();

        private void Awake()
        {
            // Create music audio source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.volume = musicVolume * masterVolume;
            musicSource.loop = true;
        }

        /// <summary>
        /// Play background music
        /// </summary>
        public void PlayMusic(string trackName, bool loop = true)
        {
            Debug.Log($"[AudioManager] Playing music: {trackName}");
            
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }

            // TODO: Load and play audio clip
            // AudioClip clip = Resources.Load<AudioClip>($"Audio/Music/{trackName}");
            // musicSource.clip = clip;
            // musicSource.loop = loop;
            // musicSource.Play();
        }

        /// <summary>
        /// Play sound effect
        /// </summary>
        public void PlaySFX(string soundName, float volume = 1.0f)
        {
            Debug.Log($"[AudioManager] Playing SFX: {soundName}");
            
            // TODO: Load and play sound effect
            // AudioClip clip = Resources.Load<AudioClip>($"Audio/SFX/{soundName}");
            // AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume * sfxVolume * masterVolume);
        }

        /// <summary>
        /// Stop current music
        /// </summary>
        public void StopMusic()
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
                Debug.Log("[AudioManager] Music stopped");
            }
        }

        /// <summary>
        /// Set master volume
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume * masterVolume;
        }

        /// <summary>
        /// Set music volume
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume * masterVolume;
        }

        /// <summary>
        /// Set SFX volume
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
        }
    }
}