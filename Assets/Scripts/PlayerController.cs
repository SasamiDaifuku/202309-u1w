using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float JUMP_POWER = 400;
    private Rigidbody2D _rigidbody2DPlayer;

    private void Start()
    {
        _rigidbody2DPlayer = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ジャンプする
            _rigidbody2DPlayer.AddForce(Vector2.up * JUMP_POWER);
        }
    }
}
