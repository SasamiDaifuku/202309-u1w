using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Playables;

public class RankingManager : MonoBehaviour{
    [Header("ランキングウィンドウ")]public Transform rankingWindow;
    [Header("バックパネル")]public GameObject backPanel;
    [Header("送信するランキング名")]public string rankingName;
    [Header("今回のスコア")]public TextMeshProUGUI currentScoreText;
    [Header("ハイスコア")]public TextMeshProUGUI highScoreText;
    [Header("名前入力フィールド")]public TMP_InputField nameInputField;
    [Header("名前入力フィールド未入力時に表示される文字")]public TextMeshProUGUI placeholder;
    private string placeholderText;//入力が一度もされていないときにインプットフィールドに表示される文字 
    [Header("リーダーズボード")]public TMP_Text leaderboardText; 
    [Header("リーダーズボードのスクロールビュー")]public ScrollRect leaderboardScrollView;
    private int currentScore; //今回のスコア(データ)
    private int highScore; //ハイスコア(データ)
    [Header("データ送信時SE")]public AudioClip sendSe;
    [Header("クリック時SE")]public AudioClip clickSe;
    [Header("クローズ時SE")]public AudioClip closeSe;
    [Header("データ送信ボタン")]public Button sendScoreButton;
    [Header("ランキングウィンドウクローズボタン")]public Button closeButton;
    [Header("ランキング読込中に表示する画像")]public GameObject loadingImage;
    [Header("ランキング読込中または読み込み失敗時に表示するテキスト")]public TextMeshProUGUI loadingText;
    [Header("ウィンドウを表示するときのタイムライン")]public PlayableDirector startdirector;
    [Header("ウィンドウを非表示するときのタイムライン")]public PlayableDirector closedirector;
    private int windowDisplayCount;//ランキングウィンドウを表示した回数
    private bool _shouldCreateAccount;//アカウントを作成するか    
    private string _customID;//ログイン時に使うID
    private float delayTime = 2;//遅延時間
    [SerializeField] private TimeController timeController;
    private static readonly string LAST_NAME_KEY = "LAST_NAME_KEY";
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    private void OnEnable(){
        //それぞれのボタンにメソッド登録
        sendScoreButton.onClick.AddListener(OnSendScoreButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
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
        //バックパネルを非表示にする
        backPanel.SetActive(false);
    }

    /// <summary>
    /// ランキングウィンドウ表示ボタンクリック
    /// </summary>
    public void OnRankingButtonClicked(){
        ShowLoadingImage(true); // ランキング読み込み前にローディング画像を表示
        //Playfabにログインしていない場合
        if (!PlayFabClientAPI.IsClientLoggedIn()){
            //ログインしていない場合の処理
            Debug.Log("User is not logged in. Logging in...");
            //PlayFabにログインする
            Login();
            return;
        }
        
        // スコア送信ボタンを非活性にする
        sendScoreButton.interactable = false;
        //ランキングウィンドウを表示した回数を初期化
        windowDisplayCount = 0;
        //スクロールビューを一番上に戻す
        leaderboardScrollView.verticalNormalizedPosition = 1;
        
        RefreshLeaderboard(); // リーダーボード情報の読み込みと表示
        
        //今回のスコアを取得するメソッド、実装はゲームに依存
        //currentScore.text = timeController.GetTextNowTime(); 
        //今回のスコア用のテキストの内容を今回のスコアに置き換える
        //currentScoreText.text = currentScore.ToString();
        //ハイスコア取得、実装はゲームに依存
        //highScore = GameManager.i.GetHighScore();
        //ハイスコア用のテキストの内容をハイスコアに置き換える
        //highScoreText.text = highScore.ToString();
        
        StartCoroutine(RefreshLeaderboardWithDelay(delayTime)); // 2秒の遅延を挟む
        
        //ウィンドウを表示させるタイムラインを再生
        startdirector.Play();
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
            StatisticName = rankingName,
            StartPosition = 0,
            //何位まで表示するか
            MaxResultsCount = 20
        };
        PlayFabClientAPI.GetLeaderboard(request, result => {
            leaderboardText.text = ""; // テキストの初期化
            foreach (var item in result.Leaderboard){
                //○○位；名前；○○点と表示された後に改行が入り、1つの順位となる。
                leaderboardText.text += $"{item.Position + 1}位: {item.DisplayName}: {item.StatValue}\n";
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
        sendScoreButton.interactable = false; 
        ShowLoadingImage(true); // データ送信中にローディング画像を表示
        //ロード時のテキストをランキング読込中…に変更する
        loadingText.text = "ランキング読込中…";
        // スコア送信時のサウンド(適宜変更する)
        //SoundManager.i.PlaySe(sendSe); 
        // 名前が空の場合は「名無し」を使用
        string playerName = string.IsNullOrEmpty(nameInputField.text) ? "名無し" : nameInputField.text;
        //ES3.Save<string>("LastName", playerName); // 名前の保存、空の場合「名無し」を保存
        UpdatePlayerDisplayName(playerName); // プレイヤー名の更新処理を追加
        var request = new UpdatePlayerStatisticsRequest {
            //スコアの更新
            Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = rankingName, Value = currentScore } }
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
        if(currentScore >= highScore){
            //今回のスコアがハイスコアを越えていたら、
            highScore = currentScore;
            //今回のスコアがハイスコアになる
            highScoreText.text = highScore.ToString();
            //データ送信ボタンを押せるようにする
            sendScoreButton.interactable = true;
        }
    }
    
    /// <summary>
    /// クローズボタン(背景)クリック
    /// </summary>
    public void OnCloseButtonClicked(){
        // クローズボタンの効果音(適宜変更)
        //SoundManager.i.PlaySe(closeSe);
        //ランキングウィンドウを表示した回数を初期化
        windowDisplayCount = 0;
        
        //ウィンドウを非表示させるタイムラインを再生
        closedirector.Play();
    }
    
    /// <summary>
    /// Playfabログイン処理
    /// </summary>
    private void Login(){
        _customID = LoadCustomID();
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
        string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY, "Foobar");;

        //idの中身がnullもしくは空の文字列("")の場合は_shouldCreateAccountはtrueになる。
        _shouldCreateAccount = string.IsNullOrEmpty(id);

        //idの中身がない場合、文字列を新規作成
        if (_shouldCreateAccount)
        {
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