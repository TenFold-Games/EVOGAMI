using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace EVOGAMI.Utils
{
    public static class CoroutineExtensions
    {
        public static Task AsTask(this IEnumerator coroutine, MonoBehaviour runner)
        {
            var tcs = new TaskCompletionSource<bool>();
            runner.StartCoroutine(RunCoroutine(coroutine, tcs));
            return tcs.Task;
        }

        private static IEnumerator RunCoroutine(IEnumerator coroutine, TaskCompletionSource<bool> tcs)
        {
            yield return coroutine;
            tcs.SetResult(true);
        }
    }
}