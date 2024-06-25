using System;
using EVOGAMI.Custom.Enums;
using UnityEngine;

namespace EVOGAMI.Custom.Serializable
{
    [Serializable]
    public class DirObjMapping
    {
        public Directions direction;
        public GameObject gameObject;
    }
}