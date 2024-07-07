using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// タイトル画面のメイン処理
/// </summary>
public class TitleController : MonoBehaviour
{
    //FadeCanvas取得
    [SerializeField] private FadeManager fadeManager;
    //ゲーム状態
    public EnumGameState.GameState GetSetGameState { get; private set; } = EnumGameState.GameState.Title;

    private void Start()
    {
        //フェードイン処理
        fadeManager.SceneFadeIn();
        //音楽を流す
        AudioManager.Instance.PlayBGM(AUDIO.BGM_IWASHIRO_SAWAGURO_ERIKO);
        //AdmobSDKの初期化
        //Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }
    
    private void Update()
    {
        //ボタンがクリックされたときは画面クリックを無視する
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //iPhoneでのタッチの確認はこっちを使う
        if (Input.GetMouseButtonDown (0)) {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                // nGUI上をクリックしているので処理をキャンセルする。
                return;
            }
        }

        //ゲーム状態がタイトル以外の時(Setting中等)はクリックを無視する
        if (GetSetGameState != EnumGameState.GameState.Title) return;
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //シーンを遷移する
            fadeManager.NextSceneTransition(EnumSceneNum.SceneNum.GameScene);
        }
    }

    /// <summary>
    /// ゲーム状態をSettingに変更
    /// </summary>
    public void SetGameStateSetting()
    {
        GetSetGameState = EnumGameState.GameState.Setting;
    }

    /// <summary>
    /// ゲーム状態をTitleに変更
    /// </summary>
    public void SetGameStateTitle()
    {
        GetSetGameState = EnumGameState.GameState.Title;
    }
}