using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Singleton<Flower>
{
    [SerializeField] private float startSize = 1f;
    [SerializeField] private float growSizeAmount = .1f;
    private void Start()
    {
        transform.localScale = Vector3.one * startSize;
    }

    public void Grow()
    {
        transform.localScale += transform.localScale * growSizeAmount;
    }



}
