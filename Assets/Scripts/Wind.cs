using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Wind : MonoBehaviour
{
    public GameObject windBox;
    public float windStrength = 1f;
    public VisualEffect windEffect;

    public bool disableGravity = false;
    public float elapsedTime = 0;

    public CharacterMove player;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<CharacterMove>();
        }

        if(elapsedTime >= 5)
        {
            if (!windBox.activeInHierarchy)
            {
                windEffect.SendEvent("OnPlay");
            }
            else
            {
                windEffect.SendEvent("OnStop");
            }
            windBox.SetActive(!windBox.activeInHierarchy);
            elapsedTime = 0;
        }

        elapsedTime += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        if (windBox.activeInHierarchy)
        {
            switch (other.name)
            {
                case "Player":
                    other.gameObject.transform.position += windStrength * transform.forward;
                    player.SetGravity(player.Stats.gravity / 4);
                    break;
                case "Flynn":
                    other.gameObject.transform.position += windStrength * transform.forward;
                    Debug.Log("I'm hitting Flynn.");

                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player.ResetGravity();
        }
    }
}
