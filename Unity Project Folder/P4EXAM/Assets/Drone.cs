using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;




enum DroneState { idle, flying, recharging, Processing }

public class Drone : MonoBehaviour
{
    private DroneState droneState;
    private Queue<IEnumerator> MoveCommands = new Queue<IEnumerator>();
    private int flagIndex;

    [SerializeField] private float _speed;
    [SerializeField] private bool _carryingItem;
    [SerializeField] private GameObject _Item;

    //private bool moveObjectSmoothIsRunning;
    //private bool _timeElapsed

    [SerializeField] private Sprite _sprite;

    private void Start()
    {
       
    }

    IEnumerator MoveDrone(Vector2 target)
    {
        float accel = 10f;   // units/sec²
        float maxSpeed = 100f;   // units/sec
        float speed = 0f;   // current speed
        const float stopThreshold = 0.01f;

        while (true)
        {
            float remaining = Vector2.Distance(transform.position, target);
            if (remaining <= stopThreshold)
                break;  // only stop when you’re actually at (or very near) the target

            // distance needed to brake to zero: d = v² / (2·a)
            float stoppingDist = (speed * speed) / (2f * accel);

            // accelerate if you can still cruise, otherwise decelerate
            if (remaining > stoppingDist && speed < maxSpeed)
            {
                speed += accel * Time.deltaTime;
                speed = Mathf.Min(speed, maxSpeed);
            }
            else
            {
                speed -= accel * Time.deltaTime;       // decelerate by the same accel
                speed = Mathf.Max(speed, 0f);
            }

            // move towards the target at your computed speed
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );

            yield return null;
        }

        // snap exactly
        transform.position = target;
        droneState = DroneState.idle;
        Debug.Log($"Drone arrived at {target}");
       
    }

    private void Update()
    {

        switch (droneState)
        {
            case DroneState.idle:
                Debug.Log("is idle");
                StartMoveCommand();
                break;

            case DroneState.flying:
                Debug.Log("is flying");
                break;
            case DroneState.recharging:
                //c
                break;
                case DroneState.Processing:
                //d
            default:
                droneState = DroneState.idle;
                Debug.Log("is idle");
                StartMoveCommand();
                break;
        }




    }


    public void AddMoveCommand()
    {
        MoveCommands.Enqueue(MoveDrone(CalcMoveCommand()));
        print("Queue length: " + MoveCommands.Count);
    }

    private void StartMoveCommand()
    {
        if (MoveCommands.Count > 0)
        {
            IEnumerator temp = MoveCommands.Dequeue();
            StartCoroutine(temp);
           // MoveCommands.Enqueue(temp);
            
            droneState = DroneState.flying;
        }

    }

    public void RemovMoveCommand()
    {
        MoveCommands.Dequeue();
    }

    

    private Vector2 CalcMoveCommand()
    {

        if (flagIndex >= FlagManager.Instance._flagPoints.Count)
        {
            flagIndex = 0;
        }

        
        print("flag index is: " + flagIndex);
        print("Flagpoints is: " + FlagManager.Instance._flagPoints.Count);
        

        
        Vector2 posB = FlagManager.Instance._flagPoints[flagIndex];
        flagIndex++;
        return posB;
        
    }

    private void TakeItem()
    {
        if (!_carryingItem && _speed <= 0.2)
        {
            
        }
    }

   


}
