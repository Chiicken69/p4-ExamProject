
using System.Collections;
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
            if (!isInDialogMode) { 
            personWhoMessaged.text = currentDialog.person;
            DialogPersonText.text = currentDialog.person;
            }

            unreadMessagesText.text = $"You have {dialogData.Count} unanswered call requests.";
            if (!listenerAdded)
            {
                messageButton.onClick.AddListener(() => ShowDialog());
                listenerAdded = true;
            }
        }
        else
        {
            unreadMessagesText.text = "You have 0 unanswered call requests.";
            messageButton.onClick.RemoveAllListeners();
            listenerAdded = false;
            personWhoMessaged.text = "No one";
        }
    }

    void DialogController()
    {
        if (timeInSeconds >= 1 && !triggeredTimes.Contains(1))
        {
            EnqueueDialog("Turiel", "Hello, are you there? Ah good, i've been told to read you a few intrustions from my notebook. Hmm let's see, if u open your blueprint book by clicking on it, you should be able to scroll through it to see diffrent buildable objects, try building a factory! I reccommend zooming out before entering build mode. By the way if you just walk away from the radio it wont stop our dialog, whoevers on the radio will just... patiently wait for you to some back and press esc to close the ui, then it should check if theres anyone else thats trying to reach your frequency");
            triggeredTimes.Add(1);
        }

        if (timeInSeconds >= 3 && !triggeredTimes.Contains(2))
        {
            EnqueueDialog("Turiel", "Next up try using your drones, you should be able to move them between " + FlagManager.Instance._allowedFlagCount + " diffrent locations, by making them pass over building like factorys, they should be able to take the output and move them around into another building to craft items. For example transfering copper into a wire factory, it shoooould make wires for you to make spools and uhhh you know stuff with");
            triggeredTimes.Add(2);
        }

        if (timeInSeconds >= 4 && !triggeredTimes.Contains(3))
        {
            EnqueueDialog("Turiel", "NOW, to my favorite part, try going up to one of your factories, and press the 'e' button. THEN u should see our pattent pending effeciency module, that plays like them video games. Press the arrow keys in the shown patteren as long as u do it correctly, it should boost your machines! Also u can close the ui with esc");
            triggeredTimes.Add(3);
        }
        
        if (timeInSeconds >= 4 && !triggeredTimes.Contains(4))
        {
            EnqueueDialog("Turiel", "By the way if you ever get tired of seeing once of your placed building try holding right click next to them for 3 seconds. They will hopefully start glowing red and get outta your sights");
            triggeredTimes.Add(4);
        }
        if (timeInSeconds >= 5 && !triggeredTimes.Contains(5))
        {
            EnqueueDialog("Turiel", "Aaand that should be everything. Have fun, Turiel out!"); //add Turiel dialog to remind player to save when that feature is added
            triggeredTimes.Add(5);
        }
        if (timeInSeconds >= 30 && !triggeredTimes.Contains(6))
        {
            EnqueueDialog("Horatio", "Dammit Turiel, why are you hogging the radio before we even get the chance to explain their mission. *sigh* Alright listen up. This is Bloomridge HQ, your misson is to construct a 'Skyspindle' a flying windmill to capture wind currents from up high, It’ll be your first major energy source. From there, you’ll move to making the Helioplate solar arrays, and eventually... the Core Bioreactor, with each compleation of a major power source we will be able to provide more support through more recipies and blueprints. Remember: without power, the lifeseeds can’t germinate. No power, no future. Report back to me once the spindle is up. We’re rooting for you, literally");
            triggeredTimes.Add(6);
        }
                if (timeInSeconds >= 111 && !triggeredTimes.Contains(7))
        {
            EnqueueDialog("Kidori", "Hey. It’s Kidori. I work in the botanial sector. Just doing the check-in rounds. You still wrestling with that spindle? Thought I saw some movement on the uplink, but maybe that was just the wind… or the squirrels again. Do you have squirrels out there? Anyway. No pressure. Just wanted to say the pollen drones are holding up back here. Mostly, One of them mistook my hair for a tulip again. You know... if you get the windmill up high enough, it’ll catch steadier air, meaning more energy. Not saying you didn’t think of that. Just… you know. Elevation’s your friend. Okay. alright i’ll stop hovering, bye-bye");
            triggeredTimes.Add(7);
        }
                if (timeInSeconds >= 160 && !triggeredTimes.Contains(8))
        {
            EnqueueDialog("Turiel", "Turiel here again, yes, again. Just a gentle reminder that windmills don’t spin themselves, and blueprints don’t jump outta the page. If you’ve built it already, well... good. Great. Ignore the old man rambling. But if not, don’t make me come out there with duct tape and a ladder. You got this.");
            triggeredTimes.Add(8);
        }
        if (timeInSeconds >= 200 && !triggeredTimes.Contains(9))
        {
            EnqueueDialog("Kidori", "Me again. Just wanted to say you’re doing fine. Turiel might act like he's the whole manual wrapped in a beard, but even he needed four tries to launch his first spindle. And his caught fire. Twice. So… yeah. You’re already ahead. Just keep tweaking those blades and don’t let the creaks scare you off. Wind sings loud when it’s working right.");
            triggeredTimes.Add(9);
        }
        if (timeInSeconds >= 250 && !triggeredTimes.Contains(10))
        {
            EnqueueDialog("Turiel", "Okay, I’ll admit it. I’m just a bit bored here at HQ. It's not like I have much to do between paperwork and... more paperwork. Thought I'd give you another friendly nudge. You’re doing great, but hey—don't make me check the logs again. Come on, give me something to report back to the team. I’ll feel better. Trust me.");
            triggeredTimes.Add(10);
        }
        if (timeInSeconds >= 300 && !triggeredTimes.Contains(11))
        {
            EnqueueDialog("Horatio", "Progress report pending on that Skyspindle. You haven’t gone silent, have you? I know the terrain’s rough and the equipment’s… temperamental, but we chose you for a reason. Get that windmill operational and keep us in the loop. HQ doesn’t like flying blind.");
            triggeredTimes.Add(11);
        }
        if (timeInSeconds >= 340 && !triggeredTimes.Contains(12))
        {
            EnqueueDialog("Kidori", "You’re not just sitting there staring dramatically into the wind again, are you? I mean, if you are, that’s fine. Very poetic. Very mysterious. Ten out of ten.");
            triggeredTimes.Add(12);
        }
        if (timeInSeconds >= 380 && !triggeredTimes.Contains(13))
        {
           EnqueueDialog("Turiel", "I’m not here to babysit, alright? …That being said, if you *do* fry the battery cores on your drones, call me before Horatio finds out.");
            triggeredTimes.Add(14);
        }
        if (timeInSeconds >= 400 && !triggeredTimes.Contains(14))
        {
            EnqueueDialog("Kidori", "Quiet day here. Even the pollen drones are napping. Kind of makes me wish I was out there with you. Not that I’d be any help I’d probably just get distracted by weird plants.");
            triggeredTimes.Add(13);
        }
               if (timeInSeconds >= 430 && !triggeredTimes.Contains(15))
        {
           EnqueueDialog("Turiel", "You know, back in my first deployment I spent two days trying to power a kiln with a water pump. Didn’t work, obviously. Point is we all start somewhere. You’re doing better than I did.");
            triggeredTimes.Add(14);
        }

        if (timeInSeconds >= 460 && !triggeredTimes.Contains(16))
        {
            EnqueueDialog("Kidori", "Found a worm in my ration bar today. Named him Carl. Put him in the planter outside. I think he’s adjusting well. Anyway, hope your day's been less... wormy.");

            triggeredTimes.Add(15);
        }


    }

    IEnumerator TypeText(string fullText, float delay = 0.03f)
    {
        dialogText.text = "";
        foreach (char c in fullText)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(delay);
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
            StopAllCoroutines(); // Stop any previous typing
            StartCoroutine(TypeText(currentDialog.dialog)); // Start typing effect

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