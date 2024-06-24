using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class PlayFabLogin : MonoBehaviour
{
    private void OnEnable()
    {
        PlayFabAuthService.OnLoginSuccess += PlayFabAuthService_OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError += PlayFabAuthService_OnPlayFabError;
    }

    private void OnDisable()
    {
        PlayFabAuthService.OnLoginSuccess -= PlayFabAuthService_OnLoginSuccess;
        PlayFabAuthService.OnPlayFabError -= PlayFabAuthService_OnPlayFabError;
    }

    private void PlayFabAuthService_OnLoginSuccess(LoginResult success)
    {
        Debug.Log("ログイン成功");
        SubmitScore(123);
    }
    private void PlayFabAuthService_OnPlayFabError(PlayFabError error)
    {
        Debug.Log("ログイン失敗");
    }
    void Start()
    {
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
    }
    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = playerScore
                }
            }
        }, result =>
        {
            Debug.Log($"スコア {playerScore} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}