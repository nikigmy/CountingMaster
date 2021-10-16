using UnityEngine;

public abstract class GameStateBase
{
    public abstract GameState type { get; }
    
    public virtual void Init(GameManager context) 
    {
    }
    
    public virtual void Play(GameManager context) 
    {
    }
    
    public virtual void Finish(GameManager context) 
    {
    }
    
    public virtual void Die(GameManager context) 
    {
    }

    public virtual void Quit(GameManager context)
    {
        Time.timeScale = 1f;
        context.onQuit.Invoke();
    }
    
    public virtual void TogglePause(GameManager context) 
    {
    }
}