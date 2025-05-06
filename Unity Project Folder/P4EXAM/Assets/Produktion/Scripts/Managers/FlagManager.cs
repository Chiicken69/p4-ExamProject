using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    [SerializeField] public int _allowedFlagCount;
    [SerializeField] public List<Vector2> _flagPoints;

    public static FlagManager Instance;
    private bool Hasrun = false;

    private void Awake()
    {
      
        
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
           
        
        

        _flagPoints = new List<Vector2>();
    }

    private Vector3 MouseWorldPos;

   



    private void Update()
    {
        SetFlags();
      

    }



    private void CleanUpFlags()
    {
        if (_flagPoints.Count > _allowedFlagCount)
        {
            //DroneManager.Instance.RemoveMoveCommands();
            _flagPoints.RemoveAt(0);
        }
    }

    private void SetFlags()
    {
        MouseWorldPos = InputHandler.Instance.PassMousePosInWorld();

        if (Input.GetMouseButtonDown(0))
        {
            _flagPoints.Add(MouseWorldPos);
            CleanUpFlags();
            DroneManager.Instance.UpdateDroneMoves();
        }
       
        
    }

    



    









}


