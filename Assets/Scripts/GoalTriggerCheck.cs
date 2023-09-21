using UnityEngine;

public class GoalTriggerCheck : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    /// <summary>
    /// ゴール判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            gameController.SetGameStateGameClear();
        }
    }
}
