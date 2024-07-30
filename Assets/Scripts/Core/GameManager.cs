using System;
using System.Collections.Generic;
using EVOGAMI.Custom.Scriptable;
using EVOGAMI.Origami;
using EVOGAMI.Region;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EVOGAMI.Core
{
    /// <summary>
    ///     Manages the game state and progression
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        ///     Singleton instance of the GameManager
        /// </summary>
        public static GameManager Instance { get; private set; }

        [Header("Checkpoints")]
        // The checkpoints in the level, ordered from start to finish
        [SerializeField] [Tooltip("The checkpoints in the level, ordered from start to finish")]
        private CheckpointRegion[] checkpoints;
        // The checkpoint the player will respawn at
        [Tooltip("The checkpoint the player will respawn at")]
        public CheckpointRegion CurrentCheckpoint { get; private set; }

        [Header("Settings")]
        // Origami related settings
        [SerializeField] [Tooltip("Origami related settings")]
        public OrigamiSettings origamiSettings;

        [Header("Scene Management")]
        // The start scene
        [SerializeField] [Tooltip("The start scene")]
        private string startMenuScene;

        [Header("End Game")]
        // Event invoked when the game is complete
        [SerializeField] [Tooltip("Event invoked when the game is complete")]
        public GameCompleteEvent onGameComplete = new();

        /// <summary>
        ///     Event invoked when the game is complete
        /// </summary>
        [Serializable] public class GameCompleteEvent : UnityEvent {}

        private GameObject _headsUpDisplay;
        private GameObject _finishLevelUi;

        /// <summary>
        ///     The indices of the checkpoints
        /// </summary>
        private Dictionary<CheckpointRegion, int> _checkpointIndices;

        #region Scene Management

        public void LoadStartMenu()
        {
            // Load the start menu scene
            SceneManager.LoadScene(startMenuScene);
        }

        #endregion

        #region Unity Functions

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Initialize checkpoint states
            _checkpointIndices = new Dictionary<CheckpointRegion, int>();
            var i = 0;
            foreach (var checkpoint in checkpoints)
                _checkpointIndices?.Add(checkpoint, i++);
            CurrentCheckpoint = checkpoints[0];

            // Locks the cursor to the game window
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            // Get the UI elements
            _headsUpDisplay = UiManager.Instance.headsUpDisplay;
            _finishLevelUi = UiManager.Instance.finishLevelUi;
        }

        #endregion

        /// <summary>
        ///     Callback for when the player reaches a checkpoint
        /// </summary>
        /// <param name="checkpoint">The checkpoint that was reached</param>
        public void CheckpointReached(CheckpointRegion checkpoint)
        {
            // Update the current checkpoint
            if (_checkpointIndices[checkpoint] > _checkpointIndices[CurrentCheckpoint])
                CurrentCheckpoint = checkpoint;
        }

        public void ExitGame()
        {
            // TODO: Save game state

            Invoke(nameof(Application.Quit), 1.0f);
        }

        public void ResetGame()
        {
            var playerRb = PlayerManager.Instance.PlayerRb;
            playerRb.useGravity = true;
            playerRb.isKinematic = false;
            playerRb.velocity = Vector3.zero;
            playerRb.freezeRotation = true;
            
            PlayerManager.Instance.PlayerOrigami.ChangeForm(OrigamiContainer.OrigamiForm.None);
            PlayerManager.Instance.PlayerOrigami.ChangeForm(origamiSettings.initialForm);
            
            // Reset the player to the current checkpoint
            PlayerManager.Instance.RespawnAtLastCheckpoint();
        }

        #region Game Complete

        /// <summary>
        ///     The game is complete
        /// </summary>
        public void GameComplete()
        {
            onGameComplete.Invoke();

            StartCoroutine(FinishLevelCoroutine());
        }

        private System.Collections.IEnumerator FinishLevelCoroutine()
        {
            yield return new WaitForSeconds(3);

            _headsUpDisplay.SetActive(false);
            _finishLevelUi.SetActive(true);

            yield return new WaitForSeconds(3);

            LoadStartMenu();
        }

        #endregion
    }
}