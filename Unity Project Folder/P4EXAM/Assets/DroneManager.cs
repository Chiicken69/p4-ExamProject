using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{


    public static DroneManager Instance;

    [SerializeField] private GameObject dronePrefab;


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
    }

    [SerializeField] List<GameObject> drones;

   

    private void Update() // curently for testing
    {
        if (InputHandler.Instance.PassInputBoolValue(3))
        {
            drones.Add(Instantiate(dronePrefab));
           
            
            
        }
    }

    public void RemoveMoveCommands()
    {
        foreach (var item in drones)
        {
           // item.GetComponent<Drone>().RemovMoveCommand();
        }
    }





}
