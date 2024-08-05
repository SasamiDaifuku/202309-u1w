using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Playables;
using UniRx;
using DG.Tweening;

public class TitleRankingManager : MonoBehaviour
{
    [Header("ランキングUIのGameObject")]public GameObject gameObject;
    [Header("ベストタイム")]public TextMeshProUGUI bestTimeText;
    [Header("リーダーズボード")]public TMP_Text leaderboardText; 
    [Header("リーダーズボードのスクロールビュー")]public ScrollRect leaderboardScrollView;
    [Header("ランキングウィンドウクローズボタン")]public CustomButton closeButton;
    [SerializeField] private CustomButton rankingButton;
    [SerializeField] private CustomButton resetButton;
    [Header("ランキング読込中に表示する画像")]public GameObject loadingImage;
    [Header("ランキング読込中または読み込み失敗時に表示するテキスト")]public TextMeshProUGUI loadingText;
    private int windowDisplayCount;//ランキングウィンドウを表示した回数
    private bool _shouldCreateAccount;//アカウントを作成するか    
    private string _customID;//ログイン時に使うID
    private float delayTime = 2;//遅延時間
    private static readonly string LAST_NAME_KEY = "LAST_NAME_KEY";
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";
    private static readonly string RANKING_NAME = "TimeAttack"; //送信するランキング名

    
    private void Start()
    {
        rankingButton.SetActive(true);
        //クリックイベントを購読
        rankingButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => OnRankingButtonClicked())
            .AddTo(this);
    }
    private void OnEnable()
    {
        //PlayFabにログインする
        Login();
        //ロード時のテキストをランキング読込中…に変更する
        loadingText.text = "ランキング読込中…";
    }

    /// <summary>
    /// ランキングウィンドウ表示ボタンクリック
    /// </summary>
    public void OnRankingButtonClicked()
    {
        rankingButton.SetActive(false);
        gameObject.SetActive(true);
        //ランキングのUIを0.5秒かけて表示
        gameObject.GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
        //それぞれのボタンにメソッド登録
        closeButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => OnCloseButtonClicked())
            .AddTo(this);
        //クリックイベントを購読
        resetButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => ResetButtonClickEvent())
            .AddTo(this);
        
        ShowLoadingImage(true); // ランキング読み込み前にローディング画像を表示
        //Playfabにログインしていない場合
        if (!PlayFabClientAPI.IsClientLoggedIn()){
            //ログインしていない場合の処理
            Debug.Log("User is not logged in. Logging in...");
            //PlayFabにログインする
            Login();
            gameObject.SetActive(false);
            //ランキングのUIを0.5秒かけて表示
            GetComponent<CanvasGroup>().alpha = 0;
            return;
        }
        //ランキングウィンドウを表示した回数を初期化
        windowDisplayCount = 0;
        //スクロールビューを一番上に戻す
        leaderboardScrollView.verticalNormalizedPosition = 1;
        
        RefreshLeaderboard(); // リーダーボード情報の読み込みと表示
        
        //ハイスコア取得、実装はゲームに依存
        float dispbestTime = PlayerPrefs.GetFloat("BEST_TIME", 10000);
        if (dispbestTime == 10000)
        {
            dispbestTime = 0;
        }
        //ハイスコア用のテキストの内容をハイスコアに置き換える
        bestTimeText.text = dispbestTime.ToString("00.00");
        StartCoroutine(RefreshLeaderboardWithDelay(delayTime)); // 2秒の遅延を挟む
    }

    /// <summary>
    /// リーダーボード読み込み
    /// </summary>
    void RefreshLeaderboard(){
        
        windowDisplayCount++;
        
        // リーダーボード読み込み開始時にローディング画像を表示
        ShowLoadingImage(true); 
        var request = new GetLeaderboardRequest {
            //送信するランキング名(Playfabのランキングと名前を合わせる)
            StatisticName = RANKING_NAME,
            StartPosition = 0,
            //何位まで表示するか
            MaxResultsCount = 20
        };
        PlayFabClientAPI.GetLeaderboard(request, result => {
            leaderboardText.text = ""; // テキストの初期化
            foreach (var item in result.Leaderboard)
            {
                float time = item.StatValue/100f * -1;
                string timeString = time.ToString("00.00");
                //○○位；名前；○○点と表示された後に改行が入り、1つの順位となる。
                leaderboardText.text += $"{item.Position + 1}位: {item.DisplayName}: {timeString}\n";
            }
            // リーダーボード読み込み完了後にローディング画像を非表示
            if(windowDisplayCount>=2){
                ShowLoadingImage(false);
            };
        }, error => {
            Debug.LogError(error.GenerateErrorReport());
            loadingText.text = "ランキング読込失敗";//リーダーボードが読み込めない場合はテキストを変更する
        });
    }
    
    /// <summary>
    /// ローディング画像の表示・非表示を制御するメソッド
    /// </summary>
    /// <param name="show"></param>
    void ShowLoadingImage(bool show) {
        //ランキング読込中に表示する画像
        loadingImage.SetActive(show);
        loadingText.enabled = show;
    }
    
    /// <summary>
    /// 指定された遅延後にリーダーボードを再読み込みするコルーチン
    /// </summary>
    /// <param name="delay">遅延時間（秒）</param>
    /// <returns>IEnumerator</returns>
    IEnumerator RefreshLeaderboardWithDelay(float delay){
        yield return new WaitForSeconds(delay); // 指定された秒数待つ
        RefreshLeaderboard(); // リーダーボードの再読み込み
    }
    
    /// <summary>
    /// クローズボタン(背景)クリック
    /// </summary>
    public void OnCloseButtonClicked()
    {
        //ランキングウィンドウを表示した回数を初期化
        windowDisplayCount = 0;
        //0.5秒かけてUI画面を消す
        GetComponent<CanvasGroup>().DOFade(0.0f, 0.5f);
        gameObject.SetActive(false);
        rankingButton.SetActive(true);
    }
    
    /// <summary>
    /// Playfabログイン処理
    /// </summary>
    private void Login(){
        _customID = LoadCustomID();
        Debug.Log($"CustomID = {_customID}");
        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };//補足　既にアカウントが作成されており、CreateAccountがtrueになっていてもエラーにはならない
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    
    /// <summary>
    /// Playfabログイン成功
    /// </summary>
    /// <param name="result"></param>
    private void OnLoginSuccess(LoginResult result){
        //アカウントを作成しようとしたのに、IDが既に使われていて、出来なかった場合
        if (_shouldCreateAccount && result.NewlyCreated == false){
            Debug.LogWarning("CustomId :" +_customID+ "は既に使われています。");
            Login();//ログインしなおし
            return;
        }

        //アカウント新規作成できたらIDを保存
        if (result.NewlyCreated){
            SaveCustomID();
            Debug.Log("新規作成成功");
        }
        Debug.Log("ログイン成功!!");
    }

    /// <summary>
    /// Playfabログイン失敗
    /// </summary>
    /// <param name="error"></param>
    //ログイン失敗
    private void OnLoginFailure(PlayFabError error){
        Debug.LogError("PlayFabのログインに失敗\n" + error.GenerateErrorReport());
    }

    //カスタムIDを取得
    private string LoadCustomID(){
        // キーが存在するか確認し、存在しない場合はデフォルト値を返す
        //string id = ES3.Load<string>(CUSTOM_ID_SAVE_KEY, defaultValue:"");
        string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY, "");

        //idの中身がnullもしくは空の文字列("")の場合は_shouldCreateAccountはtrueになる。
        _shouldCreateAccount = string.IsNullOrEmpty(id);

        //idの中身がない場合、文字列を新規作成
        if (_shouldCreateAccount)
        {
            Debug.Log("idが空");
            return GenerateCustomID();//文字列を新規作成
        }
        else
        {
            return id;//セーブされた文字列を返す
        }
    }

    //IDの保存
    private void SaveCustomID(){
        //EasySave3でIDをセーブデータに保存
        //ES3.Save<string>(CUSTOM_ID_SAVE_KEY, _customID);
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, _customID);
    }
    
    //カスタムIDを生成する
    //ユニークな文字列をGuidを使用し生成
    //https://docs.microsoft.com/ja-jp/dotnet/api/system.guid.tostring?redirectedfrom=MSDN&view=netframework-4.8#System_Guid_ToString_System_String_
    private string GenerateCustomID(){
        //Guidの構造体生成
        Guid guid = Guid.NewGuid();               
        return guid.ToString("N");//書式指定子はNを指定
    }
    
    /// <summary>
    /// ベストスコアをリセットする
    /// </summary>
    private void ResetButtonClickEvent()
    {
        PlayerPrefs.DeleteKey("BEST_TIME");
        
        float dispbestTime = PlayerPrefs.GetFloat("BEST_TIME", 10000);
        if (dispbestTime == 10000)
        {
            dispbestTime = 0;
        }
        //ハイスコア用のテキストの内容をハイスコアに置き換える
        bestTimeText.text = dispbestTime.ToString("00.00");
    }
}