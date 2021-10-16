
public partial class GameManager : Singleton<GameManager>
{
    protected class EmptyState : GameStateBase
    {
        public override GameState type => GameState.NONE;
    }
}