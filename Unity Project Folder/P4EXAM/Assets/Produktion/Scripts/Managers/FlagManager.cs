using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum mode { Normal, Flag, Blueprint }
public class FlagManager : MonoBehaviour
{
    [SerializeField] public int _allowedFlagCount;
    [SerializeField] public List<Vector2> _flagPoints;

    public mode _mode;

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
        Debug.Log("MODE IS: " + _mode.ToString());
        if (_mode == mode.Flag)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                _flagPoints.Add(MouseWorldPos);
                CleanUpFlags();
                DroneManager.Instance.UpdateDroneMoves();
            }
        }
        
       
        
    }


    public void ChangeModeToFlagMode()
    {
        Debug.Log("First " + _mode.ToString());
        if (_mode == mode.Flag)
        {
            _mode = mode.Normal;
        }
        else
        {
            _mode = mode.Flag;
        }
        Debug.Log("last: "+_mode.ToString());
    }













}


