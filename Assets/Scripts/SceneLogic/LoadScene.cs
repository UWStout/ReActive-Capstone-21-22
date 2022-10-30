using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public Animator transition;

    public float animatorTime;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.L)){
        LoadNextScene();

        }
    }
    private void LoadNextScene(){

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));

    }
    IEnumerator LoadLevel(int levelIndex){
        //play animation
        transition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(animatorTime);

        //load scene
        SceneManager.LoadScene(levelIndex);
    }
} 
