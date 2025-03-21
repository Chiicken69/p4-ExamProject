using UnityEngine;



enum FactoryState {Idle, Processing, Crafting};



public class FactoryBase : MonoBehaviour
{
    FactoryState state;




public void InitializeFactory()
    {
        state = FactoryState.Idle;

    }









}
