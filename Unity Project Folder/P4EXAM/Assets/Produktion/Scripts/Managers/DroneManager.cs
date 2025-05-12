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
        GameObject drone = Instantiate(dronePrefab);
            drone.GetComponent<Drone>().flagPoints.Clear();
            drone.GetComponent<Drone>().flagObjects.Clear();
            drones.Add(drone);
           
           
            
            
        }
    }

    public void RemoveMoveCommands()
    {
        foreach (var item in drones)
        {
           // item.GetComponent<Drone>().RemovMoveCommand();
        }
    }

    public void SpawnDrone(GameObject gb)
    {
        float randNum = Random.Range(0, 0.5f);

        GameObject drone = Instantiate(dronePrefab, gb.transform.position + new Vector3(2 + randNum, 2 + randNum, 0), Quaternion.identity);
        drone.GetComponent<Drone>().flagPoints.Clear();
        drone.GetComponent<Drone>().flagObjects.Clear();
        drones.Add(drone);
    }





}
