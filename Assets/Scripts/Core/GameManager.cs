using System;
using System.Collections.Generic;
using EVOGAMI.Custom.Scriptable;
using EVOGAMI.Region;
using UnityEngine;
using UnityEngine.Events;

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
        
        // [Header("UI Elements")]
        // The HUD for the player
        // [SerializeField] [Tooltip("The HUD for the player")]
        // public GameObject headsUpDisplay;
        // The UI for when the player finishes the level
        // [SerializeField] [Tooltip("The UI for when the player finishes the level")]
        // private GameObject finishLevelUi;
        
        [Header("Settings")]
        // Origami related settings
        [SerializeField] [Tooltip("Origami related settings")]
        public OrigamiSettings origamiSettings;
        
        private GameObject _headsUpDisplay;
        
        /// <summary>
        ///     The indices of the checkpoints
        /// </summary>
        private Dictionary<CheckpointRegion, int> _checkpointIndices;

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
            // Get the heads-up display
            _headsUpDisplay = UiManager.Instance.headsUpDisplay;
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
            Application.Quit();
        }

        #region Game Complete
        
        [Header("End Game")]
        // Event invoked when the game is complete
        [SerializeField] [Tooltip("Event invoked when the game is complete")]
        public GameCompleteEvent onGameComplete = new();
        
        /// <summary>
        ///     Event invoked when the game is complete
        /// </summary>
        [Serializable] public class GameCompleteEvent : UnityEvent {}

        /// <summary>
        ///     The game is complete
        /// </summary>
        public void GameComplete()
        {
            onGameComplete.Invoke();
        }
        
        private System.Collections.IEnumerator FinishLevelCoroutine()
        {
            yield return new WaitForSeconds(3);
            
            _headsUpDisplay.SetActive(false);
            // finishLevelUi.SetActive(true);
        }

        #endregion
    }
}