using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class MultiplierAnimation : MonoBehaviour
{
    TextMeshProUGUI tmpText;
    // Start is called before the first frame update

    #region Animation

    public float maxScale;
    private float baseScale;
    public float duration;

    public Color baseColor = Color.white;
    public Color increaseColor = Color.red;
    public Color decreaseColor = Color.green;

    Sequence sequence;

    private int previousCount = 0;

    #endregion

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        tmpText.text = "X0";

        baseScale = transform.localScale.x;
        OilPipeManager.Instance.OnOilPipeCountChanged.AddListener(ChangeMultiplier);
    }

    private void OnDisable()
    {
        sequence.Kill();
    }

    private void ChangeMultiplier(int newCount)
    {
        tmpText.text = "X" + newCount;

        Color color;
        if (previousCount > newCount)
            color = decreaseColor;
        else
            color = increaseColor;

        previousCount = newCount;

        if (sequence == null || !sequence.active)
        {
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(maxScale, duration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine));

            sequence.Join(tmpText.DOColor(color, duration).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine));
        } else
        {
            sequence.Kill();
            transform.localScale = new Vector3(baseScale, baseScale, baseScale);
            tmpText.color = baseColor;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(maxScale, duration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine));
            sequence.Join(tmpText.DOColor(color, duration).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutSine));
        }
    }
}
