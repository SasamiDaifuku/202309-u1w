using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;

/// <summary>
/// Game画面でのメイン処理
/// </summary>
public class GameController : MonoBehaviour
{
    
    //FadeCanvas取得
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private TimeController timeController;
    [SerializeField] private CustomButton retryButton;
    
    public EnumGameState.GameState GetSetGameState { get; private set; } = EnumGameState.GameState.BeforeStart;
    
    void Start()
    {
        //フェードイン処理
        fadeManager.SceneFadeIn();
        //音楽を流す
        AudioManager.Instance.PlayBGM(AUDIO.BGM_IWASHIRO_SAWAGURO_ERIKO);
        retryButton.OnButtonClicked
            .Subscribe(_ => fadeManager.CurrentSceneTransition())
            .AddTo(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ゲーム状態をプレイに変更
    /// </summary>
    public void SetGameStateGamePlay()
    {
        GetSetGameState = EnumGameState.GameState.GamePlay;
    }

    /// <summary>
    /// ゲーム状態をクリアに変更
    /// </summary>
    public void SetGameStateGameClear()
    {
        GetSetGameState = EnumGameState.GameState.GameClear;
        //Unityroomにスコアを送信する
        //UnityroomApiClient.Instance.SendScore(1, timeController.GetSetTime, ScoreboardWriteMode.HighScoreAsc);
    }
}
