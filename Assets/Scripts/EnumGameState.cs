public class EnumGameState
{
    /// <summary>
    /// ゲーム状態の遷移
    /// </summary>
    public enum GameState {
        Title,
        BeforeStart,
        GameEvent,
        GamePlay,
        GameOver,
        GameClear
    }
}