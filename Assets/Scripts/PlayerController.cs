using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private const float JUMP_POWER = 400;
    private Rigidbody2D _rigidbody2DPlayer;
    [SerializeField] private GroundCheck groundCheck;
    private GameInputs _gameInputs;

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
    }
    
    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        _gameInputs?.Dispose();
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (groundCheck.CheckGroundStatus())
        {
            // ジャンプする力を与える
            _rigidbody2DPlayer.AddForce(Vector2.up * JUMP_POWER);
        }
    }
    private void Update()
    {
        
    }
}