using UnityEngine;
using System.Collections.Generic;
using System.Collections;



enum FactoryState {Idle, Building, Processing, Crafting};




public class FactoryBase : MonoBehaviour
{



    FactoryState state;
    private ItemBase.ItemType ListOfItems;
   [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Inventory sneak peek")]
    [SerializeField] List<GameObject> OutputInventory;
   [SerializeField] List<GameObject> InputInventory;



    [Header("Internal mechanics")]
    [SerializeField] Sprite buildingSprite;
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite craftingSprite;
    [SerializeField] Sprite processingSprite;

    Sprite _currentSprite;

    [SerializeField] private float _craftingTime;
    [SerializeField] private float _tempCraftingTime;
    private float _CraftingBoosterValue = 1f;
    private bool _readyToCraft = true;
    // Ingredient amount should be at same index as ingredient list.
    [SerializeField] List<ItemBase.ItemType> _ingredientList;
    [SerializeField] List<int> _ingredientAmountForCraft;
    [SerializeField] GameObject _itemOutputType;


    [Header("Information to Construct this factory")]

    [SerializeField] List<ItemBase.ItemType> _itemListToCraftFactory;
    [SerializeField] List<int> _itemAmountToCraftFactory;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        state = FactoryState.Building;
        _tempCraftingTime = _craftingTime;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            DecreaseTempCraftingTime();
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
        InputInventory.Add(ObjectToAdd);
    }

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

    IEnumerator craft()
    {
        //not done
        ClearInputInventory();
        print("crafting started");
        state = FactoryState.Crafting;
        yield return new WaitForSeconds(_tempCraftingTime);
        CreateOutput();
        _readyToCraft = true;
        state = FactoryState.Idle;
        StopCoroutine(craft());

    }

    public void CheckForCraftingPossible()
    { // not done

        
        if (_readyToCraft)
        {
            print("test");
            if (ItemsFilled(_ingredientList, InputInventory, _ingredientAmountForCraft))
            {
                print("Check is true!");
                StartCoroutine(craft());
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

    public void DecreaseTempCraftingTime()
    {
        _CraftingBoosterValue++;
        _tempCraftingTime /= _CraftingBoosterValue;
    }
  









}
