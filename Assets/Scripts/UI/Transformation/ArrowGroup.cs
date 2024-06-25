using System.Linq;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Custom.Serializable;
using UnityEngine;

namespace EVOGAMI.UI.Transformation
{
    public class ArrowGroup : MonoBehaviour
    {
        // The arrows to display.
        [SerializeField] [Tooltip("The arrows to display.")]
        private DirObjMapping[] arrows;

        private GameObject _currentArrow;
        private Directions _currentDirection;

        #region Unity Functions

        private void Start()
        {
            DeactivateAllArrows();

            _currentArrow = null;
            _currentDirection = Directions.None;
        }

        #endregion

        private void DeactivateAllArrows()
        {
            foreach (var dirObjMapping in arrows)
                dirObjMapping.gameObject.SetActive(false);
        }

        public void DisplayArrow(Directions direction)
        {
            if (_currentDirection == direction) return;
            
            if (direction == Directions.None)
            {
                _currentArrow?.SetActive(false);
                _currentArrow = null;
                _currentDirection = Directions.None;
                return;
            }
            
            if (arrows.All(dirObjMapping => dirObjMapping.direction != direction)) return; // Should not happen.
            
            _currentArrow?.SetActive(false);
            _currentDirection = direction;
            _currentArrow = arrows.First(dirObjMapping => dirObjMapping.direction == direction).gameObject;
            _currentArrow.SetActive(true);
        }
    }
}