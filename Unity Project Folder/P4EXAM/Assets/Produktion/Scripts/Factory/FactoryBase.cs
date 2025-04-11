using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;



enum FactoryState {Idle, Building, Processing, Crafting};




public class FactoryBase : MonoBehaviour
{



    FactoryState state;
    private ItemBase.ItemType ListOfItems;
   [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Inventory sneak peek")]
    [SerializeField] List<GameObject> OutputInventory;
   [SerializeField] List<GameObject> InputInventory;

    [Header("Sprites for states")]

   
    [SerializeField] Sprite buildingSprite;
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite craftingSprite;
    [SerializeField] Sprite processingSprite;

    Sprite _currentSprite;

    [Header("For looking at, dont change in editor")]
    [SerializeField] private float _tempCraftingTime;
    [SerializeField] private float speedIncreasePercentage;

    [Header("Internal mechanics")]

    [SerializeField] private float _craftingTime;
    [SerializeField] private float _secondsToDecreaseSpeed;
    private float internalTimer;
    private bool _readyToCraft = true;


    [Header("Item recipe, and output type")]

    // Ingredient amount should be at same index as ingredient list.
    [SerializeField] List<ItemBase.ItemType> _ingredientList;
    [SerializeField] List<int> _ingredientAmountForCraft;
    [SerializeField] GameObject _itemOutputType;


    [Header("Factory building recipe")]

    [SerializeField] List<ItemBase.ItemType> _itemListToCraftFactory;
    [SerializeField] List<int> _itemAmountToCraftFactory;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        state = FactoryState.Building;
        _tempCraftingTime = _craftingTime;
        internalTimer = _secondsToDecreaseSpeed;
    }

    public void Update()
    {
        DecreaseCraftingSpeed();
        // temporary testing code
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseCraftingSpeed();
        }

        switch (state)
        {
            case FactoryState.Idle:
                CheckForCraftingPossible();
                break;
            case FactoryState.Building:
                //building logic
                break;
            case FactoryState.Processing:
                //processing logic
                break;
            case FactoryState.Crafting:
                //crafting logic
                break;
            default:
                break;
        }
        print(state);
        SwitchSprite(state);
        

    }

    private void SwitchSprite(FactoryState state)
    {
        switch (state)
        {
            case FactoryState.Idle:
                _currentSprite = idleSprite;
                break;
            case FactoryState.Building:
                _currentSprite = buildingSprite;
                break;
            case FactoryState.Processing:
                _currentSprite = processingSprite;
                break;
            case FactoryState.Crafting:
                _currentSprite = craftingSprite;
                break;
            default:
                _currentSprite = idleSprite;
                break;
        }
        _spriteRenderer.sprite = _currentSprite;
    }



    private void Start()
    {
        InitializeFactory();
        StartCoroutine(BuildFactory());
    }

    public IEnumerator BuildFactory()
    {
        while (true)
        {
            
            if (ItemsFilled(_itemListToCraftFactory, InputInventory, _itemAmountToCraftFactory))
            {
                
                ClearInputInventory();
                state = FactoryState.Idle;
                StopCoroutine(BuildFactory());
                break;
            }
            yield return null;
        }

        

    }

    public void InitializeFactory()
    {
        state = FactoryState.Building;
    }

    public void ClearOutputInventory()
    {
        OutputInventory.Clear();   
    }

    public void CreateOutput()
    {
        GameObject ObjectToAdd = _itemOutputType;
        ObjectToAdd.AddComponent<ItemBase>();
        OutputInventory.Add(ObjectToAdd); 
    }

    public void AddItemToInventory(GameObject ObjectToAdd)
    {
        if (ObjectToAdd = null)
        {
            print("Object to add was null");
            
        } else if (CheckAgainstRecipe(ObjectToAdd))
        {
            print("GAYYYYYYY");
        }


        InputInventory.Add(ObjectToAdd);
    }
    /// <summary>
    /// Returns true if item is in recipe, returns false if not
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private bool CheckAgainstRecipe(GameObject gameObject)
    {
        
        ItemBase.ItemType ObjItemType = gameObject.GetComponent<ItemBase.ItemType>();
        foreach (var item in _ingredientList)
        {
            if (ObjItemType == item)
            {
                return true;
            }


        }
        return false;

    }


    // other scripts call this to take items
    public GameObject TakeItemFromOutputInventory()
    {
        int i = OutputInventory.Count;

        GameObject TemporaryObject = OutputInventory[i];
        OutputInventory.RemoveAt(i);
        return TemporaryObject;
    }

    public void ClearInputInventory()
    {
        InputInventory.Clear();
    }

    IEnumerator Craft()
    {
        //not done
        ClearInputInventory();
        print("crafting started");
        state = FactoryState.Crafting;
        _tempCraftingTime = _craftingTime;

        while (_tempCraftingTime >= 0)
        {



            _tempCraftingTime -= Time.deltaTime * (1f + (speedIncreasePercentage / 100f));







            yield return null;

            if (_tempCraftingTime <= 0)
            {
                CreateOutput();
                _readyToCraft = true;
                state = FactoryState.Idle;
                StopCoroutine(Craft());
                //break;
            }

        }






    }

    public void CheckForCraftingPossible()
    { // not done

        
        if (_readyToCraft)
        {
            print("test");
            if (ItemsFilled(_ingredientList, InputInventory, _ingredientAmountForCraft))
            {
                print("Check is true!");
                StartCoroutine(Craft());
                _readyToCraft = false;
            }
           
        }
    }

    public bool ItemsFilled(List<ItemBase.ItemType> itemlist, List<GameObject> inventory, List<int> itemAmount) // hashmap when? :pleading emoji:
    {
        int i = 0;
        int k = 0;
        int TrueReturns = 0;

        foreach (var ingredient in itemlist)
        {
            foreach (var item in inventory)
            {
                ItemBase tempItemBase = item.GetComponent<ItemBase>();
                if (tempItemBase.type == ingredient) k++;


            }
            if (k >= itemAmount[i])
            {
                TrueReturns++;
            }
            i++;
            k = 0;
        }
        if (TrueReturns >= itemlist.Count)
        {
            return true;
          
        } else
        // state = FactoryState.Idle;
         return false;

    }
    
    /// <summary>
    /// Decreases the crafting time of the factory the script is on. Call this from other scripts
    /// </summary>
    public void IncreaseCraftingSpeed()
    {
        //works but needs to be adjusted later for actual gameplay, 
        // _CraftingBoosterValue++;
        // _tempCraftingTime /= _CraftingBoosterValue;
        speedIncreasePercentage += 100;


        print("Decrease temporary crafting time function was called, this should only be called once");
    }
   

   private void DecreaseCraftingSpeed()
    {
        internalTimer -= Time.deltaTime;
        if (internalTimer <= 0 && speedIncreasePercentage > 0)
        {
            speedIncreasePercentage--;
            internalTimer = _secondsToDecreaseSpeed;
        }
    }
  









}
