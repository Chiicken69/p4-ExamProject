
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class RadioScript : MonoBehaviour
{
    [SerializeField] int timeInSeconds;
    
     int StarttimeInSeconds;

    private bool _closeUI;

    [SerializeField] Drone drone;

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

    [SerializeField] private GameObject speachBoble;

    [SerializeField] bool tutorielDone = false;

    [SerializeField] FactoryManager factoryManager;
    [SerializeField] ArrowMinigame arrowMinigame;
    [SerializeField] BuildingDeleter buildingDeleter;



    bool factoryBuildTutorielDone = false;
    bool dronesTutorielDone = false;
    bool minigameTutorielDone = false;
    bool deleteingTutorielDone = false;

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
            AudioManager.Instance.PlaySFX("ButtonClick");
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
            speachBoble.SetActive(true);
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
            speachBoble.SetActive(false);
            messageButton.onClick.RemoveAllListeners();
            listenerAdded = false;
            personWhoMessaged.text = "No one";
        }
    }

    void Dequeue()
    {
        dialogData.Dequeue();
        currentDialog = null;
    }

    void TutorielController()
    {
        // First tutorial dialog
        if (!triggeredTimes.Contains(1))
        {
            EnqueueDialog("Turiel",
                "Hello, are you there?\nAh good. I've been told to read you a few instructions from my notebook.\nHmm let's see..... \n" +
                "If you open your blueprint book by clicking on it, you should be able to scroll through it to see different buildable objects. Try building a wire factory! I recommend zooming out before entering build mode.\n" +
                "By the way, if you just walk away from the radio, it won't stop our dialogue; whoever's on the radio will just... patiently wait for you to come back and press the *ESC* button to close the UI. " +
                "ONLY THEN will the radio check if there's anyone else that's trying to reach your frequency.");
            triggeredTimes.Add(1);
            return;
        }

        // Factory tutorial: Check if factories are built
        if (!factoryBuildTutorielDone)
        {
            if (factoryManager.Factories.Count > 0)
            {
                factoryBuildTutorielDone = true;
            }
            return;
        }

        // Skip to the next part when the factory tutorial is done
        if (!triggeredTimes.Contains(91))
        {
            Dequeue();
            triggeredTimes.Add(91);
            return;
        }
        // Dialog for Turiel about using drones
        if (!triggeredTimes.Contains(3))
        {
            EnqueueDialog("Turiel",
                "Next up, try using your drones. \nYou should be able to activate flag managing mode by clicking on the other button. \n" +
                $"You will be able to move drones between {drone.maxFlagCount} different locations, by making them pass over buildings like factories. \n" +
                "They should be able to take the output and move it around into another building to craft items. \n" +
                "For example, transferring copper into a wire factory should make wires for you to make spools and other parts.");
            triggeredTimes.Add(3);
            return;
        }

        // Check if drone tutorial is done
        if (!dronesTutorielDone)
        {
            if (drone.maxFlagCount > 0 && drone._carryingItem)
            {
                dronesTutorielDone = true;
            }
            return;
        }

        // Skip to the next part when drone tutorial is done
        if (!triggeredTimes.Contains(93))
        {
            Dequeue();
            triggeredTimes.Add(93);
            return;
        }
        // Dialog for Turiel about the efficiency module
        if (!triggeredTimes.Contains(2))
        {
            EnqueueDialog("Turiel",
                "NOW, to my favorite part! \nTry going up to the copper farm, or one of your factories and press the 'e' button. \n" +
                "THEN you should see our patent-pending efficiency module that plays like those video games. Press the arrow keys in the shown pattern; as long as you do it correctly, it should boost your machines!");
            triggeredTimes.Add(2);
            return;
        }

        // Check if the minigame tutorial is done
        if (!minigameTutorielDone)
        {
            if (arrowMinigame.minigameTutorielDone)
            {
                minigameTutorielDone = true;
            }
            return;
        }

        // Skip to the next part when the minigame tutorial is done
        if (!triggeredTimes.Contains(92))
        {
            Dequeue();
            triggeredTimes.Add(92);
            return;
        }
        // Dialog for Turiel about deleting buildings
        if (!triggeredTimes.Contains(4))
        {
            EnqueueDialog("Turiel",
                "By the way, if you ever get tired of seeing one of your placed buildings, try holding right-click while standing next to them for 3 seconds. " +
                "They will hopefully start glowing red and get out of your sight.");
            triggeredTimes.Add(4);
            return;
        }

        // Check if deleting tutorial is done
        if (!deleteingTutorielDone)
        {
            if (buildingDeleter.deleterTutorielDone == true)
            {
                deleteingTutorielDone = true;
            }
            return;
        }

        // Skip to the final part when deleting tutorial is done
        if (!triggeredTimes.Contains(94))
        {
            Dequeue();
            triggeredTimes.Add(94);
            return;
        }

        // Final dialog from Turiel
        if (!triggeredTimes.Contains(5))
        {
            EnqueueDialog("Turiel", "Aaand that should be everything. \nHave fun, \nTuriel out!");
            triggeredTimes.Add(5);
            tutorielDone = true;
            return;
        }
    }

    void DialogController()
    {

            if (!tutorielDone)
            {
                TutorielController();
                StarttimeInSeconds = Mathf.FloorToInt(Time.time);
            }
            if (tutorielDone)
        {
            timeInSeconds = Mathf.FloorToInt(Time.time) - StarttimeInSeconds;

            if (!triggeredTimes.Contains(6))
            {
                //EnqueueDialog("Horatio", "Dammit Turiel, why are you hogging the radio before we even get the chance to explain them whats going on? \n*sigh* If you forgot mission is to construct a 'Skyspindle,' a flying windmill to capture wind currents from up high. It’ll be your first major energy source. From there, you’ll move to making the Helioplate solar arrays, and eventually... the Core Bioreactor, with each completion of a major power source we will be able to provide more support through more recipes and blueprints. Remember: without power, the lifeseeds can’t germinate. No power, no future. Report back to me once the spindle is up. We’re rooting for you, literally.");
                EnqueueDialog("Horatio", "Dammit Turiel, why are you hogging the radio before we even get the chance to explain them whats going on? \n*sigh* \nIf you forgot, your job is to construct a 'Skyspindle,' a flying windmill to capture wind currents from up high. It’ll be your first major energy source, but more on that later.");
                triggeredTimes.Add(6);
            }

            if (timeInSeconds >= 261 && !triggeredTimes.Contains(7))
            {   // This one is there no point to having tbh
                //EnqueueDialog("Kidori", "Hey. It’s Kidori, from the botanical sector. Just doing the check-in rounds. You still wrestling with that spindle? Thought I saw some movement on the uplink, but maybe that was just the wind… or the squirrels again. Do you have squirrels out there? \nAnyway. No pressure. Just wanted to say the pollen drones are holding up back here. Mostly, one of them mistook my hair for a tulip again. You know... if you get the windmill up high enough, it’ll catch steadier air, meaning more energy. Not saying you didn’t think of that. Just… you know. Elevation’s your friend. Okay. Alright, I’ll stop hovering, bye-bye.");
                EnqueueDialog("Kidori", "Hey. It’s Kidori, i work with the gardens back here in case you were curious\n Anyway thought i should check in on you, since you you've been out for a bit. Just wanted to say the pollen drones are holding up back here. Mostly, one of them mistook my hair for a tulip again. Hopefully yours are doing better, though i hope you'll come see them some time sometime, bye-bye for now tho.");
                triggeredTimes.Add(7);
            }
            if (timeInSeconds >= 200 && !triggeredTimes.Contains(8))
            {   // Seems passive aggressive. Not sure if I like it.
                //EnqueueDialog("Turiel", "Turiel here again, yes, again. \nJust a gentle reminder that windmills don’t spin themselves, and blueprints don’t jump outta the page. If you’ve built it already, well... good. Great. Ignore the old man rambling. But if not, don’t make me come out there with duct tape and a ladder. You got this.");
                //triggeredTimes.Add(8);
            }
            if (timeInSeconds >= 420 && !triggeredTimes.Contains(9))
            {
                //EnqueueDialog("Kidori", "Me again. Just wanted to say you’re doing fine. Turiel might act like he's the whole manual wrapped in a beard, but even he needed four tries to launch his first spindle. And his caught fire. Twice. So… yeah. You’re already ahead. Just keep tweaking those blades and don’t let the creaks scare you off. Wind sings loud when it’s working right.");
                // The problem is that i didn't like kidoris other chat, so this is her first one which is a problem. 
                // I like this one, to remember that they are not alone and that they are doing fine, but im not sure what the relationsship between The player and Kidori should be.
                //EnqueueDialog("Kidori", "Me again. Just wanted to say you’re doing fine. Turiel might act like he's the whole manual wrapped in a beard, but even he needed four tries to launch his first spindle. And his caught fire. Twice. So… yeah. You’re already ahead. Just keep tweaking those blades and don’t let the creaks scare you off. Wind sings loud when it’s working right.");
                //EnqueueDialog("Kidori", "Hi, it's me Kidori, your smaller self! \nCan you hear me??!? \nJust wanted to join in on the fun and say you’re doing great. \nTuriel might act like he's the whole manual wrapped in a beard, but even he needed four tries to launch his first spindle. And his caught fire.... Twice. So... yeah. \nYou’re already ahead. Just keep tweaking those blades and don’t let the creaks scare you off. Wind sings loud when it’s working right.");
                EnqueueDialog("Kidori", "Heyoo, it's me again. \nJust wanted to join in on the fun and say you’re doing great. \nTuriel might act like he's the whole manual wrapped in a beard, but even he needed four tries to launch his first spindle. And his caught fire.... Twice. So... yeah. \nYou’re already ahead. Just keep tweaking those blades and don’t let the creaks scare you off. Wind sings loud when it’s working right.");
                triggeredTimes.Add(9);
            }
            if (timeInSeconds >= 300 && !triggeredTimes.Contains(10))
            {   // Passive agressive.
                //EnqueueDialog("Turiel", "Okay, I’ll admit it. I’m just a bit bored here at HQ. It's not like I have much to do between paperwork and... more paperwork. Thought I'd give you another friendly nudge. You’re doing great, but hey—don't make me check the logs again. Come on, give me something to report back to the team. I’ll feel better. Trust me.");
                //triggeredTimes.Add(10);
            }
            if (timeInSeconds >= 400 && !triggeredTimes.Contains(11))
            {
                // Agressive
                //EnqueueDialog("Horatio", "Progress report pending on that Skyspindle. You haven’t gone silent, have you? I know the terrain’s rough and the equipment’s… temperamental, but we chose you for a reason. Get that windmill operational and keep us in the loop. HQ doesn’t like flying blind.");
                //triggeredTimes.Add(11);
            }
            if (timeInSeconds >= 520 && !triggeredTimes.Contains(12))
            {   // Passive agressive
                //EnqueueDialog("Kidori", "You’re not just sitting there staring dramatically into the wind again, are you? I mean, if you are, that’s fine. Very poetic. Very mysterious. Ten out of ten.");
                //triggeredTimes.Add(12);
            }
            if (timeInSeconds >= 550 && !triggeredTimes.Contains(13))
            {
                // I think i like the though behind it, but not alle of the content.
                // But im not sure what we want there instead. 
                EnqueueDialog("Turiel", "I’m not here to babysit, alright? …That being said, if you *do* fry the battery cores on your drones, call me before Horatio finds out.");
                triggeredTimes.Add(13);
            }
            if (timeInSeconds >= 700 && !triggeredTimes.Contains(14))
            {   // This one is nice. If we get to introdduce Kidori.
                EnqueueDialog("Kidori", "Quiet day here. Even the pollen drones are napping. Kind of makes me wish I was out there with you. Not that I’d be any help I’d probably just get distracted by weird plants.");
                triggeredTimes.Add(14);
            }
            if (timeInSeconds >= 800 && !triggeredTimes.Contains(15))
            {   // done like this one. Its kinda agressive. 
                //how it is agressive its supposed to be him trying be relateable
                //EnqueueDialog("Turiel", "You know, back in my first deployment I spent two days trying to power a kiln with a water pump. Didn’t work, obviously. Point is we all start somewhere. You’re doing better than I did.");
                //triggeredTimes.Add(15);
            }

            if (timeInSeconds >= 840 && !triggeredTimes.Contains(16))
            {   // I like this one. 
                EnqueueDialog("Kidori", "Found a worm in my ration bar today. \nNamed him Carl... \nI put him in the planter outside, and it appears he’s adjusting well. \nAnyway, hope your day's been less... wormy.");

                triggeredTimes.Add(16);
            }
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

            if (tutorielDone) {
                Dequeue();
            }

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