using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarkTrigger : MonoBehaviour
{
    public GameObject bark;
    public TMPro.TMP_Text barkText;
    public BarkPool barkPool;

    private string[] sentences;

    void Start()
    {
        sentences = barkPool.barkLines;
    }

    private void OnTriggerEnter(Collider other)
    {
        barkText.text = sentences[Random.Range(0, sentences.Length)];

        if (other.tag == "Player")
        {
            bark.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bark.SetActive(false);
        }
    }
}
