using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{


    public static IEnumerator Lerp(float duration, Action OnStart, Action OnLerp, Action OnEnd)
    {

        float t = 0;

        OnStart?.Invoke();
        while (t < duration)
        {
            t += Time.deltaTime;
            OnLerp?.Invoke();
            yield return null;
        }

        OnEnd?.Invoke();


    }



}
