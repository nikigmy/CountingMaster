using GameAnalyticsSDK;

public partial class GameManager
{
    protected class DeadState : GameStateBase
    {
        public override GameState type => GameState.DEAD;

        public override void Init(GameManager context)
        {
            base.Init(context);
            
            context._analyticsManager.ReportLevelProgression(GAProgressionStatus.Fail, context._levelController.currentLevelIndex);
        }

        public override void Play(GameManager context)
        {
            base.Play(context);
            
            context.SetState(GameState.MENU);
        }
    }
}