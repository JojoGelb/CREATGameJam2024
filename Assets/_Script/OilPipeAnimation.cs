using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPipeAnimation : MonoBehaviour
{
    public Transform pipe;

    public Vector3 startRotation = new Vector3(-115, 0, 0);
    public Vector3 endRotation = new Vector3(65, 0, 0);

    private Tween tweenRotation;

    private void OnEnable()
    {
        pipe.rotation = Quaternion.Euler(startRotation);

        // Rotate the object to the end rotation and back to the start rotation
        tweenRotation = pipe.DOLocalRotate(endRotation, 2f) // Rotate to end rotation over 2 seconds
                 .SetLoops(-1, LoopType.Yoyo) // Loop infinitely and play back and forth
                 .SetEase(Ease.InOutSine); // Smooth the rotation
    }

    private void OnDisable()
    {
        tweenRotation.Kill();
    }
}
