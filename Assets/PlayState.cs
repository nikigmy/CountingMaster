using GameAnalyticsSDK;

public partial class GameManager
{
    protected class PlayState : GameStateBase
    {
        public override GameState type => GameState.PLAYING;

        public override void Init(GameManager context)
        {
            base.Init(context);
            
            context._analyticsManager.ReportLevelProgression(GAProgressionStatus.Start, context._levelController.currentLevelIndex);
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