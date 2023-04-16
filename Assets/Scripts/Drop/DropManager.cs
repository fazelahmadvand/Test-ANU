using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropManager : Singleton<DropManager>
{
    [SerializeField] private Drop drop;
    [SerializeField] private List<int> values = new();



    [SerializeField] private List<DropHolder> tops = new();
    [SerializeField] private List<DropHolder> mids = new();
    [SerializeField] private List<DropHolder> bots = new();

    private List<DropHolder> holders = new();

    public const float DURATION = .2f;

    private void Start()
    {

        holders = new List<DropHolder>();
        holders.AddRange(bots);
        holders.AddRange(mids);
        holders.AddRange(tops);

        var im = InputManager.Instacne;
        im.OnDragDown += () =>
        {
            FillTop();
        };

        im.OnDragLeft += () =>
        {
        };

        im.OnDragRight += () =>
        {

        };



    }

    public void FillTop()
    {
        var list = new List<int>();
        foreach (var item in tops)
        {
            if (!item.HasDrop)
                list.Add(tops.IndexOf(item));
        }
        if (list.Count == 0) return;
        var rnd = Random.Range(0, list.Count);
        var valRnd = Random.Range(0, values.Count);
        tops[list[rnd]].FillDrop(drop, values[valRnd]);

    }

    public List<DropHolder> GetHolders(DropHolder holder, PositionState state)
    {
        var list = new List<DropHolder>();

        foreach (var item in holders)
        {
            if (item == holder)
                continue;

            if (state == PositionState.Down)
            {
                if (holder.transform.position.y == item.transform.position.y)
                {
                    list.Add(item);
                }
            }
            else if (state == PositionState.left)
            {
                if (holder.transform.position.y == item.transform.position.y
                    && holder.transform.position.x > item.transform.position.x)
                {
                    list.Add(item);
                }
            }
            else if (state == PositionState.Right)
            {
                if (holder.transform.position.y == item.transform.position.y
                    && holder.transform.position.x < item.transform.position.x)
                {
                    list.Add(item);
                }
            }


        }

        if (state == PositionState.Down)
            list.OrderBy(item => item.transform.position.y);
        else
            list.OrderBy(item => item.transform.position.x);


        return list;
    }

}
