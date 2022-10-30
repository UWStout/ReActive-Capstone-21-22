using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;

    public TMP_Text nameText, dialogueText;
    public TMP_Text interactText;

    private bool dialogueActive;
    private bool isPrompted;

    private Queue<string> sentences;

    private GameObject dialogueCamera;

    private Dialogue currentDialogue;
    private DialogueTrigger currentTrigger;

    public GameObject[] responseButtons;

    private GameObject mainCamera;

    public Dialogue geneIntroWithName;
    public Dialogue windNoFlynn;

    public GameObject scarfHatVillager;
    private GameObject flynn = null, flynn2 = null;

    public GameObject textEndIcon;

    public float characterTimeWait = 0.02f;

    
    string colorTag = "<color=#00000000>";

    float RolloverCharacterSpread = 10f;
    public float FadeSpeed = 50f;


    void Start()
    {
        sentences = new Queue<string>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public bool GetDialogue(){
        return dialogueActive;
    }

    private void Update()
    {
        if (dialogueActive)
        {
            if (RootScript.Input.Interact.WasPressed && !isPrompted)
            {
                DisplayNextSentence();
            }
            if (RootScript.Input.Pause.WasPressed )
            {
                EndDialogue();
            }
        }
    }

    private static void DisableMouse()
    {
        Cursor.visible = false;
    }

    // Starts the dialogue interaction
    public void StartDialogue(DialogueTrigger trigger)
    {
        dialogueActive = true;
        dialogueCamera = trigger.dialogueCamera;
        currentTrigger = trigger;
        currentDialogue = trigger.dialogue;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        dialogueUI.SetActive(true);

        isPrompted = false;
        nameText.text = currentDialogue.characterName;
        dialogueCamera.transform.localPosition = currentDialogue.cameraPos;
        dialogueCamera.transform.localEulerAngles = currentDialogue.cameraRot;

        sentences.Clear(); // Empties the sentences string queue

        //Adds each sentence string from the dialogue object to the sentences string queue
        foreach (string sentence in currentDialogue.prompts)
        {
            sentences.Enqueue(sentence);
        }

        //ToggleActiveCamera(); // Switches the active state between the main camera and dialogue camera in the hierarchy
        TogglePlayer(); // Switches the active state of the player in the hierarchy
        DisplayNextSentence();
    }

    // Displays the next sentence in the queue as the dialogue text
    // If the sentences string queue is empty and there are no responses, calls EndDialogue
    // Else if the sentences string queue is empty, display the response buttons with their respective texts
    // Else, dequeues the next string in the sentences string queue
    private void DisplayNextSentence()
    {
        textEndIcon.transform.eulerAngles = Vector3.zero;
        textEndIcon.SetActive(false);

        if (sentences.Count == 0 && currentDialogue.responses.Length == 0)
        {
            if(currentDialogue.nextPrompts.Length != 0)
            {
                UpdateCurrentDialogue(currentDialogue.nextPrompts[0]);
                DisplayNextSentence();
            }
            else
            {
                EndDialogue();
            }
        }
        else if (sentences.Count == 0)
        {
            ArrayList options = new ArrayList(currentDialogue.responses);
            EnableResponseButtons(options);
        }
        else
        {
            if (currentDialogue.soundToPlay != null)
            {
                RootScript.SoundManager.PlaySound(currentDialogue.soundToPlay, -1, transform);
            }
            
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            dialogueText.text = sentence;

            //StartCoroutine(TypeSentence(sentence)); // Animates the sentence text
            StartCoroutine(FadeInText());
        }
    }

    public void OnResponseButtonClicked(int buttonID)
    {
        Cursor.lockState=CursorLockMode.Locked;
        UpdateCurrentDialogue(currentDialogue.nextPrompts[buttonID]);
        DisableResponseButtons();
        DisplayNextSentence();
    }

    private void EnableResponseButtons(ArrayList options)
    {
        isPrompted = true;
        Cursor.lockState=CursorLockMode.Confined;

        for (int i = 0; i < options.Count; i++)
        {
            TextMeshProUGUI responseText = responseButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            responseText.text = options[i].ToString();
            
            responseButtons[i].SetActive(true);
            
        }
        //for controller support
       RootScript.UIElements.ChangeActiveButton(responseButtons[0]);
    }

    private void DisableResponseButtons()
    {
        isPrompted = false;

        foreach (GameObject button in responseButtons)
        {
            button.SetActive(false);
        }
    }

    // Animates the sentence text
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        // for each char c in string sentence do
        int index = 0;
        while (index <= sentence.Length)
        {
            dialogueText.text = sentence.Substring(0, index) + colorTag + sentence.Substring(index) + "</color>";
            index++;
            yield return new WaitForSeconds(characterTimeWait);
        }

        textEndIcon.SetActive(true);

    }

    /// <summary>
    /// Method to animate (fade in) vertex colors of a TMP Text object.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInText()
    {
        // Set the whole text transparent
        dialogueText.color = new Color
            (
                dialogueText.color.r,
                dialogueText.color.g,
                dialogueText.color.b,
                0
            );
        // Need to force the text object to be generated so we have valid data to work with right from the start.
        dialogueText.ForceMeshUpdate();
 
 
        TMP_TextInfo textInfo = dialogueText.textInfo;
        Vector3[] newVertexPositions;
        Color32[] newVertexColors;
 
        int currentCharacter = 0;
        int startingCharacterRange = currentCharacter;
        bool isRangeMax = false;

        newVertexPositions = textInfo.meshInfo[0].vertices;
 
        while (!isRangeMax)
        {
            int characterCount = textInfo.characterCount;
 
            // Spread should not exceed the number of characters.
            byte fadeSteps = (byte)Mathf.Max(1, 255 / RolloverCharacterSpread);
 
            for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
            {
                // Skip characters that are not visible (like white spaces)
                if (!textInfo.characterInfo[i].isVisible) continue;
 
                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
 
                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
 
                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
 
                // Get the current character's alpha value.
                byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);
 
                // Set new alpha values.
                newVertexColors[vertexIndex + 0].a = alpha;
                newVertexColors[vertexIndex + 1].a = alpha;
                newVertexColors[vertexIndex + 2].a = alpha;
                newVertexColors[vertexIndex + 3].a = alpha;

                // Get the vertex positions

                // Update vertex positions.
                newVertexPositions[vertexIndex + 0].y += (255-alpha)/100f;
                newVertexPositions[vertexIndex + 1].y += (255-alpha)/100f;
                newVertexPositions[vertexIndex + 2].y += (255-alpha)/100f;
                newVertexPositions[vertexIndex + 3].y += (255-alpha)/100f;
 
                if (alpha == 255)
                {
                    startingCharacterRange += 1;
 
                    if (startingCharacterRange == characterCount)
                    {
                        // Update mesh vertex data one last time.
                        dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                        textEndIcon.SetActive(true);

                        isRangeMax = true; // Would end the coroutine.
 
                        yield return new WaitForSeconds(1.0f);
 
                        // Reset the text object back to original state.
                        //dialogueText.ForceMeshUpdate();
 
                        yield return new WaitForSeconds(1.0f);
 
                        // Reset our counters.
                        currentCharacter = 0;
                        startingCharacterRange = 0;
                        
                    }
                }
            }
 
            // Upload the changed vertex colors to the Mesh.
            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
 
            if (currentCharacter + 1 < characterCount) currentCharacter += 1;
 
            yield return new WaitForSeconds(0.25f - FadeSpeed * 0.01f);
        }
    }

    // Ends the dialogue interaction
    private void EndDialogue()
    {
        DisableMouse();
        currentTrigger.EndDialogue();
        currentDialogue = null;
        dialogueUI.SetActive(false);
        DisableResponseButtons();
        TogglePlayer(); // Switches the active state of the player in the hierarchy
        //ToggleActiveCamera(); // Switches the active state between the main camera and dialogue camera in the hierarchy

        dialogueActive = false;
    }

    // Switches the active state between the main camera and dialogue camera in the hierarchy
    private void ToggleActiveCamera()
    {
        mainCamera.SetActive(!mainCamera.activeInHierarchy);
        dialogueCamera.SetActive(!dialogueCamera.activeInHierarchy);
    }

    // Switches the active state of the player in the hierarchy
    private void TogglePlayer()
    {
        RootScript.CharMove.GetPlayerModel().SetActive(!RootScript.CharMove.GetPlayerModel().activeInHierarchy);
    }

    // Returns the active state of the dialogue system
    public bool DialgoueIsActive()
    {
        return dialogueActive;
    }

    public void UpdateCurrentDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;

        sentences.Clear(); // Empties the sentences string queue

        nameText.text = currentDialogue.characterName;
        dialogueCamera.transform.localPosition = currentDialogue.cameraPos;
        dialogueCamera.transform.localEulerAngles = currentDialogue.cameraRot;
        DialogueAction(currentDialogue.actionID);
        StartCoroutine(RootScript.GlobalQueue.TweenAndWaitVector3(RootScript.PlayerCamera.transform, dialogueCamera.transform.position, dialogueCamera.transform.eulerAngles, 0.5f, LeanTweenType.easeOutSine));

        // Adds each sentence string from the dialogue object to the sentences string queue
        foreach (string sentence in currentDialogue.prompts)
        {
            sentences.Enqueue(sentence);
        }
    }

    private void DialogueAction(int actionID)
    {
        if(SceneManager.GetActiveScene().name == "BuildScene")
        {
            GameObject windBox = GameObject.Find("WindBox");
            if(actionID == 0)
            {
                Debug.Log("Dialogue event not specified. To trigger an event, change the event ID to it's corresponding value.");
            }
            else if(actionID == 1) // Learning Gene's name by talking with him will change his name in all dialogue objects.
            {
                GameObject.Find("Guide_Idle").GetComponent<DialogueTrigger>().dialogue = geneIntroWithName;
            }
            else if(actionID == 2) // Disable wind box object and script; enable Flynn
            {
                windBox.GetComponent<Wind>().windBox.SetActive(false);
                windBox.GetComponent<Wind>().windEffect.SendEvent("OnStop");
                windBox.GetComponent<Wind>().enabled = false;
                if(flynn == null)
                {
                    flynn = Instantiate(scarfHatVillager, new Vector3(140, 15.5f, 7), Quaternion.Euler(new Vector3(0, 150, 0)));
                    flynn.name = "Flynn";
                    flynn.AddComponent<Rigidbody>();
                    Destroy(flynn.GetComponent<Villager>());
                    Destroy(flynn.GetComponent<NavMeshAgent>());
                }
            }
            else if(actionID == 3) //TODO activate wind box object and script; move Flynn
            {
                windBox.GetComponent<Wind>().enabled = true;
                windBox.GetComponent<Wind>().windEffect.SendEvent("OnPlay");
                windBox.GetComponent<Wind>().elapsedTime = 0;
                windBox.GetComponent<Wind>().windBox.SetActive(true);
                GameObject.Find("Wind Sign").GetComponent<DialogueTrigger>().dialogue = windNoFlynn;
            }
            else
            {
                Debug.Log("ERROR: Dialogue event ID out of bounds.");
            }
        }
        else if(SceneManager.GetActiveScene().name == "Intro")
        {
            if(actionID == 0)
            {
                Debug.Log("Dialogue event not specified. To trigger an event, change the event ID to it's corresponding value.");
            }
            else
            {
                Debug.Log("ERROR: Dialogue event ID out of bounds.");
            }
        }
        else if(SceneManager.GetActiveScene().name == "MidpointTest")
        {
            if(actionID == 0)
            {
                Debug.Log("Dialogue event not specified. To trigger an event, change the event ID to it's corresponding value.");
            }
            else if(actionID == 1)
            {
                if (flynn2 == null)
                {
                    flynn2 = Instantiate(scarfHatVillager, new Vector3(80,40,40), Quaternion.Euler(new Vector3(0,42,0)));
                    flynn2.name = "Flynn";
                    flynn2.AddComponent<Rigidbody>();
                    Destroy(flynn.GetComponent<Villager>());
                    Destroy(flynn2.GetComponent<NavMeshAgent>());
                }
            }
            else
            {
                Debug.Log("ERROR: Dialogue event ID out of bounds.");
            }
        }
    }
}
