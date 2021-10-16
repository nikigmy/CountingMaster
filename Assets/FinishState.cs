using GameAnalyticsSDK;

public partial class GameManager
{
    protected class FinishState : GameStateBase
    {
        public override GameState type => GameState.FINISH;

        public override void Init(GameManager context)
        {
            base.Init(context);
            context._gameSave.level++;
            context._gameSave.Save();
            context._analyticsManager.ReportLevelProgression(GAProgressionStatus.Complete, context._levelController.currentLevelIndex, PlayerController.Instance.Score);
            StatesUtil.Pause();
        }

        public override void Play(GameManager context)
        {
            base.Play(context);
            context.SetState(GameState.MENU);
            
            StatesUtil.Unpause();
        }
    }
}