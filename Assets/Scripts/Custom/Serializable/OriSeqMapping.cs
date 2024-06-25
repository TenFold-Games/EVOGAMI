using System;
using System.Linq;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Origami;

namespace EVOGAMI.Custom.Serializable
{
    [Serializable]
    public class OriSeqMapping
    {
        public OrigamiContainer.OrigamiForm form;
        public Directions[] sequence;

        public override string ToString()
        {
            return sequence.Aggregate("", (current, dir) => current + (dir + ""));
        }
    }
}