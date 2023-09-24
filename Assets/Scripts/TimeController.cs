using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private const float START_TIME = 0f;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameController gameController;

    /// <summary>
    /// タイマーの時間
    /// </summary>
    public float GetSetTime { get; private set; }
    
    void Start()
    {
        //現在の残り時間にタイマーの設定値を入れる
        GetSetTime = START_TIME;
        //バトル時間の表示を更新
        UpdateDisplayTime(GetSetTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetSetGameState == EnumGameState.GameState.GamePlay)
        {
            //バトル時間の表示を更新
            GetSetTime += Time.deltaTime;
            UpdateDisplayTime(GetSetTime);
        }
        
    }

    /// <summary>
    /// 時間の表示を更新
    /// </summary>
    /// <param name="currentTime"></param>
    private void UpdateDisplayTime(float currentTime)
    {
        //バトルの残り時間を更新
        timerText.text = currentTime.ToString("00.00");
    }
    
    public string GetTextNowTime()
    {
        //バトルの残り時間を更新
        return GetSetTime.ToString("00.00");
    }
}
