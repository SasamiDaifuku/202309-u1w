using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TitleRanking : MonoBehaviour
{
    //[SerializeField] private CanvasGroup canvasGroupSound;
    [SerializeField] private CustomButton rankingButton;
    //[SerializeField] private CustomButton resetButton;
    //[SerializeField] private CustomButton closeButton;

    [SerializeField] private TitleRankingManager titleRankingManager;

    
    private void Start()
    {
        InitializeSetting();
        
        //クリックイベントを購読
        rankingButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => RankingButtonClickEvent())
            .AddTo(this);
        /*
        resetButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => ResetButtonClickEvent())
            .AddTo(this);
        closeButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => CloseButtonClickEvent())
            .AddTo(this);
            */

    }

    private void InitializeSetting()
    {
        rankingButton.SetActive(true);
    }

    private void CloseButtonClickEvent()
    {
        InitializeSetting();
    }

    private void RankingButtonClickEvent()
    {
        rankingButton.SetActive(false);
        titleRankingManager.OnRankingButtonClicked();
    }
    
}
