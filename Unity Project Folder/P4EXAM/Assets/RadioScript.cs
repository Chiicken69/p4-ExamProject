
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class RadioScript : MonoBehaviour
{
    [SerializeField] int timeInSeconds;

    private bool _closeUI;

    private bool isInDialogMode = false;

    [SerializeField] private Text unreadMessagesText;
    private int amountOfUnread;
    [SerializeField] private Text personWhoMessaged;
    [SerializeField] private GameObject dialogTextGameObject;
    [SerializeField] private GameObject dialogPersonThatMessagedGameObject;
    [SerializeField] private GameObject buttonForMessages;
    private bool listenerAdded = false;

    [SerializeField] private GameObject radioUI;
    //name,Dialog
  private Queue<DialogEntry> dialogData = new Queue<DialogEntry>();
    private HashSet<int> triggeredTimes = new HashSet<int>();

    [System.Serializable]
    public class DialogEntry
    {
        public string person;
        public string dialog;
    }

    private DialogEntry currentDialog;
    private Button messageButton;
    private Text dialogText;
    private Text DialogPersonText;

    void Start()
    {
        messageButton = buttonForMessages.GetComponent<Button>();
        dialogText = dialogTextGameObject.GetComponent<Text>();
        DialogPersonText = dialogPersonThatMessagedGameObject.GetComponent<Text>();
    }

    void Update()
    {
        timeInSeconds = Mathf.FloorToInt(Time.time);

        GetKeyInfo();
        CloseUI();
        CharPicker();
        DialogController();

        if (isInDialogMode)
        {
            ShowDialog();
        }
        else
        {
            unreadMessagesText.enabled = true;
            dialogTextGameObject.SetActive(false);
            dialogPersonThatMessagedGameObject.SetActive(false);
            buttonForMessages.SetActive(true);
        }
    }

    private void GetKeyInfo()
    {
        _closeUI = InputHandler.Instance.PassInputBoolValue(2);
    }

    void CloseUI()
    {
        if (_closeUI && radioUI.activeInHierarchy)
        {
            radioUI.SetActive(false);
            isInDialogMode = false;
        }
    }

    void CharPicker()
    {
        if (currentDialog == null && dialogData.Count > 0)
        {
            currentDialog = dialogData.Peek();
        }

        if (dialogData.Count > 0)
        {
            personWhoMessaged.text = currentDialog.person;
            DialogPersonText.text = currentDialog.person;

            unreadMessagesText.text = $"You have {dialogData.Count} unread messages.";
            if (!listenerAdded)
            {
                messageButton.onClick.AddListener(() => ShowDialog());
                listenerAdded = true;
            }
        }
        else
        {
            unreadMessagesText.text = "You have 0 Messages";
            messageButton.onClick.RemoveAllListeners();
            listenerAdded = false;
            personWhoMessaged.text = "No one";
        }
    }

    void DialogController()
    {
        if (timeInSeconds >= 1 && !triggeredTimes.Contains(1))
        {
            EnqueueDialog("Turiel", "Hello, are you there? Ah good, i've been told to read you a few intrustions from my notebook. Hmm let's see, if u open your blueprint book by clicking on it, you should be able to scroll through it to see diffrent buildable objects, try building a factory! Any will do. By the way if you just walk away from the radio it wont stop our dialog, whoevers on the radio will just... patiently wait for u to some back and press esc to close the ui, then it should check if theres anyone else thats trying to reach your frequency");
            triggeredTimes.Add(1);
        }

        if (timeInSeconds >= 3 && !triggeredTimes.Contains(2))
        {
            EnqueueDialog("Turiel", "Next up try using your drones, you should be able to move them between " + FlagManager.Instance._allowedFlagCount + " diffrent locations, by making them pass over building like factorys, they should be able to take the output and move them around into another building to craft items. For example transfering copper into a wire factory, it shoooould make wires for you to make spools and uhhh you know stuff with");
            triggeredTimes.Add(2);
        }

        if (timeInSeconds >= 4 && !triggeredTimes.Contains(3))
        {
            EnqueueDialog("Turiel", "NOW, to my favorit part, try going up to one of your factories, and press the 'e' button. THEN u should see our pattent pending effeciency module, that plays like them video games. Press the arrow keys in the shown patteren as long as u do it correctly, it should boost your machines! Also u can close the ui with esc");
            triggeredTimes.Add(3);
        }
        
        if (timeInSeconds >= 4 && !triggeredTimes.Contains(4))
        {
            EnqueueDialog("Turiel", "By the way if you ever get tired of seeing once of your placed building try holding right click next to them for 3 seconds. They will hopefully start glowing red and get outta your sights");
            triggeredTimes.Add(4);
        }
                if (timeInSeconds >= 5 && !triggeredTimes.Contains(5))
        {
            EnqueueDialog("Turiel", "AAand that should be everything. Have fun, Turiel out!"); //add Turiel dialog to remind player to save when that feature is added
            triggeredTimes.Add(5);
        }
    }

    void EnqueueDialog(string person, string dialog)
    {
        dialogData.Enqueue(new DialogEntry
        {
            person = person,
            dialog = dialog
        });

        Debug.Log($"Queued message from {person}: \"{dialog}\"");
    }

    void ShowDialog()
    {
        if (!isInDialogMode && currentDialog != null)
        {
            dialogText.text = currentDialog.dialog;
            dialogData.Dequeue();
            currentDialog = null;
            isInDialogMode = true;
        }
        else
        {
            unreadMessagesText.enabled = false;
            dialogTextGameObject.SetActive(true);
            dialogPersonThatMessagedGameObject.SetActive(true);
            buttonForMessages.SetActive(false);
        }
    }
}