using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //[SerializeField] float groundCheckRadius = 0f;
    //[SerializeField] float groundCheckOffsetY = 0f;
    [SerializeField] float groundCheckDistance = 0f;
    [SerializeField] LayerMask groundLayers = 0;

    /// <summary>
    /// 接地判定に利用するBoxCollider
    /// </summary>
    [SerializeField] private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public RaycastHit2D CheckGroundStatus()
    {
        return Physics2D.BoxCast(
            new Vector2(_boxCollider.transform.position.x, _boxCollider.transform.position.y) + _boxCollider.offset,
            _boxCollider.size,
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayers);
        //return Physics2D.CircleCast((Vector2)transform.position + groundCheckOffsetY * Vector2.up , groundCheckRadius, Vector2.down, groundCheckDistance, groundLayers);
    }

    /// <summary>
    /// Raycastの可視化を行う
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(_boxCollider.transform.position.x, _boxCollider.transform.position.y) + _boxCollider.offset - new Vector2(0f, groundCheckDistance),_boxCollider.size);
        
        //　CircleCastのレイを疑似的に視覚化
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffsetY * Vector2.up + Vector2.down * groundCheckDistance, groundCheckRadius);
    }
}