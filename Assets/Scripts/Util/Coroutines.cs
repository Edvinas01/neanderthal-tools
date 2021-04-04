using System;
using System.Collections;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public static class Coroutines
    {
        public static IEnumerator Progress(
            float from,
            float to,
            float duration,
            Action<float> onValue
        )
        {
            onValue(from);

            var progress = 0f;
            while (progress < 1f)
            {
                var value = Mathf.Lerp(from, to, progress);
                onValue(value);

                progress += Time.unscaledDeltaTime / duration;

                yield return null;
                // yield return new WaitForEndOfFrame(); ?
            }

            onValue(to);

            yield return null;
        }
    }
}
