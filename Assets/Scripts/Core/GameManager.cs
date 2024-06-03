using System.Collections.Generic;
using EVOGAMI.Region;
using UnityEngine;

namespace EVOGAMI.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CheckpointRegion[] checkpoints;

        public CheckpointRegion currentCheckpoint;

        // TODO: Optimize this mess
        // private Dictionary<CheckpointRegion, bool> _checkpointStates;
        private Dictionary<CheckpointRegion, int> _checkpointIndices;
        public static GameManager Instance { get; private set; }

        #region Unity Functions

        public void Awake()
        {
            // Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Keep between scenes
            // DontDestroyOnLoad(this);

            // Initialize checkpoint states
            // _checkpointStates = new Dictionary<CheckpointRegion, bool>();
            _checkpointIndices = new Dictionary<CheckpointRegion, int>();
            var i = 0;
            foreach (var checkpoint in checkpoints)
            {
                // _checkpointStates.Add(checkpoint, false);
                _checkpointIndices.Add(checkpoint, i++);
            }

            // _checkpointStates[checkpoints[0]] = true;
            currentCheckpoint = checkpoints[0];
        }

        #endregion

        /// <summary>
        ///     Callback for when the player reaches a checkpoint
        /// </summary>
        /// <param name="checkpoint">The checkpoint that was reached</param>
        public void CheckpointReached(CheckpointRegion checkpoint)
        {
            // _checkpointStates[checkpoint] = true;
            if (_checkpointIndices[checkpoint] > _checkpointIndices[currentCheckpoint])
                currentCheckpoint = checkpoint;
        }

        /// <summary>
        ///     Callback for when the player loses all lives
        /// </summary>
        public void GameOver()
        {
            Debug.Log("Game Over!");
        }

        public void ExitGame()
        {
            Debug.Log("Exiting game...");
            // TODO: Save game state
            Application.Quit();
        }
    }
}