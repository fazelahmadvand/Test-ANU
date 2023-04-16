using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHolder : MonoBehaviour
{
    [SerializeField] private Drop drop;
    public bool HasDrop => drop != null;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        InputManager.Instacne.OnDragDown += () =>
        {
            HandleDrop(PositionState.Down);
        };

    }



    public void FillDrop(Drop d, int val)
    {
        drop = Instantiate(d, transform.position, Quaternion.identity, transform);
        drop.InitializeDrop(val);
    }

    private async void HandleDrop(PositionState state)
    {
        if (!HasDrop) return;
        var list = DropManager.Instacne.GetHolders(this, state);
        if (list.Count == 0) return;

        foreach (var item in list)
        {
            if (!item.HasDrop & HasDrop)
                await drop.Move(DropManager.DURATION, item.transform.position);
        }


    }


    public void MoveDrop()
    {

    }

    public void RemoveDrop()
    {
        Destroy(drop.gameObject);
    }




}
