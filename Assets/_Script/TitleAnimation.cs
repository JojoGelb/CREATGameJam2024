using DG.Tweening;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public float durationScaleIn;

    public float durationYoYo;

    private float initialScale;

    public float YoYoMaxScaleAddition = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale.x;
        Sequence s = DOTween.Sequence();


        transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        s.Append(transform.DOScale(initialScale, durationScaleIn));

        s.Append(transform.DOScale(initialScale + YoYoMaxScaleAddition, durationYoYo)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
