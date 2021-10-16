using System;
using AnimationInstancing;
using UnityEngine;
using UnityEngine.Events;

public partial class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _arenaWidth;
    [SerializeField] protected LevelController _levelController;
    public UnityEvent onQuit;
    public StateChangedEvent onStateChanged;

    public GameState currentGameState => currentState.type;
    public AnalyticsManager analytics => _analyticsManager;

    public GameSave GameSave => _gameSave;

    public float arenaWidth => _arenaWidth;

    private AnalyticsManager _analyticsManager;

    private GameSave _gameSave;
    
    protected GameStateBase currentState = null;

    void Awake()
    {
        _analyticsManager = new AnalyticsManager();
        StartCoroutine(AnimationManager.GetInstance().LoadAnimationAssetBundle(Application.streamingAssetsPath + "/animationtexture"));
        DontDestroyOnLoad(gameObject);
        _gameSave = new GameSave();
        SetState(GameState.MENU);
    }

    public void SetState(GameState state)
    {
        var oldState = currentState?.type ?? GameState.NONE;
        currentState = CreateGameState(state);
        currentState.Init(this);

        switch (oldState)
        {
            case GameState.PAUSED:
                StatesUtil.Unpause();
                break;
        }
        
        onStateChanged?.Invoke(oldState, currentState.type);
    }
    
    public void Die()
    {
        currentState.Die(this);
    }
    
    public void Play() 
    {
        currentState.Play(this);
    }
    

    public void Finish() 
    {
        currentState.Finish(this);
    }
    
    public void Quit()
    {
        currentState.Quit(this);
    }
    
    public void TogglePause() 
    {
        currentState.TogglePause(this);
    }
    
    protected virtual GameStateBase CreateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MENU:
                return new MenuState();
                break;
            case GameState.PLAYING:
                return new PlayState();
                break;
            case GameState.PAUSED:
                return new PauseState();
                break;
            case GameState.DEAD:
                return new DeadState();
                break;
            case GameState.FINISH:
                return new FinishState();
                break;
            case GameState.NONE:
                return new EmptyState();
                break;
            default:
                throw new ArgumentException($"Attempting to create an incorrect state: {state}");
        }
    }
    
    [System.Serializable]
    public class StateChangedEvent : UnityEvent<GameState, GameState> { }

}