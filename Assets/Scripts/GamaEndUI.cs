using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class GamaEndUI : MonoBehaviour
{
    [SerializeField] private CustomButton retryButton;
    [SerializeField] private CustomButton endButton;
    [SerializeField] private CustomButton tweetButton;

    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private GameController gameController;
    [SerializeField] private TimeController timeController;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string LinkUrl = "";

    private void Start()
    {
        // ボタンの初期値をfalseに設定
        retryButton.SetActive(false);
        endButton.SetActive(false);
        tweetButton.SetActive(false);
        //クリックイベントを購読
        retryButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => fadeManager.CurrentSceneTransition())
            .AddTo(this);
        endButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => fadeManager.NextSceneTransition(EnumSceneNum.SceneNum.Title))
            .AddTo(this);
        tweetButton.OnButtonClicked.AsObservable()
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
        //UnityRoomTweet.Tweet("kaeru_kaeru", $"{timeController.GetTextNowTime()}でカエルは天に還りました...", "天に還るカエル", "unity1week");
        string tweettext = $"{timeController.GetTextNowTime()}でカエルは天に還りました...";
        string hashtags = "天に還るカエル";
        var url = "https://twitter.com/intent/tweet?"
                  + "text=" + tweettext
                  + "&url=" + LinkUrl
                  + "&hashtags=" + hashtags;

#if UNITY_EDITOR
        Application.OpenURL ( url );
#elif UNITY_WEBGL
            // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
            Application.OpenURL(url);
#endif
    }

    /// <summary>
    /// ゲーム終了時のUIを表示する
    /// </summary>
    private void DisplayGameEndUI()
    {
        // ボタンをtrueに設定
        retryButton.SetActive(true);
        endButton.SetActive(true);
        tweetButton.SetActive(true);

        //スコアを取得
        scoreText.text = timeController.GetTextNowTime();
        
        //ゲーム終了時のUIを0.5秒かけて表示
        GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
    }
}
