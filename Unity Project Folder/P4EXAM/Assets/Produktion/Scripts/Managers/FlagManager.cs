using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum mode { Normal, Flag, Blueprint }
public class FlagManager : MonoBehaviour
{
    [SerializeField] public int _allowedFlagCount;
    [SerializeField] public List<Vector2> _flagPoints;
    public static bool _flagmode = false;
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
       // SetFlags();
      

    }

    private void LateUpdate()
    {
        print(_flagmode);
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
       
        if (Input.GetMouseButtonDown(0)
            && _flagmode
            && !EventSystem.current.IsPointerOverGameObject())

        {
            MouseWorldPos = InputHandler.Instance.PassMousePosInWorld();
            
            Debug.Log("Placing flag at: " + MouseWorldPos);
            _flagPoints.Add(MouseWorldPos);
            CleanUpFlags();
            
        }


        /*
        MouseWorldPos = InputHandler.Instance.PassMousePosInWorld();
        //Debug.Log("MODE IS: " + _mode.ToString());
        
        
            
            if (Input.GetMouseButtonDown(0))
            {
                    

                

                if (_flagmode == true)
                 {
                _flagPoints.Add(MouseWorldPos);
                CleanUpFlags();
                DroneManager.Instance.UpdateDroneMoves();

                 }
        }
        
        
       */

    }


    public void ChangeModeToFlagMode()
    {

        _flagmode = !_flagmode;
        Debug.Log("FlagMode is now: " + _flagmode);
        if (!_flagmode)
        {
            print("SIGMAAAA");
        }
        if (_flagmode)
        {
            print("NOT VERY SIGMAA");
        }

    }



  









}


