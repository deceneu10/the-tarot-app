using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PulseObject : MonoBehaviour
{

    private Vector3 originalScale;
    private Vector3 finalScale;

    // Start is called before the first frame update
    void Start()
    {

        originalScale = transform.localScale;
        finalScale = originalScale * 1.1f;

        transform.DOScale(finalScale, 1.0f)
           .SetEase(Ease.InOutSine)
           .SetLoops(-1, LoopType.Yoyo);


    }


}
