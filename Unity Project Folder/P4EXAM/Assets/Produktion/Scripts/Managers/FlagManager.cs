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
    [SerializeField] private int _allowedFlagCount;
    [SerializeField] public GameObject FlagPrefab;
    public static FlagManager Instance;
    [SerializeField] public List<Vector2> _flagPoints;
    [SerializeField] public List<GameObject> FlagObjects;
    public static bool _flagmode = false;
    public mode _mode;

[SerializeField] DroneManager droneManager;

    private Dictionary<Drone, List<Vector2>> droneFlags = new Dictionary<Drone, List<Vector2>>();  // To store flags for each drone

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
{
    // Convert mouse position to world coordinates
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // Iterate through all the drones
    foreach (GameObject drone in droneManager.drones) // assuming `allDrones` is a list or array of all drone instances
    {
        Drone addflagtodrone = drone.GetComponent<Drone>();
        // Calculate the distance between the mouse position and the drone
        float distance = Vector2.Distance(mousePos, drone.transform.position);

        // Define a click radius (e.g., 0.5f for a small area around the drone)
        float clickRadius = 0.5f;

        // If the mouse is within the click radius of the drone
        if (distance <= clickRadius)
        {
            // Add flag for the clicked drone
            AddFlagForDrone(addflagtodrone, mousePos);
            break; // Exit loop after finding the first matching drone
        }
    }
}

    }

    // Adds a flag for a specific drone
    public void AddFlagForDrone(Drone drone, Vector2 flagPosition)
    {
        if (!droneFlags.ContainsKey(drone))
        {
            droneFlags[drone] = new List<Vector2>();
        }

        // Only add a new flag if it's not exceeding the allowed count
        if (droneFlags[drone].Count < _allowedFlagCount)
        {
            droneFlags[drone].Add(flagPosition);
            DisplayFlagsForDrone(drone);
        }
    }

    // Displays flags for a specific drone
    public void DisplayFlagsForDrone(Drone drone)
    {
        if (!droneFlags.ContainsKey(drone)) return;

        // Clear existing flags
        foreach (Transform child in drone.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate flags
        foreach (var flagPos in droneFlags[drone])
        {
            Instantiate(FlagPrefab, flagPos, Quaternion.identity, drone.transform);
        }
    }

    // Gets flags assigned to a specific drone
    public List<Vector2> GetFlagsForDrone(Drone drone)
    {
        if (droneFlags.ContainsKey(drone))
        {
            return droneFlags[drone];
        }
        return new List<Vector2>();
    }
}
