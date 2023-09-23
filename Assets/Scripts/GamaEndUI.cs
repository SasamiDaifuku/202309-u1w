using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using naichilab;
using TMPro;

public class GamaEndUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button tweetButton;

    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private GameController gameController;
    [SerializeField] private TimeController timeController;

    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        // ボタンの初期値をfalseに設定
        retryButton.enabled = false;
        endButton.enabled = false;
        tweetButton.enabled = false;
        //クリックイベントを購読
        retryButton.onClick.AsObservable()
            .Subscribe(_ => fadeManager.CurrentSceneTransition())
            .AddTo(this);
        endButton.onClick.AsObservable()
            .Subscribe(_ => fadeManager.NextSceneTransition(EnumSceneNum.SceneNum.Title))
            .AddTo(this);
        tweetButton.onClick.AsObservable()
            .Subscribe(_ => TweetScore())
            .AddTo(this);
        //GameController上のGameStateの状態の変化を購読
        gameController
            .ObserveEveryValueChanged(x => x.GetSetGameState)
            .Where(x => x == EnumGameState.GameState.GameClear)
            .Subscribe(x => DisplayGameEndUI());
    }

    /// <summary>
    /// スコアをツイートする
    /// </summary>
    private void TweetScore()
    {
        UnityRoomTweet.Tweet("kaeru_kaeru", $"{timeController.GetTextNowTime()}でカエルは天に還りました...", "天に還るカエル", "unity1week");
    }

    /// <summary>
    /// ゲーム終了時のUIを表示する
    /// </summary>
    private void DisplayGameEndUI()
    {
        // ボタンをtrueに設定
        retryButton.enabled = true;
        endButton.enabled = true;
        tweetButton.enabled = true;

        //スコアを取得
        scoreText.text = timeController.GetTextNowTime();
        
        //ゲーム終了時のUIを0.5秒かけて表示
        GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
    }
}
