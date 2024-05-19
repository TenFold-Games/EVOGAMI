using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EVOGAMI.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        // Audio sources
        [SerializeField] private AudioSource[] backgroundMusics;
        [SerializeField] private AudioSource[] soundEffects;

        // Flags
        public bool shouldPlayBGM = true;
        private bool _isPlayingBGM;

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            // Keep between scenes
            DontDestroyOnLoad(gameObject);

            // Play the first background music track
            if (shouldPlayBGM && backgroundMusics.Length > 0)
                PlayRandomBackgroundMusic();
        }

        public void Update()
        {
            if (shouldPlayBGM && !_isPlayingBGM)
                PlayRandomBackgroundMusic();
        }

        /// <summary>
        ///     Play the audio source
        /// </summary>
        /// <param name="audioSource">The audio source to play</param>
        /// <param name="loop">Should the audio source loop?</param>
        private void Play([NotNull] AudioSource audioSource, bool loop)
        {
            if (_isPlayingBGM) return;

            _isPlayingBGM = true;
            audioSource.loop = loop;
            audioSource.Play();
        }

        /// <summary>
        ///     Stop the audio source
        /// </summary>
        /// <param name="audioSource"></param>
        private void Stop([NotNull] AudioSource audioSource)
        {
            if (!_isPlayingBGM) return;

            _isPlayingBGM = false;
            audioSource.Stop();
        }

        #region BGM

        /// <summary>
        ///     Play the background music with the given audio source
        /// </summary>
        /// <param name="source">The audio source to play</param>
        /// <param name="loop">Should the audio source loop?</param>
        public void PlayBackgroundMusic(AudioSource source, bool loop = false)
        {
            if (source == null)
                return;

            Play(source, loop);
        }

        /// <summary>
        ///     Play the background music with the given index
        /// </summary>
        /// <param name="index">The index of the audio source to play</param>
        /// <param name="loop">Should the audio source loop?</param>
        public void PlayBackgroundMusic(int index, bool loop = false)
        {
            if (index < 0 || index >= backgroundMusics.Length)
                return;

            Play(backgroundMusics[index], loop);
        }

        /// <summary>
        ///     Play a random background music track
        /// </summary>
        /// <param name="loop">Should the audio source loop?</param>
        public void PlayRandomBackgroundMusic(bool loop = false)
        {
            var randomIndex = Random.Range(0, backgroundMusics.Length);
            Play(backgroundMusics[randomIndex], loop);
        }

        /// <summary>
        ///     Stop the background music with the given audio source
        /// </summary>
        /// <param name="source">The audio source to stop</param>
        public void StopBackgroundMusic(AudioSource source)
        {
            if (source == null)
                return;

            Stop(source);
        }

        /// <summary>
        ///     Stop the background music with the given index
        /// </summary>
        /// <param name="index"></param>
        public void StopBackgroundMusic(int index)
        {
            if (index < 0 || index >= backgroundMusics.Length)
                return;

            Stop(backgroundMusics[index]);
        }

        #endregion

        #region SFX

        /// <summary>
        ///     Play the sound effect with the given audio source
        /// </summary>
        /// <param name="source">The audio source to play</param>
        /// <param name="loop">Should the audio source loop?</param>
        public void PlaySoundEffect(AudioSource source, bool loop = false)
        {
            if (source == null)
                return;

            Play(source, loop);
        }

        /// <summary>
        ///     Play the sound effect with the given index
        /// </summary>
        /// <param name="index">The index of the audio source to play</param>
        /// <param name="loop">Should the audio source loop?</param>
        public void PlaySoundEffect(int index, bool loop = false)
        {
            if (index < 0 || index >= soundEffects.Length)
                return;

            Play(soundEffects[index], loop);
        }

        #endregion
    }
}