public partial class GameManager
{
    protected class PauseState : GameStateBase
    {
        public override GameState type => GameState.PAUSED;

        public override void TogglePause(GameManager context)
        {
            base.TogglePause(context);
            StatesUtil.Unpause();

            context.SetState(GameState.PLAYING);
        }
    }
}