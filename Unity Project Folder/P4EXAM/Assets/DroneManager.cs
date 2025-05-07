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

     [SerializeField] public List<GameObject> drones;

   

    private void Update() // curently for testing
    {
        //InputHandler.Instance.PassInputBoolValue(3)
        if (Input.GetKeyDown(KeyCode.O))
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
