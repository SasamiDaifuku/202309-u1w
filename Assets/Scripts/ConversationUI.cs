using DG.Tweening;
using TMPro;
using UnityEngine;

public class ConversationUI : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private TextMeshProUGUI speakText;

    private const int RANGE_MIN = 0;
    private const int RANGE_MAX = 5;

    private const int FADE_MESSAGE_INTERVAL = 1;
    private const int NEXT_MESSAGE_INTERVAL = 1;

    
    public enum Conversation
    {
        Story,
        Free,
        Talk,
    }
    private Conversation GetSetConversation { get; set; } = Conversation.Story;

    public void SpeakStoryConversation()
    {
        DOTween.Sequence()
            .AppendCallback(() => { speakText.text = "死んだぜ..."; })
            .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
            .AppendInterval(FADE_MESSAGE_INTERVAL)
            .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
            .AppendInterval(NEXT_MESSAGE_INTERVAL)
            .AppendCallback(() => { speakText.text = "...成仏するか"; })
            .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
            .AppendInterval(FADE_MESSAGE_INTERVAL)
            .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
            .AppendInterval(NEXT_MESSAGE_INTERVAL)
            .AppendCallback(() => { speakText.text = "この上に天国があるらしいぜ"; })
            .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
            .AppendInterval(FADE_MESSAGE_INTERVAL)
            .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
            .AppendInterval(NEXT_MESSAGE_INTERVAL)
            .AppendCallback(() => { speakText.text = "・・・"; })
            .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
            .AppendInterval(FADE_MESSAGE_INTERVAL)
            .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
            .AppendInterval(NEXT_MESSAGE_INTERVAL)
            .AppendCallback(() => { speakText.text = "まぁ時間の許す限り付き合ってくれや"; })
            .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
            .AppendInterval(FADE_MESSAGE_INTERVAL)
            .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
            .AppendInterval(NEXT_MESSAGE_INTERVAL)
            .AppendCallback(() => { GetSetConversation = Conversation.Free; })
            ;
    }

    /// <summary>
    /// 矢印キーを押したときにしゃべる
    /// </summary>
    public void SpeakFreeConversation()
    {
        //ストーリーやしゃべってるときは次の喋りをしない
        if (GetSetConversation != Conversation.Free) return;
        
        //RANGE_MAXの値は含まれない
        switch (Random.Range(RANGE_MIN,RANGE_MAX))
        {
            case 0:
                DOTween.Sequence()
                    .AppendCallback(() => { GetSetConversation = Conversation.Talk; })
                    .AppendCallback(() => { speakText.text = "おい!お前!!!"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "今歩こうとしたか！！？"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "勘弁してくれよな..."; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { GetSetConversation = Conversation.Free; })
                    ;
                break;
            case 1:
                DOTween.Sequence()
                    .AppendCallback(() => { GetSetConversation = Conversation.Talk; })
                    .AppendCallback(() => { speakText.text = "ハァ… 困ったなァ"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "まさか死んじゃうなんてェ…"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "左右キィ入力してるみたいだから急いで動きたいけど"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "もう疲れちゃって"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "全然動けなくてェ..."; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "..."; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "運びたくなったろ？"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { GetSetConversation = Conversation.Free; })
                    ;
                break;
            case 2:
                DOTween.Sequence()
                    .AppendCallback(() => { GetSetConversation = Conversation.Talk; })
                    .AppendCallback(() => { speakText.text = "ゲコッ..ゲコッ..."; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "...おっとすまん"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "ついネイチブなカエル語が出ちまった"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { GetSetConversation = Conversation.Free; })
                    ;
                break;
            case 3:
                DOTween.Sequence()
                    .AppendCallback(() => { GetSetConversation = Conversation.Talk; })
                    .AppendCallback(() => { speakText.text = "Rボタンでリトライ"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "やり直しができるらしいぜ 便利だよな"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "ま、俺の語りも最初からなわけだが"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "ありがたいだろ？"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { GetSetConversation = Conversation.Free; })
                    ;
                break;
            case 4:
                DOTween.Sequence()
                    .AppendCallback(() => { GetSetConversation = Conversation.Talk; })
                    .AppendCallback(() => { speakText.text = "俺ァ別に動けないわけじゃないんだぜ"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "俺がジャクシん頃はゲコゲコ言わせたもんだ"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { speakText.text = "お前にも見せてやりたかったぜ"; })
                    .Append(GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f))
                    .AppendInterval(FADE_MESSAGE_INTERVAL)
                    .Append(GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f))
                    .AppendInterval(NEXT_MESSAGE_INTERVAL)
                    .AppendCallback(() => { GetSetConversation = Conversation.Free; })
                    ;
                break;
        }
    }
}
