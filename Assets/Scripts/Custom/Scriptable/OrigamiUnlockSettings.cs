using System.Collections.Generic;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Custom.Scriptable
{
    [CreateAssetMenu(fileName = "UnlockSettings", menuName = "Debug/UnlockSettings", order = 0)]
    public class OrigamiUnlockSettings : ScriptableObject
    {
        public List<OriBoolMapping> formUnlockStates = new List<OriBoolMapping>(5);
    }
}