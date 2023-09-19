using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game画面でのメイン処理
/// </summary>
public class GameController : MonoBehaviour
{
    
    //FadeCanvas取得
    [SerializeField] private FadeManager fadeManager;
    
    void Start()
    {
        //フェードイン処理
        fadeManager.SceneFadeIn();
        //音楽を流す
        AudioManager.Instance.PlayBGM(AUDIO.BGM_IWASHIRO_SAWAGURO_ERIKO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
