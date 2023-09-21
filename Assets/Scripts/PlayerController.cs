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
    [SerializeField] private GroundCheck groundCheck;
    private GameInputs _gameInputs;
    [SerializeField] private GameController gameController;

    private void Awake()
    {
        _rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        // Actionスクリプトのインスタンス生成
        _gameInputs = new GameInputs();
        // Actionイベント登録
        _gameInputs.Player.Jump.performed += OnJump;
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
        }

        if (_firstJump == false)
        {
            _firstJump = true;
            //最初のジャンプをした時点でゲームスタートとする
            gameController.SetGameStateGamePlay();
        }
    }
    private void Update()
    {
        
    }
}