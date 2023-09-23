using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroupSound;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    [SerializeField] private TitleController titleController;
    
    private void Awake()
    {
        InitializeSetting();
       
        //クリックイベントを購読
        soundButton.onClick.AsObservable()
            .Subscribe(_ => SoundButtonClickEvent())
            .AddTo(this);
        closeButton.onClick.AsObservable()
            .Subscribe(_ => CloseButtonClickEvent())
            .AddTo(this);
    }

    private void InitializeSetting()
    {
        soundButton.interactable = true;
        //UI画面の項目をfalseに設定する
        closeButton.enabled = false;
        bgmSlider.enabled = false;
        seSlider.enabled = false;
        canvasGroupSound.blocksRaycasts = false;
    }

    private void CloseButtonClickEvent()
    {
        InitializeSetting();
        titleController.SetGameStateTitle();
        //0.5秒かけてUI画面を消す
        canvasGroupSound.DOFade(0.0f, 0.5f);
    }

    private void SoundButtonClickEvent()
    {
        soundButton.interactable = false;
        titleController.SetGameStateSetting();
        
        //UI画面の項目をfalseに設定する
        closeButton.enabled = true;
        bgmSlider.enabled = true;
        seSlider.enabled = true;

        canvasGroupSound.blocksRaycasts = true;
        //0.5秒かけてUI画面を表示
        canvasGroupSound.DOFade(1.0f, 0.5f);
    }
    
}
