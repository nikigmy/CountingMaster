public partial class GameManager
{
    protected class MenuState : GameStateBase
    {
        public override GameState type => GameState.MENU;

        public override void Init(GameManager context)
        {
            base.Init(context);
            PlayerController.Instance.Reset();
        }

        public override void Play(GameManager context)
        {
            base.Play(context);
            context.SetState(GameState.PLAYING);
        }
    }
}