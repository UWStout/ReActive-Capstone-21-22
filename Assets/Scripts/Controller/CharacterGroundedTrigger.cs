using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundedTrigger : MonoBehaviour
{
    [SerializeField] private List<Collider> foundObjects;
    [SerializeField] private List<Collider> foundCheckpoints;

    [SerializeField] private CharacterMove parentMove;

    [SerializeField] private SoundEffectType checkpointSound;
	[SerializeField] private SoundEffectType killplaneSound;
    
    void Start()
    {
        foundObjects.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("incoming collision: "+ other.gameObject.name);
        
        switch (other.tag)
		{
            default:
                foundObjects.Add(other);
                break;
			case "Checkpoint":
				parentMove.SetRespawn(parentMove.transform.position, parentMove.transform.rotation);
                if (!foundCheckpoints.Contains(other))
                {
                    if (foundCheckpoints.Count > 0)
                    {
                        foundCheckpoints[foundCheckpoints.Count - 1].GetComponent<Animator>().SetInteger("CheckState", 2);
                        foundCheckpoints.Clear();
                    }
                    Animator checkpointAnimator = other.GetComponent<Animator>();

                    RootScript.SoundManager.PlaySound(checkpointSound, -1f, transform);

                    other.gameObject.SetActive(true);
                    if(checkpointAnimator.runtimeAnimatorController!=null) checkpointAnimator.SetInteger("CheckState", 1);
                    foundCheckpoints.Add(other);
                }
				break;
			case "Killplane":
				RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.3f, LeanTweenType.easeOutSine);
				RootScript.GlobalQueue.RespawnPlayer();
                RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToDown, 0.3f, LeanTweenType.easeInSine);
				break;
            case "SceneTrigger":
                Debug.Log("Found SceneTrigger, entering");
                other.GetComponent<LevelEndTrigger>().OnEnterStart();
                break;
		}
        
    }

    void OnTriggerExit(Collider other)
    {
        foundObjects.Remove(other);
    }

    public void ResetFoundObjects()
    {
        foundObjects.Clear();
    }

    public bool GetGrounded()
    {
        for (int i = 0; i < foundObjects.Count; i++)
        {
            if (foundObjects[i] != null)
            {
                if (!foundObjects[i].isTrigger)
                {
                    return true;
                }
            }
            else
            {
                foundObjects.RemoveAt(i);
                i--;
            }
        }

        return false;
    }
}
