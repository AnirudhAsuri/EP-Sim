using UnityEngine;
using DG.Tweening;

public class PowerUpGraphicsEffects : MonoBehaviour
{
    [SerializeField] private GameObject powerUpModel;

    [SerializeField] private float popHeightTop;
    [SerializeField] private float popHeightBottom;
    [SerializeField] private float popDuration;

    [SerializeField] private float floatDistance;
    [SerializeField] private float duration;

    private void Start()
    {
        Sequence popSequence = DOTween.Sequence();

        popSequence.Append(transform.DOLocalMoveY(popHeightTop, popDuration).SetEase(Ease.OutQuad));

        popSequence.Append(transform.DOLocalMoveY(popHeightBottom, popDuration).SetEase(Ease.InQuad));

        popSequence.OnComplete(StartIdleEffects);
    }

    private void StartIdleEffects()
    {
        transform.DOLocalMoveY(transform.localPosition.y + floatDistance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        powerUpModel.transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    void OnDestroy()
    {
        transform.DOKill();
        powerUpModel.transform.DOKill();
    }
}