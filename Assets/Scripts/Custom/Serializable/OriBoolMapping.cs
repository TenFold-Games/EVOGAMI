using System;
using EVOGAMI.Origami;

namespace EVOGAMI.Custom.Serializable
{
    [Serializable]
    public class OriBoolMapping
    {
        public OrigamiContainer.OrigamiForm form;
        public bool isUnlocked;

        public void Deconstruct(out OrigamiContainer.OrigamiForm form, out bool isUnlocked)
        {
            form = this.form;
            isUnlocked = this.isUnlocked;
        }
    }
}