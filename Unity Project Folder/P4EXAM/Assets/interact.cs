using UnityEngine;

public class interact : MonoBehaviour
{
    private bool playerInTrigger = false;

    private bool _interact;

    public ArrowMinigame arrowMinigame;
    [SerializeField] private GameObject minigameUI;

    private void Update()
    {

        _interact = InputHandler.Instance.PassInputBoolValue(1);
        if ((playerInTrigger == true && _interact == true)&& !minigameUI.activeInHierarchy)
        {
            print("WHAT THE FUCK");
            arrowMinigame.StartMinigame();
        }
        
    }



    void OnTriggerStay2D(Collider2D other)
    {
        print("mewo");
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInTrigger = true;

            print("pluh"+playerInTrigger);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        print("omew");
        if (other.gameObject.CompareTag("Factory"))
        {
            playerInTrigger = false;
            print("i have turned off");
        }
    }
}
