using System;
using System.Collections;
using UnityEngine;

namespace Source.Scripts.Helpers
{
    public static class CoroutineManager
    {
        public static IEnumerator WaitThenPerform(float time,Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        public static IEnumerator WaitThenPerformUnscaled(float time, Action action)
        {
            yield return new WaitForSecondsRealtime(time);
            action?.Invoke();
        }

        public static IEnumerator DoForPeriod(float time,Action action)
        {
            float value = 0f;
            while (value < time)
            {
                value += Time.deltaTime;
                action?.Invoke();
                yield return null;
            }
        }
    }
}