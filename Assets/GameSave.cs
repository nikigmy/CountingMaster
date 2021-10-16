using UnityEngine;

public class GameSave
{
    private int _startCount;

    public int startCount
    {
        get => _startCount;
        set
        {
            _startCount = value;
        }
    }

    private int _gold;

    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
        }
    }
    

    
    private int _level;

    public int level
    {
        get => _level;
        set
        {
            _level = value;
        }
    }
    
    public GameSave()
    {
        Load();
    }
    
    public void Load()
    {
        _level = PlayerPrefs.GetInt("Level", 1);
        _startCount = PlayerPrefs.GetInt("StartCount", 1);
        _gold = PlayerPrefs.GetInt("Gold", 0);
    }
    
    public void Save()
    {
        PlayerPrefs.SetInt("Level", _level);
        PlayerPrefs.SetInt("StartCount", _startCount);
        PlayerPrefs.SetInt("Gold", _gold);
    }
}