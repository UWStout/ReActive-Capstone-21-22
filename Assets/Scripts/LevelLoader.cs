using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles any load transition between scenes for the game
/// </summary>
public class LevelLoader : MonoBehaviour
{
   
    /// <summary>
    /// name of the level to load
    /// </summary>
    private string level;

    /// <summary>
    /// Used to load the scene asynchronously
    /// </summary>
    private AsyncOperation asyncLoad;

    public void StartLoadingScene(string scene)
    {
        level = scene;
        asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation=false;
    }

    public IEnumerator FinishLoadingScene(bool activate)
    {
        if (activate)
        {
            RootScript.UIElements.SetLoadingDone();
            asyncLoad.allowSceneActivation = true;
        }

        yield return asyncLoad;
    }

    /// <summary>
    /// Loads a scene and activates it before returning
    /// </summary>
    /// <param name="scene">Scene to load</param>
    /// <param name="activate">Activate the scene once the function finishes</param>
    /// <returns>IEnumerator for TimeQueue</returns>
    public IEnumerator DoAllToLoadScene(string scene, bool activate)
    {
        level = scene;
        asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation=false;

        if (activate)
        {
            RootScript.UIElements.SetLoadingDone();
            
            asyncLoad.allowSceneActivation = true;
            yield return asyncLoad;
        }
        else
        {
            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }
        }

        
    }

    /// <summary>
    /// Asynchronously loads the new scene based on the active scene.
    /// </summary>
    /// <returns> Returns null to pause the load function, continuing where it left off on the next frame</returns>
	IEnumerator LoadAsyncScene()
	{
        asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation=false;

        // While the new scene is still loading
        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress>=0.9f /*need to check for scene transition?*/){
                asyncLoad.allowSceneActivation=true;
            }
            yield return null;
        }
    }
}
