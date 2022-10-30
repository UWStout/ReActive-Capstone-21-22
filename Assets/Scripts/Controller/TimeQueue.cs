using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeQueue : MonoBehaviour
{
    [System.Serializable]
    public struct QueueInfo
    {
        [SerializeField] private string name;
        public IEnumerator functionToCall;
        [SerializeField] private bool waitForCompletion;

        public QueueInfo(IEnumerator _function, string _name, bool _wait)
        {
            functionToCall = _function;
            name = _name;
            waitForCompletion = _wait;
        }

        public bool WaitForCompletion()
        {
            return waitForCompletion;
        }
        
    };

    [SerializeField] private QueueInfo currentInfo;
    [SerializeField] private List<QueueInfo> queue = new List<QueueInfo>();
    

    void Start()
    {
        StartCoroutine(CoroutineCoordinator());
    }

    /// <summary>
    /// Check to see if the queue is empty
    /// </summary>
    /// <returns>Whether the queue is empty</returns>
    public bool QueueEmpty()
    {
        return queue.Count == 0;
    }

    IEnumerator CoroutineCoordinator()
    {
        
        while (true)
        {
            while (queue.Count > 0)
            {
                currentInfo = queue[0];
                queue.RemoveAt(0);

                if (currentInfo.WaitForCompletion())
                {
                    yield return StartCoroutine(currentInfo.functionToCall);
                }
                else
                {
                    StartCoroutine(currentInfo.functionToCall);
                }
            }
                
            yield return null;
        }
    }

    /// <summary>
    /// Add a coroutine to the queue
    /// </summary>
    /// <param name="ienum">The function to add to the coroutine</param>
    /// <param name="_name">Optional - Name of the function</param>
    /// <param name="waitForCompletion">Optional - whether the the queue waits for the function to complete</param>
    public void AddCoroutine(IEnumerator ienum, string _name = "Unnamed Function", bool waitForCompletion = true)
    {
        queue.Add(new QueueInfo(ienum, _name, waitForCompletion));
    }

    /// <summary>
    /// Perform a transition effect on-screen
    /// </summary>
    /// <param name="t_effect"> The type of effect to use</param>
    /// <param name="duration"> The length of the transition</param>
    /// <param name="waitForCompletion"> Whether or not to wait for this effect to complete before continuing the queue</param>
    /// <param name="tweenType"> The LeanTweenType of effect the transition will have</param>
    public void QueueTransition(UIControl.TransitionEffect t_effect, float duration, LeanTweenType tweenType = LeanTweenType.linear)
    {
        if (RootScript.UIElements != null)
        {
            switch (t_effect)
            {
                default: AddCoroutine(RootScript.UIElements.PerformEffect_Instant(duration), "Transition Instant"); break;
                case UIControl.TransitionEffect.SlideInFromLeft: AddCoroutine(RootScript.UIElements.PerformEffect_SlideInFromLeft(duration, tweenType), "Transition Slide In From Left"); break;
                case UIControl.TransitionEffect.SlideOutToLeft: AddCoroutine(RootScript.UIElements.PerformEffect_SlideOutToLeft(duration, tweenType), "Transition Slide Out To Left"); break;
                case UIControl.TransitionEffect.SlideInFromRight: AddCoroutine(RootScript.UIElements.PerformEffect_SlideInFromRight(duration, tweenType), "Transition Slide In From Right"); break;
                case UIControl.TransitionEffect.SlideOutToRight: AddCoroutine(RootScript.UIElements.PerformEffect_SlideOutToRight(duration, tweenType), "Transition Slide Out To Right"); break;
                case UIControl.TransitionEffect.SlideInFromUp: AddCoroutine(RootScript.UIElements.PerformEffect_SlideInFromUp(duration, tweenType), "Transition Slide In From Up"); break;
                case UIControl.TransitionEffect.SlideOutToUp: AddCoroutine(RootScript.UIElements.PerformEffect_SlideOutToUp(duration, tweenType), "Transition Slide Out To Up"); break;
                case UIControl.TransitionEffect.SlideInFromDown: AddCoroutine(RootScript.UIElements.PerformEffect_SlideInFromDown(duration, tweenType), "Transition Slide In From Down"); break;
                case UIControl.TransitionEffect.SlideOutToDown: AddCoroutine(RootScript.UIElements.PerformEffect_SlideOutToDown(duration, tweenType), "Transition Slide Out To Down"); break;
            }
        }
    }

    /// <summary>
    /// Add a coroutine to the queue to respawn the player
    /// </summary>
    public void RespawnPlayer()
    {
        AddCoroutine(RespawnPlayerCoroutine(), "Respawn Player");
    }

    /// <summary>
    /// Wait for the currently loading scene to finish loading
    /// </summary>
    /// <param name="activate">Whether or not to activate the scene once it is finished loading</param>
    public void WaitForSceneToLoad(bool activate)
    {
        AddCoroutine(RootScript.SceneLoader.FinishLoadingScene(activate), "Waiting for [scene] to load");
    }

    /// <summary>
    /// Add a load scene to the queue
    /// </summary>
    /// <param name="scene">Scene to load</param>
    /// <param name="activate">Whether or not to activate the scene once it is finished loading</param>
    public void LoadAllOfScene(string scene, bool activate)
    {
        AddCoroutine(RootScript.SceneLoader.DoAllToLoadScene(scene, activate), "Loading all of scene: " + scene);
    }

    /// <summary>
    /// Move the camera to a specific position and rotation over time
    /// </summary>
    /// <param name="position">Target position</param>
    /// <param name="rotation">Target rotation</param>
    /// <param name="time">Transition time</param>
    public void LerpCameraToVector3(Vector3 position, Vector3 rotation, float time)
    {
        AddCoroutine(TweenAndWaitVector3(RootScript.PlayerCamera.transform, position, rotation, time, LeanTweenType.easeOutSine), "Lerp Camera To Vector3");
    }

    /// <summary>
    /// Move the camera to a specific transform over time
    /// </summary>
    /// <param name="_transform">Target transform</param>
    /// <param name="time">Transition time</param>
    public void LerpCameraToTransform(Transform _transform, float time)
    {
        AddCoroutine(TweenAndWaitVector3(RootScript.PlayerCamera.transform, _transform.position, _transform.eulerAngles, time, LeanTweenType.easeOutSine), "Lerp Camera To Transform");
    }

    /// <summary>
    /// Set whether the camera is active (movable in gameplay)
    /// </summary>
    /// <param name="active">Whether the camera will be active</param>
    public void SetCameraActive(bool active)
    {
        AddCoroutine(SetCameraActiveCoroutine(active), "Lerp Camera State To: " + (active ? "Active" : "InActive"));
    }

    public void WaitForTime(float time)
    {
        AddCoroutine(WaitForTimeCoroutine(time), "Wait for " + time + " seconds", true);
    }

    // PRIVATE

    private IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(0);
        if (RootScript.CharMove != null)
        {
            RootScript.CharMove.Respawn();
        }
    }

    public IEnumerator TweenAndWaitVector3(Transform _transform, Vector3 position, Vector3 rotation, float tweenTime, LeanTweenType tweenType)
    {
        LeanTween.move(_transform.gameObject, position, tweenTime).setEase( tweenType );
        LeanTween.rotate(_transform.gameObject, rotation, tweenTime).setEase( tweenType );
        yield return new WaitForSeconds(tweenTime);
    }

    private IEnumerator SetCameraActiveCoroutine(bool active)
    {
        RootScript.PlayerCamera.SetCameraActive(active);
        yield return null;
    }

    private IEnumerator WaitForTimeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
