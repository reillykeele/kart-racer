using System;
using System.Collections;
using UnityEngine;

namespace Util.Coroutine
{
    public static class CoroutineUtil
    {
        public static IEnumerator WaitForExecute(Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}
