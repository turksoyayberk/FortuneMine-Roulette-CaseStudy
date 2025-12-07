namespace Core.Services
{
    public static class GameStateManager
    {
        public static GameState CurrentState { get; private set; } = GameState.Idle;

        public static void SetState(GameState newState)
        {
            if (CurrentState == newState)
                return;

            CurrentState = newState;
        }
    }

    public enum GameState
    {
        Idle,
        Spinning,
        Popup,
        Rewarding,
    }
}