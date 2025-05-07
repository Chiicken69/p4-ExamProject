using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemBase;
using static UnityEngine.GraphicsBuffer;




enum DroneState { idle, flying, recharging, Processing }

public class Drone : MonoBehaviour
{
    private DroneState droneState;
    private Queue<IEnumerator> MoveCommands = new Queue<IEnumerator>();
    private int flagIndex;

   
    
    private SpriteRenderer _imageSpriteRenderer;
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
        StartCoroutine(PatrolFlags());
    }

    private IEnumerator PatrolFlags()
    {
        while (true)
        {
            List<Vector2> flags = FlagManager.Instance.GetFlagsForDrone(this);
            if (flags.Count == 0)
            {
                yield return null;
                continue;
            }

            // Wrap around if we've gone past the last flag
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
        float stopThreshold = 0.01f;

        while (Vector2.Distance(transform.position, target) > stopThreshold)
        {
            float remaining = Vector2.Distance(transform.position, target);
            float stoppingDist = (speed * speed) / (2f * 10f);  // 10f is accel here

            if (remaining > stoppingDist && speed < 100f)
            {
                speed += 10f * Time.deltaTime;
                speed = Mathf.Min(speed, 100f);
            }
            else
            {
                speed -= 10f * Time.deltaTime;
                speed = Mathf.Max(speed, 0f);
            }

            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        // Snap exactly
        transform.position = target;
    }


    private IEnumerator ItemTransferLogic()
    {
        
        while (true)
        {
            if (takeItem() == true)
            {
                yield return new WaitForSecondsRealtime(1.5f);
            }
            if (DepositItem() == true)
            {
                yield return new WaitForSecondsRealtime(1.5f);
            }
            yield return null;
        }
        
    }

    private bool takeItem()
    {
        if (!_carryingItem)
        {
            GameObject tempGB = FactoryManager.Instance.ReturnFactory(this.transform.position);

            if (tempGB == null)
            {
                Debug.Log("yo dawg this shit null");
                return false;
            }
            else
            {
               _Item =  tempGB.GetComponent<FactoryBase>().TakeItemFromOutputInventory();
                if (_Item != null)
                {
                    ChangeCarryingState();
                    return true;
                }
                
            }

        }
        return false;
    }

    private bool DepositItem()
    {
        if (_carryingItem)
        {
            print("yo wtf");
            GameObject tempGB = FactoryManager.Instance.ReturnFactory(this.transform.position);
          

            if (tempGB == null)
            {
                Debug.Log("this is null. Deposit");
                return false;
            }
            else if (_Item != null)
            {
                //tempGB.GetComponent<FactoryBase>().CheckAgainstRecipe(_Item)
                tempGB.GetComponent<FactoryBase>().AddItemToInventory(_Item);
               
                _Item = null;
                ChangeCarryingState();
                Debug.Log("Deposited item: " + _Item);
                return true;
            }

        }
        return false;
    }

   /* private bool IsItemInRecipe(GameObject GB)
    {
        FactoryBase FB = GB.GetComponent<FactoryBase>();

        ItemBase IB = _Item.GetComponent<ItemBase>();



        if ()
        {
            
        }
    } */

    private void ChangeCarryingState()
    {
        if (_Item != null)
        {
            _carryingItem = true;
        }
        else
        {
            _carryingItem= false;
        }
    }
}


