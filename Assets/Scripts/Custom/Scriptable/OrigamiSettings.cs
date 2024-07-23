using System.Collections.Generic;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Custom.Scriptable
{
    [CreateAssetMenu(fileName = "OrigamiSettings", menuName = "Settings/OrigamiSettings", order = 1)]
    public class OrigamiSettings : ScriptableObject
    {
        [Header("Form")]
        [Tooltip("The initial form of the player")]
        public OrigamiContainer.OrigamiForm initialForm;
        [Tooltip("Specify whether the form is unlocked or not at the start of the game. " +
                 "The initial form is always unlocked regardless of this setting.")]
        public List<OriBoolMapping> formUnlockStates;

        [Header("Collectibles")]
        [Tooltip("The total number of cranes in the level")]
        public int totalCranes = 4;
    }
}