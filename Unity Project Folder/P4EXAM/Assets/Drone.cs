using System.Collections;
using System.Collections.Generic;
using UnityEngine;




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

    IEnumerator MoveDrone(Vector2 PositionB)
    {
        Vector2 PositionA = this.transform.position;
        Debug.Log("MoveDrone Routine Started");
        bool _moveObjectSmoothIsRunning = true;
        float _timeElapsed = 0f;
        Vector3 pos = new Vector3();
        float durationByDistance;


        durationByDistance = Vector2.Distance(PositionA, PositionB);
        durationByDistance = durationByDistance / _speed;

        while (_timeElapsed < durationByDistance)
        {
            // pos = Vector2.Lerp(PositionA, PositionB, _timeElapsed / durationByDistance);
            pos.x = Mathf.SmoothStep(PositionA.x, PositionB.x, _timeElapsed / durationByDistance);
            pos.y = Mathf.SmoothStep(PositionA.y, PositionB.y, _timeElapsed / durationByDistance);
           // print("CoroutinePOSITION: " + pos);

            this.transform.position = pos;

            _timeElapsed += Time.deltaTime;
            yield return null;


        }

        // coroutine = null;
        droneState = DroneState.idle;
        Debug.Log("drone is at location :3");

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
                break;
        }




    }


    public void AddMoveCommand()
    {
        MoveCommands.Enqueue(MoveDrone(CalcMoveCommand()));
    }

    private void StartMoveCommand()
    {
        if (MoveCommands.Count > 0)
        {
            StartCoroutine(MoveCommands.Dequeue());
            
            droneState = DroneState.flying;
        }

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

}
