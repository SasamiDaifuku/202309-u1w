using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float JUMP_POWER = 400;
    private Rigidbody2D _rigidbody2DPlayer;
    [SerializeField] private GroundCheck _groundCheck;

    private void Start()
    {
        _rigidbody2DPlayer = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 設置している場合はジャンプ
        if (_groundCheck.CheckGroundStatus())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ジャンプする
                _rigidbody2DPlayer.AddForce(Vector2.up * JUMP_POWER);
            }
        }
    }

    /// <summary>
    /// 衝突を検知
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("何かが判定に入りました");
    }
}