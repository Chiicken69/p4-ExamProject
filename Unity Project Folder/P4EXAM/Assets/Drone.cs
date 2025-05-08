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

   
    
    private SpriteRenderer _imageSpriteRenderer;
   // [SerializeField] private float _speed;
    [SerializeField] private bool _carryingItem;
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
        StartCoroutine(ItemTransferLogic());
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

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
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
        if (_carryingItem)
        {
            _sprite = _Item.GetComponent<ItemBase>().Sprite;
            _imageSpriteRenderer.sprite = _sprite;
        }
        else _imageSpriteRenderer.sprite = null;
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
