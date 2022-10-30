using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialogueIndicator;
    public GameObject dialogueCamera;

    private bool isInRange;
    private bool isInteracting;
    private float interactionCooldown;

    private GameObject dialogueUI;

    void Start()
    {
        dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
    }

    void LateUpdate()
    {
        interactionCooldown -= Time.deltaTime;

        if (isInRange && !isInteracting && RootScript.Input.Interact.WasPressed && !RootScript.UIElements.DialgoueIsActive() && interactionCooldown <= 0f)
        {
            RootScript.UIElements.ShowInteractText(false);
            TriggerDialogue();

            if (RootScript.UIElements.DialgoueIsActive())
            {
                RootScript.GlobalQueue.SetCameraActive(false);
                RootScript.GlobalQueue.LerpCameraToTransform(dialogueCamera.transform, 0.5f);
                RootScript.GlobalQueue.AddCoroutine(WaitForDialogueToComplete(), "Wait For Dialogue To Complete");
                RootScript.GlobalQueue.LerpCameraToTransform(RootScript.PlayerCamera.transform, 0.5f);
                RootScript.GlobalQueue.SetCameraActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRange = true;
            RootScript.UIElements.ShowInteractText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInRange = false;
            RootScript.UIElements.ShowInteractText(false);
        }
    }

    public void TriggerDialogue()
    {
        isInteracting = true;
        RootScript.UIElements.ShowInteractText(false);
        RootScript.UIElements.StartDialogue(this);
    }

    public void EndDialogue()
    {
        isInteracting = false;
        interactionCooldown = 3f;
    }

    public IEnumerator WaitForDialogueToComplete()
    {
        while (RootScript.UIElements.DialgoueIsActive())
        {
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }
}
