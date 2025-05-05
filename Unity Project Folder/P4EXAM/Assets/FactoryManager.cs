using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;


public class FactoryManager : MonoBehaviour
{
    public List<Vector3Int> FactoryGridPositions;
    public List<GameObject> Factories;
    
    [SerializeField] private Tilemap _tilemap;
    float timer = 5f;
    
    



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
       timer -= Time.deltaTime;
        if (timer <= 0)
        {
            tempName();
            timer = 5f;
        }
    }

    private void tempName()
    {
        MonoBehaviour[] Behaviours = Object.FindObjectsByType<FactoryBase>(FindObjectsSortMode.None);

        foreach (var item in Behaviours)
        {
           
            Factories.Add(item.gameObject);  
        }
    }

    public void AddFactory(GameObject gb)
    {
        Factories.Add(gb);
        //_factoryGridPositions.Add(ConvertToGrid(gb.transform.position));

    }
    [ContextMenu("pluh")]

    public GameObject ReturnFactory(Vector3 DronePos)
    {
       
        if (CheckNearFactory(DronePos) != -1)
        {
            return Factories[CheckNearFactory(DronePos)]; 
        }
        return null;    
    }

    private int CheckNearFactory(Vector3 DronePos)
    {
        DronePos = ConvertToGrid(DronePos);
        int i = 0;
        foreach (var FactoryPosition in FactoryGridPositions)
        {
            if (DronePos == FactoryPosition)
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    private Vector3Int ConvertToGrid(Vector2 pos)
    {
        return _tilemap.layoutGrid.WorldToCell(pos);
    }

    

}
