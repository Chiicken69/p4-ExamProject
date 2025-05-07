using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum mode { Normal, Flag, Blueprint }
public class FlagManager : MonoBehaviour
{
    [SerializeField] public int _allowedFlagCount;
    [SerializeField] public List<Vector2> _flagPoints;
    [SerializeField] public GameObject FlagPrefab;
    [SerializeField] public List<GameObject> FlagObjects;
    public  static bool _flagmode = false;
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
            DisplayFlags();
            
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

        //_flagmode = !_flagmode;
        _flagmode = !_flagmode;
        Debug.Log("FlagMode is now: " + _flagmode);
        if (!_flagmode)
        {
            print("SIGMAAAA");
           //ChangeButtonlook(Color.gray);
        }
        if (_flagmode)
        {
            print("NOT VERY SIGMAA");
           // ChangeButtonlook(Color.red);
        }

    }

    public void ChangeButtonlook()
    {
        GameObject FlagMangButton = GameObject.FindGameObjectWithTag("FlagMangButton");
        Color PassiveColor = new Color(255, 255, 255, 1f);
        Color ToggledColor = new Color(255, 255, 255, 0.75f);

        if (!_flagmode)
        {
            print("SIGMAAAA");
            FlagMangButton.GetComponent<UnityEngine.UI.Image>().color = PassiveColor ;
            //ChangeButtonlook(Color.gray);
        }
        if (_flagmode)
        {
            print("NOT VERY SIGMAA");
            FlagMangButton.GetComponent<UnityEngine.UI.Image>().color = ToggledColor;
           // ChangeButtonlook(Color.red);
        }
    }

    public void DisplayFlags()
    {
        // 1) Instantiate any new flags we need
        foreach (var pt in _flagPoints)
        {
            // only add if we don’t already have one at exactly that position
            bool alreadyExists = FlagObjects.Any(obj =>
                (Vector2)obj.transform.position == pt
            );
            if (!alreadyExists)
                FlagObjects.Add(Instantiate(FlagPrefab, pt, Quaternion.identity));
        }

        // 2) Collect indices of flag‑objects to remove
        var toRemove = new List<int>();
        for (int i = 0; i < FlagObjects.Count; i++)
        {
            Vector2 pos = FlagObjects[i].transform.position;
            // if this position is no longer in _flagPoints, mark it
            if (!_flagPoints.Contains(pos))
                toRemove.Add(i);
        }

        // 3) Destroy & remove them, iterating indices backwards
        for (int ri = toRemove.Count - 1; ri >= 0; ri--)
        {
            int idx = toRemove[ri];
            Destroy(FlagObjects[idx]);        // schedule GameObject destruction
            FlagObjects.RemoveAt(idx);        // remove from our list
        }

    }



  









}


