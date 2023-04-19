using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Flower : Singleton<Flower>
{
    [SerializeField] private float startSize = 1f;
    [SerializeField] private float growSizeAmount = .1f;
    [SerializeField] private float growDuration = .5f;

    [SerializeField] private int counter = 5;
    [SerializeField] private ParticleSystem ps;


    private void OnEnable()
    {
        transform.localScale = Vector3.one * startSize;
        HUDManager.Instacne.SetCounterValue(counter);
    }


    public void Grow()
    {
        var extra = transform.localScale * growSizeAmount;
        counter--;
        HUDManager.Instacne.SetCounterValue(counter);
        if (counter == 0)
            Manager.Instacne.ChangeState(GameState.Win);
        ps.Play();
        transform.DOScale(transform.localScale + extra, growDuration);
    }



}
