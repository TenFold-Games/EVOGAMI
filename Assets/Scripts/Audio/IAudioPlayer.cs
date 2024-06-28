using UnityEngine;

namespace EVOGAMI.Audio
{
    /// <summary>
    ///     Interface for playing audio
    /// </summary>
    /// <typeparam name="TAudio">The type of audio to play, should be either AudioClip or AudioSource</typeparam>
    /// <typeparam name="TReturn">The return type of the PlayAudio method, should be void or IEnumerator</typeparam>
    public interface IAudioPlayer<in TAudio, out TReturn>
    {
        /// <summary>
        ///     Play the audio
        /// </summary>
        /// <param name="audio">The audio to play</param>
        /// <returns>void, or IEnumerator if the audio is played asynchronously</returns>
        public TReturn PlayAudio(TAudio audio);
    }

    public interface IAudioPlayer
    {
        /// <summary>
        ///     Play the audio
        /// </summary>
        /// <param name="audio">The audio to play</param>
        public void PlayAudio(AudioSource audio);
    }
}