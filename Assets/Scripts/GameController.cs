using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //FadeCanvas取得
    [SerializeField] private Fade fade;

    //フェード時間(秒)
    [SerializeField] private float fadeTime;

    private void Start()
    {
        //シーン開始時にフェードをかける
        fade.FadeOut(fadeTime);
        //音楽を流す
        AudioManager.Instance.PlayBGM(AUDIO.BGM_IWASHIRO_SAWAGURO_ERIKO);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //シーンを遷移する
            Debug.Log("スペースキーが押されました");
        }
    }
}