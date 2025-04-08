
using System.Collections.Generic;


using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ArrowMinigame : MonoBehaviour
{
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private GameObject arrow;

    [SerializeField] private Transform Parent;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private CameraZoom playerZoom;
    [SerializeField] private List<Sprite> spriteList;

    List<GameObject> cloneArrowList = new List<GameObject>();
    List<DirectionName> directionList = new List<DirectionName>();

    private bool _interact;
    private bool _closeUI;
    private Vector3 _moveDir;

    private bool currentRoundPlaying = false;
    private Sprite arrowImage;

    int direction;
    string result;
    private enum DirectionName { Up, Left, Right, Down, None }

    private int _arrowAmount = 5;
    private int currentArrowIndex = 0;




    private void Update()
    {
        GetKeyInfo();
        StartMinigame();
        if (currentRoundPlaying == true)
        {
            RunRound();
        }
    }
    private void GetKeyInfo()
    {
        _interact = InputHandler.Instance.PassInputBoolValue(1);
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);

        _moveDir = InputHandler.Instance.PassInputMoveDir();
    }

    private void StartMinigame()
    {
        if (_interact == true && !minigameUI.activeInHierarchy)
        {
            minigameUI.SetActive(true);
            playerMovement.enabled = false;
            playerZoom.enabled = false;
            CalculateArrows(true);
            Debug.Log("Opened arrowui");

        }
        else if (_closeUI == true && minigameUI.activeInHierarchy)
        {
            minigameUI.SetActive(false);
            playerMovement.enabled = true;
            playerZoom.enabled = true;
            CalculateArrows(false);
            Debug.Log("closed arrow ui");

        }
    }
    private void CalculateArrows(bool gameState)
    {
        if (gameState == true)
        {

            if (gameState == true)
            {
                float value;
                for (int i = 1; i <= _arrowAmount; i++)
                {
                    GameObject cloneArrow;
                    float min = 0f;
                    float max = _arrowAmount * 100 - 100;

                    value = i * 100 - 100; // Simulate a value in range [0, 400]

                    // Normalize and map into UI space range (-500 to +500)
                    float normalized = Mathf.InverseLerp(min, max, value);
                    float mappedX = Mathf.Lerp(-400, 400, normalized);


                    DirectionName directionEnum = (DirectionName)UnityEngine.Random.Range(0, 4);
                    arrowImage = spriteList[(int)directionEnum];



                    arrow.SetActive(true);

                    arrow.GetComponent<Image>().sprite = arrowImage;
                    cloneArrow = Instantiate(arrow);
                    // cloneArrow.transform.localScale += new Vector3(2, 2, 2);

                    cloneArrow.name = "Arrow_" + directionEnum.ToString(); // Name for debug

                    cloneArrow.transform.SetParent(Parent, false);

                    // Set UI position properly using RectTransform
                    RectTransform rt = cloneArrow.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(mappedX, 0f); // or set a Y offset if needed


                    cloneArrowList.Add(cloneArrow);
                    directionList.Add(directionEnum);

                }
            }
            currentRoundPlaying = true;
        }
        arrow.SetActive(false);
        if (gameState == false)
        {

            foreach (GameObject cloneArrow in cloneArrowList)
            {
                if (cloneArrow != null)
                {
                    Destroy(cloneArrow);
                }
            }
            cloneArrowList.Clear();
            directionList.Clear();
            currentArrowIndex = 0;

        }
    }

    private void RunRound()
    {
        if (currentArrowIndex >= directionList.Count)
        {
            Debug.Log("All inputs correct! Round finished.");
            currentRoundPlaying = false;
            return;
        }

        if (_moveDir != Vector3.zero)
        {
            if (CheckCombo(_moveDir) == directionList[currentArrowIndex])
            {
                Debug.Log("Correct input for arrow " + currentArrowIndex);
                Destroy(cloneArrowList[currentArrowIndex]);
                currentArrowIndex++;
            }
            else
            {
                Debug.Log("Incorrect input for arrow " + currentArrowIndex);
                // Optional: Add penalty logic here
            }
        }

    }



    private DirectionName CheckCombo(Vector3 input)
    {
        if (input == Vector3.up)
        { 
        return DirectionName.Up;
        }
        if (input == Vector3.left)
        {
            return DirectionName.Left;
        }
        if (input == Vector3.right)
        {
            return DirectionName.Right;
        }
        if (input == Vector3.down)
        {
            return DirectionName.Down;
        }
        return DirectionName.None;
    }
}

