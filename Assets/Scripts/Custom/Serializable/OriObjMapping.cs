using System;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Custom.Serializable
{
    /// <summary>
    ///     Serializable class that maps an Origami form to a GameObject.
    /// </summary>
    [Serializable]
    public class OriObjMapping
    {
        public OrigamiContainer.OrigamiForm form;
        public GameObject gameObject;
    }
}