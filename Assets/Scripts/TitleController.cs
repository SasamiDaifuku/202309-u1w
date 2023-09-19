using UnityEngine;

/// <summary>
/// タイトル画面のメイン処理
/// </summary>
public class TitleController : MonoBehaviour
{
    //FadeCanvas取得
    [SerializeField] private FadeManager fadeManager;

    private void Start()
    {
        //フェードイン処理
        fadeManager.SceneFadeIn();
        //音楽を流す
        AudioManager.Instance.PlayBGM(AUDIO.BGM_IWASHIRO_SAWAGURO_ERIKO);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //シーンを遷移する
            fadeManager.NextSceneTransition(EnumSceneNum.SceneNum.GameScene);
        }
    }
}