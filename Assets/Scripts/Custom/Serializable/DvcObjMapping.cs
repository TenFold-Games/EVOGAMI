using System;
using EVOGAMI.Core;
using EVOGAMI.Custom.Enums;
using UnityEngine;

namespace EVOGAMI.Custom.Serializable
{
    [Serializable]
    public class DvcObjMapping
    {
        public InputManager.InputDeviceScheme device;
        public GameObject gameObject;
    }
}
