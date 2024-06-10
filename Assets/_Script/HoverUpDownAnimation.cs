using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverUpDownAnimation : MonoBehaviour
{
    [SerializeField]
    private float MaxMoveDistance = 1;

    [SerializeField]
    private float duration;

    private Tween tween;

    // Update is called once per frame
    void OnEnable()
    {
        tween = transform.DOLocalMoveY(MaxMoveDistance, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine); 
    }

    private void OnDisable()
    {
        tween.Kill();
        transform.localPosition = Vector3.zero;
    }
}
