using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public List<Vector2> flagPoints = new List<Vector2>();
    public List<GameObject> flagObjects = new List<GameObject>();
    public GameObject flagPrefab;
    public int maxFlagCount = 3;
    //public float moveSpeed = 2f;

    //private Queue<IEnumerator> MoveCommands = new Queue<IEnumerator>();
    //private int flagIndex;
    public GameObject _lastFactoryAccessed = null;

    private SpriteRenderer _imageSpriteRenderer;
    // [SerializeField] private float _speed;
    [SerializeField] public bool _carryingItem;
    [SerializeField] private GameObject _Item;

    [SerializeField] private Sprite _sprite;

    //  [Header("Motion Parameters")]
    //[SerializeField] float accel = 10f;   // units/sec²
    //[SerializeField] float maxSpeed = 100f;  // units/sec
    //[SerializeField] float stopThreshold = 0.01f; // how close is “at target”?

    [Header("Motion Parameters")]
    [SerializeField] float accel = 10f;   // units/sec²
    [SerializeField] float maxSpeed = 100f;  // units/sec
    [SerializeField] float stopThreshold = 0.01f; // how close is “at target”?
    float speed = 0f;

    private List<GameObject> _factoriesToUse;
    private GameObject _middleFactory; // used only in 3-factory logic
    private GameObject _lastFactory;
    [SerializeField] List<GameObject> visitedFactoriesInOrder = new List<GameObject>();
    [SerializeField] HashSet<GameObject> visitedSet = new HashSet<GameObject>();

    bool hasPatrolled = false;
    bool isRunning = false;

    [SerializeField] private float timeRemaining = 0.1f; // 10-second timer

    private void Awake()
    {
        SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var renderer in allRenderers)
        {
            if (renderer.gameObject != this.gameObject)
            {
                _imageSpriteRenderer = renderer;
                break;
            }
        }
    }

    void Start()
    {


        StartCoroutine(PatrolFlags());

        //StartCoroutine(ItemTransferLogic());
    }

    IEnumerator PatrolFlags()
    {
        while (true)
        {
            if (flagPoints.Count == 0)
            {
                yield return null;
                continue;
            }

            for (int i = 0; i < flagPoints.Count; i++)
            {
                Vector2 targetPos = flagPoints[i];
                while (Vector2.Distance(transform.position, targetPos) > 0.1f)
                {
                    yield return StartCoroutine(MoveDroneTo(targetPos));
                    yield return null;
                }

                // ✅ NEW: Check factory at the arrived flag position
                GameObject factory = FactoryManager.Instance.ReturnFactory(transform.position);
                if (factory != null)
                {
                    Debug.Log($"Factory found at {targetPos}: {factory.name}");

                    if (!visitedFactoriesInOrder.Contains(factory) && visitedFactoriesInOrder.Count < 3)
                    {
                        visitedFactoriesInOrder.Add(factory);
                        visitedSet.Add(factory);
                        Debug.Log($"Added {factory.name} to visitedFactoriesInOrder");
                    }
                }
                else
                {
                    Debug.Log($"No factory found at {targetPos}");
                    if (visitedFactoriesInOrder.Count != 2)
                        visitedFactoriesInOrder.Clear();
                    visitedSet.Clear();
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }



    private IEnumerator MoveDroneTo(Vector2 target)
    {
       

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

        Debug.Log("aaaa im tracking ejg tracker den" + hasPatrolled + "ím" + isRunning);
        //if (!hasPatrolled && !isRunning)
        {

            Debug.Log("IM NOT LYING");
            PatrolThenStartItemTransfer();
        }

        if (_carryingItem)
        {
            _sprite = _Item.GetComponent<ItemBase>().Sprite;
            _imageSpriteRenderer.sprite = _sprite;
        }
        else
        {
            _imageSpriteRenderer.sprite = null;

        }

        if (!_carryingItem && IsDroneIdle())
        {
            ResetFactoryAccess();
        }
    }

    private bool IsDroneIdle()
    {
        // Check if there is no nearby factory, or if the drone is far from its last accessed factory
        GameObject nearbyFactory = FactoryManager.Instance.ReturnFactory(this.transform.position);

        // Drone is idle if it's not at the last accessed factory and not near any factory
        if (nearbyFactory == null || nearbyFactory != _lastFactoryAccessed)
        {
            hasPatrolled = false;
            return true;
        }

        return false;
    }
    private void PatrolThenStartItemTransfer()
    {
        isRunning = true;

        if (flagPoints.Count == 0 || visitedFactoriesInOrder.Count < 2)
        {
            isRunning = false;
            return;
        }


        if (visitedFactoriesInOrder.Count == 2)
        {
            hasPatrolled = true;
            isRunning = false;
            _factoriesToUse = visitedFactoriesInOrder;  // [A, B]
            ItemTransferLogicForFactories(false);

        }
        else if (visitedFactoriesInOrder.Count == 3)
        {
            hasPatrolled = true;
            isRunning = false;
            _factoriesToUse = new List<GameObject> { visitedFactoriesInOrder[0], visitedFactoriesInOrder[2] };  // [A, C]
            _middleFactory = visitedFactoriesInOrder[1];  // optional, for skipping logic
            _lastFactory = visitedFactoriesInOrder[1];  // optional, for skipping logic
            ItemTransferLogicForFactories(true);

        }
    }







    private void ResetFactoryAccess()
    {
        // Reset the last factory accessed when the drone is idle and no longer at a factory
        _lastFactoryAccessed = null;
    }

    private void ItemTransferLogicForFactories(bool HasOver2Factorys)
    {
        timeRemaining -= Time.deltaTime;
        if (speed <= 3f)
        {
            Debug.Log("Time running!");
            if (!_carryingItem)
            {
                Debug.Log("FUCKING TAKE IT");
      
                takeItem(HasOver2Factorys);
            }
            else
            {
                DepositItem(HasOver2Factorys);
            }
        }
        else
        {
            Debug.Log("Time's up!");
            timeRemaining = 0f;
        }
    }
    private bool takeItem(bool HasOver2Factorys)
    {
        if (!_carryingItem)
        {
            GameObject tempGB = FactoryManager.Instance.ReturnFactory(this.transform.position);



            if ((tempGB == null || IsSameFactory(tempGB)) && !(HasOver2Factorys == true && tempGB == _middleFactory) || IsDroneIdle())
            {
                return false;
            }

            _Item = tempGB.GetComponent<FactoryBase>().TakeItemFromOutputInventory();

            if (_Item != null)
            {
                _lastFactoryAccessed = tempGB;  // update here after success
                ChangeCarryingState();
                return true;
            }

        }
        return false;
    }


    private bool DepositItem(bool HasOver2Factorys)
    {
        if (_carryingItem)
        {
            GameObject tempGB = FactoryManager.Instance.ReturnFactory(this.transform.position);

            if (tempGB == null || IsSameFactory(tempGB))
            {
                return false;
            }

            tempGB.GetComponent<FactoryBase>().AddItemToInventory(_Item);
            _Item = null;
            ChangeCarryingState();

            _lastFactoryAccessed = tempGB;  // update here

            return true;
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
            _carryingItem = false;
        }
    }
    /*
    private bool IsSameFactory(GameObject Factory)
    {
        return Factory != null && Factory == _lastFactoryAccessed;
    }

    */

    /// <summary>
    /// Returns true if the factory under the drone is the same as the last one it
    /// accessed; returns false if it’s a new (different) factory, and records it.
    /// </summary>
    private bool IsSameFactory(GameObject Factory)
    {
        // no factory under us? just say “not same” but don't clear the cache
        if (Factory == null)
            return false;

        // same factory as last time?
        if (Factory == _lastFactoryAccessed)
            return true;

        // a new factory—remember it, and say “not same”
        _lastFactoryAccessed = Factory;
        return false;
    }

}