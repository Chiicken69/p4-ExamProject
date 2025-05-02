using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FactoryManager : MonoBehaviour
{
    public List<Vector3Int> _factoryGridPositions;
    public GameObject[] _factories;
    [SerializeField] private Tilemap _tilemap;
    private bool _routineRunning = false;
    private int TempInt;
    



    public static FactoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance);
        }
    }


    private void Update()
    {
        if (!_routineRunning)
        {
            StartCoroutine("FactoryCheck");
        }
    }

    private void tempfuncname()
    {
        TempInt = 0;
       _factories = GameObject.FindGameObjectsWithTag("Factory");


        foreach (var factory in _factories)
        {
            for (int i = 1; i < _factoryGridPositions.Count; i++)
            {
               
                if (_factoryGridPositions[i] != ConvertToGrid(factory.transform.position)) ;
                {
                    TempInt++;
                }
            }
            if (TempInt >= _factoryGridPositions.Count)
            {
                _factoryGridPositions.Add(ConvertToGrid(factory.transform.position));
            }
        }


    }

    private Vector3Int ConvertToGrid(Vector2 pos)
    {
        return _tilemap.layoutGrid.WorldToCell(pos);
    }

    private IEnumerator FactoryCheck()
    {
        _routineRunning = true;
        yield return new WaitForSecondsRealtime(2f);


        tempfuncname();
        _routineRunning = false;
 
    }

}
