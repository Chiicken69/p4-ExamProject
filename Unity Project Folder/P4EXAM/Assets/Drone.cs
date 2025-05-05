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

    [SerializeField] private Sprite _sprite;

    [Header("Motion Parameters")]
    [SerializeField] float accel = 10f;   // units/sec²
    [SerializeField] float maxSpeed = 100f;  // units/sec
    [SerializeField] float stopThreshold = 0.01f; // how close is “at target”?



    private void Start()
    {
        // Kick off the infinite patrol loop
        StartCoroutine(PatrolFlags());
    }
    private IEnumerator PatrolFlags()
    {
        int flagIndex = 0;

        while (true)
        {
            var flags = FlagManager.Instance._flagPoints;
            if (flags.Count == 0)
            {
                // no flags yet → wait a frame and retry
                yield return null;
                continue;
            }

            // wrap around if we’ve gone past the last flag
            if (flagIndex >= flags.Count)
                flagIndex = 0;

            Vector2 target = flags[flagIndex];
            yield return StartCoroutine(MoveDroneTo(target));

            flagIndex++;
        }
    }
    private IEnumerator MoveDroneTo(Vector2 target)
    {
        float speed = 0f;

        while (Vector2.Distance(transform.position, target) > stopThreshold)
        {
            float remaining = Vector2.Distance(transform.position, target);
            float stoppingDist = (speed * speed) / (2f * accel);

            // accelerate until we need to brake
            if (remaining > stoppingDist && speed < maxSpeed)
            {
                speed += accel * Time.deltaTime;
                speed = Mathf.Min(speed, maxSpeed);
            }
            else
            {
                // start decelerating
                speed -= accel * Time.deltaTime;
                speed = Mathf.Max(speed, 0f);
            }

            // Unity handles the “never overshoot” for us
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );

            yield return null;
        }

        // snap exactly
        transform.position = target;
    }
    private void Update()
    {

        switch (droneState)
        {
            case DroneState.idle:
                Debug.Log("is idle");
                
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

    private void TakeItem()
    {
        if (!_carryingItem && _speed <= 0.2)
        {
            GameObject tempGB = FactoryManager.Instance.ReturnFactory(this.transform.position);

            if (tempGB == null)
            {
                Debug.Log("yo dawg this shit null");
            }
            else
            {
                tempGB.GetComponent<FactoryBase>().TakeItemFromOutputInventory();
            }

        }
    }

    private void DepositItem()
    {
        if (_carryingItem && _speed <= 0.2)
        {
            GameObject tempGB = FactoryManager.Instance.ReturnFactory(this.transform.position);

            if (tempGB == null)
            {
                Debug.Log("yo dawg this shit null");
            }
            else
            {
                tempGB.GetComponent<FactoryBase>().TakeItemFromOutputInventory();
            }

        }
    }
}


