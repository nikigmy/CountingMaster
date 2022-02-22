
public partial class GameManager
{
    protected class PlayState : GameStateBase
    {
        public override GameState type => GameState.PLAYING;

        public override void Init(GameManager context)
        {
            base.Init(context);
        }

        public override void TogglePause(GameManager context)
        {
            base.TogglePause(context);
            StatesUtil.Pause();

            context.SetState(GameState.PAUSED);
        }

        public override void Die(GameManager context)
        {
            base.Die(context);
            context.SetState(GameState.DEAD);
        }

        public override void Finish(GameManager context)
        {
            base.Finish(context);
            context.SetState(GameState.FINISH);
        }
    }
}