
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPickUp : MonoBehaviour
{
    [SerializeField] private int endCreditSceneIndex;
    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.name=="Player"){
            RootScript.TheGameManager.CollectOrb();
           
            //do a scene transition
            SceneManager.LoadScene(endCreditSceneIndex);


        }
    } 
}
