using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    float groundCheckRadius = 0f;
    [SerializeField]
    float groundCheckOffsetY = 0f;
    [SerializeField]
    float groundCheckDistance = 0f;
    [SerializeField]
    LayerMask groundLayers = 0;

    public RaycastHit2D CheckGroundStatus()
    {
        return Physics2D.CircleCast((Vector2)transform.position + groundCheckOffsetY * Vector2.up , groundCheckRadius, Vector2.down, groundCheckDistance, groundLayers);
    }
 
    void OnDrawGizmos() {
        //　Cubeのレイを疑似的に視覚化
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffsetY * Vector2.up + Vector2.down * groundCheckDistance, groundCheckRadius);
    }
    
}
