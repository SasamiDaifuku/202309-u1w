using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    //FadeCanvas取得
    [SerializeField]
    private Fade fade;

    //フェード時間(秒)
    [SerializeField]
    private float fadeTime;


    /// <summary>
    /// シーンのフェードインをする
    /// </summary>
    public void SceneFadeIn()
    {
        //シーン開始時にフェードをかける
        fade.FadeOut(fadeTime);
    }

    /// <summary>
    /// 指定シーン遷移時のフェード
    /// </summary>
    /// <param name="sceneNum"></param>
    public void NextSceneTransition(EnumSceneNum.SceneNum sceneNum)
    {
        int scenenum = (int)sceneNum;
        //フェードをかけてからシーン遷移する
        fade.FadeIn(fadeTime, () =>
        {
            SceneManager.LoadScene(scenenum);
        });
    }
    /// <summary>
    /// 現在のシーンへ遷移時のフェード
    /// </summary>
    public void CurrentSceneTransition()
    {
        //フェードをかけてからシーン遷移する
        fade.FadeIn(fadeTime, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}