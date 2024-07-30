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

public class RankingManager : MonoBehaviour
{
    [Header("ランキングUIのGameObject")]public GameObject gameObject;
    [Header("今回のタイム")]public TextMeshProUGUI currentTimeText;
    [Header("ベストタイム")]public TextMeshProUGUI bestTimeText;
    [Header("名前入力フィールド")]public TMP_InputField nameInputField;
    [Header("名前入力フィールド未入力時に表示される文字")]public TextMeshProUGUI placeholder;
    private string placeholderText;//入力が一度もされていないときにインプットフィールドに表示される文字 
    [Header("リーダーズボード")]public TMP_Text leaderboardText; 
    [Header("リーダーズボードのスクロールビュー")]public ScrollRect leaderboardScrollView;
    private float bestTime; //ハイスコア(データ)
    [Header("データ送信ボタン")]public CustomButton sendScoreButton;
    [Header("ランキングウィンドウクローズボタン")]public CustomButton closeButton;
    [Header("ランキング読込中に表示する画像")]public GameObject loadingImage;
    [Header("ランキング読込中または読み込み失敗時に表示するテキスト")]public TextMeshProUGUI loadingText;
    private int windowDisplayCount;//ランキングウィンドウを表示した回数
    private bool _shouldCreateAccount;//アカウントを作成するか    
    private string _customID;//ログイン時に使うID
    private float delayTime = 2;//遅延時間
    [SerializeField] private TimeController timeController;
    private static readonly string LAST_NAME_KEY = "LAST_NAME_KEY";
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";
    private static readonly string RANKING_NAME = "TimeAttack"; //送信するランキング名

    private void OnEnable()
    {
        //PlayFabにログインする
        Login();
        //デフォルトメッセージはiOS、Androidの場合とパソコンの場合で変える
if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android){
            placeholderText = "タップして入力";
        } else {
            placeholderText = "クリックして入力";
        }
        //デフォルトメッセージをインプットフィールドに表示
        placeholder.text = placeholderText;
        //インプットフィールドに名前を表示
        string lastName = PlayerPrefs.GetString(LAST_NAME_KEY, "");
        nameInputField.text = lastName;
        //名前が空でない場合、インプットフィールドのテキストを空にする
        if(lastName != ""){
            placeholder.text = "";
        }
        //ロード時のテキストをランキング読込中…に変更する
        loadingText.text = "ランキング読込中…";
    }

    /// <summary>
    /// ランキングウィンドウ表示ボタンクリック
    /// </summary>
    public void OnRankingButtonClicked()
    {
        gameObject.SetActive(true);
        //ランキングのUIを0.5秒かけて表示
        gameObject.GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
        //それぞれのボタンにメソッド登録
        sendScoreButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => OnSendScoreButtonClicked())
            .AddTo(this);
        closeButton.OnButtonClicked.AsObservable()
            .Subscribe(_ => OnCloseButtonClicked())
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
        // スコア送信ボタンを非活性にする
        sendScoreButton.SetActive(true);
        //ランキングウィンドウを表示した回数を初期化
        windowDisplayCount = 0;
        //スクロールビューを一番上に戻す
        leaderboardScrollView.verticalNormalizedPosition = 1;
        
        RefreshLeaderboard(); // リーダーボード情報の読み込みと表示
        
        //今回のスコアを取得するメソッド
        currentTimeText.text = timeController.GetTextNowTime();
        //ハイスコア取得、実装はゲームに依存
        bestTime = PlayerPrefs.GetFloat("BEST_TIME", 10000);
        //ハイスコア用のテキストの内容をハイスコアに置き換える
        bestTimeText.text = bestTime.ToString("00.00");
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
                float time = item.StatValue/100f;
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
    /// スコア送信ボタンクリック
    /// </summary>
    public void OnSendScoreButtonClicked(){
        // 送信後はボタンを非活性化(何回も押せないように)
        sendScoreButton.SetActive(false); 
        ShowLoadingImage(true); // データ送信中にローディング画像を表示
        //ロード時のテキストをランキング読込中…に変更する
        loadingText.text = "ランキング読込中…";
        // 名前が空の場合は「名無し」を使用
        string playerName = string.IsNullOrEmpty(nameInputField.text) ? "名無し" : nameInputField.text;
        UpdatePlayerDisplayName(playerName); // プレイヤー名の更新処理を追加
        //PlayFabはint型しか登録できないので×100してint型に変換3
        float bestTimeMul = bestTime * 100;
        int bestTimeInt = (int)bestTimeMul;
        Debug.Log($"ベストタイム={bestTime}");
        Debug.Log($"ベストタイム×100={bestTimeMul}");
        Debug.Log($"ベストタイム整数={bestTimeInt}");
        var request = new UpdatePlayerStatisticsRequest {
            //スコアの更新
            Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = RANKING_NAME, Value = bestTimeInt } }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, result => {
            //スコア更新処理が完了したら、少し遅延を挟んでからリーダーボードを再読み込みする
            //遅延を挟まないと、データ送信前の状態のリーダーボードがそのまま表示される
            StartCoroutine(RefreshLeaderboardWithDelay(delayTime)); // 2秒の遅延を挟む
        }, error => {
            //スコア更新処理が失敗した場合の処理
            Debug.LogError(error.GenerateErrorReport());
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
    /// プレイヤーネーム更新
    /// </summary>
    /// <param name="playerName"></param>
    private void UpdatePlayerDisplayName(string playerName) {
        // 名前が2文字以下の場合、前に半角スペースを追加して3文字にする
        while (playerName.Length < 3) {
            playerName = " " + playerName;
        }
        // プレイヤー名の更新
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = playerName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result => {
            // プレイヤー名の更新が成功した場合の処理
            Debug.Log("Display name updated successfully.");
        }, error => {
            // プレイヤー名の更新が失敗した場合の処理
            Debug.LogError(error.GenerateErrorReport());
        });
    }
    
    /// <summary>
    /// 指定された遅延後にリーダーボードを再読み込みするコルーチン
    /// </summary>
    /// <param name="delay">遅延時間（秒）</param>
    /// <returns>IEnumerator</returns>
    IEnumerator RefreshLeaderboardWithDelay(float delay){
        yield return new WaitForSeconds(delay); // 指定された秒数待つ
        RefreshLeaderboard(); // リーダーボードの再読み込み
        // ハイスコアの更新チェック
        /*
        if(currentTime >= bestTime){
            //今回のスコアがハイスコアを越えていたら、
            bestTime = currentTime;
            //今回のスコアがハイスコアになる
            bestTimeText.text = bestTime.ToString();
            //データ送信ボタンを押せるようにする
            sendScoreButton.SetActive(true);
        }
        */
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
}