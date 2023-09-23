using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroupSound;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private SliderEventExpansion seSliderExpansion;

    [SerializeField] private TitleController titleController;

    private AudioManager _audioManager;
    
    private void Awake()
    {
        InitializeSetting();

        //シーン遷移後もここでAudioManagerを取得
        _audioManager = AudioManager.Instance;
        //クリックイベントを購読
        soundButton.onClick.AsObservable()
            .Subscribe(_ => SoundButtonClickEvent())
            .AddTo(this);
        closeButton.onClick.AsObservable()
            .Subscribe(_ => CloseButtonClickEvent())
            .AddTo(this);
        //スライダーのイベントを購読
        bgmSlider.onValueChanged.AsObservable()
            .Subscribe(_ => SetBGMVolume(bgmSlider.value))
            .AddTo(this);
        seSliderExpansion.OnChangeValue.AsObservable()
            .Subscribe(value => SetSeVolume(seSlider.value))
            .AddTo(this);

    }

    /// <summary>
    /// 音量をセット
    /// </summary>
    /// <param name="bgmVolume"></param>
    private void SetBGMVolume(float bgmVolume)
    {
        //変化した値を格納する
        _audioManager.ChangeBGMVolume(bgmVolume);
    }
    /// <summary>
    /// 音量をセット
    /// </summary>
    /// <param name="seVolume"></param>
    private void SetSeVolume(float seVolume)
    {
        _audioManager.ChangeSeVolume(seVolume);;
        //SEを鳴らす
        _audioManager.PlaySe(AUDIO.SE_8BITJUMP3);
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
        
        //UI画面の項目をtrueに設定する
        closeButton.enabled = true;
        bgmSlider.enabled = true;
        seSlider.enabled = true;

        bgmSlider.value = _audioManager.GetBGMVolume();
        seSlider.value = _audioManager.GetSeVolume();

        canvasGroupSound.blocksRaycasts = true;
        //0.5秒かけてUI画面を表示
        canvasGroupSound.DOFade(1.0f, 0.5f);
    }
    
}
