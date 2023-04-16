using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T ins;
    public static T Instacne
    {
        get
        {
            ins ??= FindObjectOfType<T>();
            return ins;
        }
    }

}
