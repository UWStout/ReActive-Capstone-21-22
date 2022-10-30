using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class ReticleTween
    {
        [Header ("Pre-Move")]
        public Vector3 preMovePos;
        public float preMoveRot;

        [Header ("Post-Move")]
        public Vector3 postMovePos;
        public float postMoveRot;

        public LeanTweenType reticleTweenType;
        public float reticleTweenTime = 0.1f;
        public RectTransform reticlePieceRect;
    }

    [SerializeField] private List<ReticleTween> reticleTweens;

    private bool canGrapple;
    private GrapplingHook gh;

    private void UpdateGrapplingHookReference()
    {
        gh = FindObjectOfType<GrapplingHook>();
    }

    void Update()
    {
        if (gh == null) UpdateGrapplingHookReference();
        else HandleGrappleState();

    }

    /// <summary>
    /// Handle the grappling state
    /// </summary>
    private void HandleGrappleState()
    {
        if (canGrapple) // If previously highlighted grapple point
        {
            if (!(gh.CheckForGrapplePoint())) // If now not highlighting
            {
                canGrapple = false;
                foreach (ReticleTween tween in reticleTweens) TweenRectTransform(tween, false);
            }
        }
        else
        {
            if ((gh.CheckForGrapplePoint())) // If now highlighting
            {
                canGrapple = true;
                foreach (ReticleTween tween in reticleTweens) TweenRectTransform(tween, true);
            }
        }
    }

    /// <summary>
    /// Tween a ReticleTween of choice
    /// </summary>
    /// <param name="tween">tween to use</param>
    /// <param name="direction">true = focusing, false = unfocusing</param>
    private void TweenRectTransform(ReticleTween tween, bool direction)
    {
        if (direction) // Focusing
        {
            LeanTween.move(tween.reticlePieceRect, tween.postMovePos, tween.reticleTweenTime).setEase( tween.reticleTweenType );
            LeanTween.rotateZ(tween.reticlePieceRect.gameObject, tween.postMoveRot, tween.reticleTweenTime).setEase( tween.reticleTweenType );
        }
        else // Unfocusing
        {
            LeanTween.move(tween.reticlePieceRect, tween.preMovePos, tween.reticleTweenTime).setEase( tween.reticleTweenType );
            LeanTween.rotateZ(tween.reticlePieceRect.gameObject, tween.preMoveRot, tween.reticleTweenTime).setEase( tween.reticleTweenType );
        }
        
    }
}
