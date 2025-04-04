using UnityEngine;

public class ArrowMinigame : MonoBehaviour
{
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private Movement playerMovement;

    private bool _interact;

    private void Update()
    {
        GetKeyInfo();
        StartMinigame();
    }
    private void GetKeyInfo()
    {
        _interact = InputHandler.Instance.PassInputBoolValue();
    }
    private void StartMinigame()
    {
        if (_interact == true)
        {
            minigameUI.SetActive(true);
            playerMovement.enabled = false;
            CalculateArrows(true);
        }
        else if (_interact == true && gameObject.active == true)
        {
            minigameUI.SetActive(false);
            playerMovement.enabled = true;
            CalculateArrows(false);
        }
    }
    private void CalculateArrows(bool gameState)
    {
    }

}
