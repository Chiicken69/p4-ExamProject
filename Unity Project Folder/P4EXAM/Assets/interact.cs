using UnityEngine;

public class interact : MonoBehaviour
{
    private bool playerInTrigger = false;

    private bool _interact;

    public ArrowMinigame arrowMinigame;
    [SerializeField] private GameObject minigameUI;
    private FactoryBase currentFactory;

    private void Update()
    {

        _interact = InputHandler.Instance.PassInputBoolValue(1);
        if ((playerInTrigger == true && _interact == true) && !minigameUI.activeInHierarchy)
        {
            print("WHAT THE FUCK");
            arrowMinigame.StartMinigame();
            
        }
        if (currentFactory != null)
        {
            arrowMinigame.Checkstate(currentFactory);
        }

    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInTrigger = true;

            // Attempt to get the FactoryBase component from the collided object
            FactoryBase factory = other.gameObject.GetComponent<FactoryBase>();
            if (factory != null)
            {
                // Store the reference for later use
                currentFactory = factory;
            }
            else
            {
                Debug.LogWarning("FactoryBase component not found on the collided object.");
            }


        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        print("omew");
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInTrigger = false;
            currentFactory = null;
            print("i have turned off");
        }
    }
}
