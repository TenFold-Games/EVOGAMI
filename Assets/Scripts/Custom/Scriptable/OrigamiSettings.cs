﻿using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Custom.Scriptable
{
    [CreateAssetMenu(fileName = "OrigamiSettings", menuName = "Custom/OrigamiSettings", order = 0)]
    public class OrigamiSettings : ScriptableObject
    {
        public OrigamiContainer.OrigamiForm initialForm;
    }
}