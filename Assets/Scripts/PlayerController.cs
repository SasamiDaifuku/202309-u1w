using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private const float JUMP_POWER = 400;
    private bool _firstJump;
    private Rigidbody2D _rigidbody2DPlayer;
    private GameInputs _gameInputs;
    private AudioManager _audioManager;
    
    [SerializeField] private GameController gameController;
    [SerializeField] private GroundCheck groundCheck;

    [SerializeField] private ConversationUI conversationUI;
   
    private enum MovePlatform
    {
        Other,
        UpDown,
    }
    private MovePlatform GetSetMovePlatform { get; set; } = MovePlatform.Other;

    private Transform _movePlatformTransform;
    
    private void Awake()
    {
        _rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        // Actionスクリプトのインスタンス生成
        _gameInputs = new GameInputs();
        // Actionイベント登録
        _gameInputs.Player.Jump.performed += OnJump;
        _gameInputs.Player.Move.performed += OnMove;
        //AudioManagerを取得
        _audioManager = AudioManager.Instance;
        // Input Actionを機能させるためには、
        // 有効化する必要がある
        _gameInputs.Enable();
        _firstJump = false;
    }
    
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        _gameInputs?.Dispose();
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (groundCheck.CheckGroundStatus()  && gameController.GetSetGameState != EnumGameState.GameState.GameClear)
        {
            // ジャンプする力を与える
            _rigidbody2DPlayer.AddForce(Vector2.up * JUMP_POWER);
            //ジャンプのSEを流す
            _audioManager.PlaySe(AUDIO.SE_8BITJUMP3);
        }
        
        if (_firstJump == false)
        {
            _firstJump = true;
            //最初のジャンプをした時点でゲームスタートとする
            gameController.SetGameStateGamePlay();
            //ストーリーを話始める
            conversationUI.SpeakStoryConversation();
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //なき声を決める
        switch (Random.Range(0, 2))
        {
            case 0:
                AudioManager.Instance.PlaySe(AUDIO.SE_SE_FROG01);
                break;
            case 1:
                AudioManager.Instance.PlaySe(AUDIO.SE_SE_FROG02);
                break;
        }
        //移動ボタンが押された場合勝手にしゃべる
        //conversationUI.SpeakFreeConversation();
    }
}