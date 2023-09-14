using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        //音楽を流す
        AudioManager.Instance.PlayBGM(AUDIO.BGM_IWASHIRO_AME_NO_HAZAMA);
    }
}
