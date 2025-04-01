using UnityEngine;
using System.Collections.Generic;
using System.Collections;



enum FactoryState {Idle, Building, Processing, Crafting};




public class FactoryBase : MonoBehaviour
{



    FactoryState state;
    private ItemBase.ItemType ListOfItems;
    List<GameObject> OutputInventory;
   [SerializeField] List<GameObject> InputInventory;

    [Header("Ingredients")]
    private bool _readyToCraft = true;
    // Ingredient amount should be at same index as ingredient list.
    [SerializeField] List<ItemBase.ItemType> _ingredientList;
    [SerializeField] List<int> _ingredientAmountForCraft;
    [SerializeField] GameObject _itemOutputType;

    [Header("Internal mechanics")]
    [SerializeField] private float _craftingTime;





    

    public void InitializeFactory()
    {
        state = FactoryState.Building;
    }

    public void ClearOutputInventory()
    {
        OutputInventory.Clear();   
    }

    public void SetIngredientList()
    {

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
       // ClearInputInventory();
        print("crafting started");
        yield return new WaitForSeconds(_craftingTime);
        _readyToCraft = true;
        StopCoroutine(craft());

    }

    public void CheckForCraftingPossible()
    { // not done

        print("checking called");
        if (_readyToCraft)
        {
            if (ItemsFilled())
            {
                print("Check is true!");
                StartCoroutine(craft());
                _readyToCraft = false;
            }
           
        }
    }

    public bool ItemsFilled() // hashmap when :pleading emoji:
    {
        int i = 0;
        int k = 0;
        int TrueReturns = 0;

        foreach (var ingredient in _ingredientList)
        {
            foreach (var item in InputInventory)
            {
                ItemBase tempItemBase = item.GetComponent<ItemBase>();
                if (tempItemBase.type == ingredient) k++;


            }
            if (k >= _ingredientAmountForCraft[i])
            {
                TrueReturns++;
            }
            i++;
            k = 0;
        }
        if (TrueReturns >= _ingredientList.Count)
        {
            return true;
        }
        else return false;

        /*
        foreach (var item in InputInventory)
        {
            ItemBase tempItemBase = item.GetComponent<ItemBase>();

            foreach (var ingredient in _ingredientList)
            {
                if (tempItemBase.type == ingredient) k++;
            }

            if (k >= _ingredientAmountForCraft[i])
            {
                TrueReturns++;
            }

            k = 0;
        }
        */


    }
  









}
