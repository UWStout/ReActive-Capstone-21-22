using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class BarkTriggerOrbs : MonoBehaviour
{
    [SerializeField] private GameObject barkObject;
    [SerializeField] private TMPro.TMP_Text barkText;
    [SerializeField] private GameObject onBuyDisable;
    [SerializeField] private float endPosition;

    //[SerializeField] private Image barkImage;

    [SerializeField] private int orbCost;

    private bool isInRange;

    private bool hasAlreadyBought;

    [TextArea(1, 2)]
    public string sentence;

    private void Update()
    {
        if (RootScript.Input.Interact.WasPressed && isInRange)
        {
            if (CanBuyOrb() && !hasAlreadyBought)
            {
                RootScript.TheGameManager.RemoveOrbs(orbCost);
                hasAlreadyBought = true;
                moveDoor();
                SetText("");
            }
            else
            {
                SetText("Can't Buy");
            }
            
        }

        if (onBuyDisable.gameObject.transform.position.y == endPosition && onBuyDisable.activeSelf == true)
        {
            onBuyDisable.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAlreadyBought)
        {
            SetText(sentence);

            if (other.tag == "Player")
            {
                barkObject.SetActive(true);
                isInRange = true;
            }
        }
        else
        {
            SetText("Can't Buy");
            barkObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            barkObject.SetActive(false);
            isInRange = false;
        }

    }

    private bool CanBuyOrb()
    {
        int numOrbs = RootScript.TheGameManager.GetCurrentOrbs();
        Debug.Log("user has " + numOrbs + " orbs");
        return numOrbs >= orbCost;
    }

    private void SetText(string text)
    {
        barkText.text = text;
    }

    private void moveDoor()
    {
        barkObject.SetActive(false);
        LeanTween.moveLocalY(onBuyDisable, endPosition, 3f);
    }
}