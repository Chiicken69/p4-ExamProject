using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private Vector3 MouseWorldPos;

    [SerializeField] private int _allowedFlagCount;
    [SerializeField] private List<Vector2> _flagPoints;




    private void Update()
    {
        SetFlags();
        
    }



    private void CleanUpFlags()
    {
        if (_flagPoints.Count > _allowedFlagCount)
        {
            _flagPoints.RemoveAt(0);
        }
    }

    private void SetFlags()
    {
        MouseWorldPos = InputHandler.Instance.PassMousePosInWorld();

        if (Input.GetMouseButtonDown(0))
        {
            _flagPoints.Add(MouseWorldPos);
        }
        CleanUpFlags();
    }

    



    









}


