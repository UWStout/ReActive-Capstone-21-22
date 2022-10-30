using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
       if(other.gameObject.tag=="Player"){
            if (this.gameObject.tag == "Battery")
            {
                RootScript.SoundManager.PlaySound(RootScript.SoundManager.FindSoundTypeByString("BatteryCollect"), -1f, transform);
                RootScript.TheGameManager.UpdateBattery();
                Destroy(gameObject);
            } else
            {
                RootScript.SoundManager.PlaySound(RootScript.SoundManager.FindSoundTypeByString("OrbCollect"), -1f, transform);
                RootScript.TheGameManager.CollectOrb();
                Destroy(gameObject);
            }
       }
   }
}
