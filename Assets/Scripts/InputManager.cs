using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    [SerializeField] private int dragIntensity = 150;


    private Vector3 currentTouchPos;
    public event Action OnDragDown;

    public event Action OnDragLeft;

    public event Action OnDragRight;

    [SerializeField] private int dragIndex;


    private void OnDisable()
    {
        OnDragDown = null;
        OnDragLeft = null;
        OnDragRight = null;
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            currentTouchPos = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0))
        {
            HandleDown();

            Handle();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Manager.Instacne.state == GameState.WaitingInput)
            {

                if (dragIndex == 1) OnDragDown?.Invoke();
                if (dragIndex == 2) OnDragLeft?.Invoke();
                if (dragIndex == 3) OnDragRight?.Invoke();

            }

            dragIndex = 0;
        }


    }

    private void HandleDown()
    {
        var diff = currentTouchPos.y - Input.mousePosition.y;

        if (MathF.Abs(diff) > dragIntensity)
        {
            dragIndex = 1;
        }

    }

    private void Handle()
    {
        var diff = currentTouchPos - Input.mousePosition;

        if (Mathf.Abs(diff.x) < dragIntensity) return;

        if (diff.x > dragIntensity)
        {
            dragIndex = 2;
        }


        if (diff.x < dragIntensity)
        {
            dragIndex = 3;
        }

    }


}
