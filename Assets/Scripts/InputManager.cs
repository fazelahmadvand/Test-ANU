using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    [SerializeField] private int dragIntensity = 150;


    private Vector3 currentTouchPos;
    [SerializeField] private bool isDragDown;
    public event Action OnDragDown;

    public event Action OnDragLeft;
    [SerializeField] private bool isDragLeft;

    public event Action OnDragRight;
    [SerializeField] private bool isDragRight;



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

            if (isDragLeft) OnDragLeft?.Invoke();
            if (isDragRight) OnDragRight?.Invoke();

            isDragDown = false;
            isDragRight = false;
            isDragLeft = false;
        }


    }

    private void HandleDown()
    {
        var diff = currentTouchPos.y - Input.mousePosition.y;

        if (MathF.Abs(diff) > dragIntensity)
        {
            if (!isDragDown) OnDragDown?.Invoke();
            isDragDown = true;
        }

    }

    private void Handle()
    {
        var diff = currentTouchPos - Input.mousePosition;

        if (Mathf.Abs(diff.x) < dragIntensity) return;

        if (diff.x > dragIntensity)
        {
            isDragLeft = true;
        }


        if (diff.x < dragIntensity)
        {
            isDragRight = true;
        }

    }


}
