using UnityEngine;
using DG.Tweening;
using TMPro; // 追加

public class PressStartText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dotweenText;
    private const float DOTWEEN_INTERVAL = 1.0f;

    void Start()
    {
        _dotweenText.DOFade(0.0f, DOTWEEN_INTERVAL)   // アルファ値を0にしていく
            .SetLoops(-1, LoopType.Yoyo);    // 行き来を無限に繰り返す
    }
}