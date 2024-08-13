using System;
using System.Collections;
using UnityEngine;

namespace EVOGAMI.Utils
{
    public static class CoroutineUtils
    {
        public static IEnumerator DelayAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static IEnumerator DelayActionRealtime(float delay, Action action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action();
        }

        public static IEnumerator WaitForSecondsRealtime(float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);
        }
    }
}