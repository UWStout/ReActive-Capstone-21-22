using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndCreditLogic : MonoBehaviour
{
    [SerializeField]private TMP_Text orbText;

   private void Start() {
       Cursor.lockState = CursorLockMode.Confined;
       
           
       
        orbText.text="You have collected "+ RootScript.TheGameManager.GetCurrentOrbs() + " out of "+ RootScript.TheGameManager.GetMaxOrbs() + " orbs!";

        Cursor.visible=true;

   }
   public void MainMenu(){
       SceneManager.LoadScene(0);
   }

}

