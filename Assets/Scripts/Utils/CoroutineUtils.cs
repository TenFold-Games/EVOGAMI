using System.Collections;
using UnityEngine;

namespace EVOGAMI.Utils
{
    public class CoroutineUtils
    {
        public static IEnumerator DelayAction(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}