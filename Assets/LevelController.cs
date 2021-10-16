using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int levelsCount => _levels.Count;

    public int currentLevelIndex => _currentLevelIndex;

    [SerializeField] private List<Level> _levels;
    
    private Level _currentLevel;
    private int _currentLevelIndex;

    private void Start()
    {
    }

    public void SpawnLevel(int index)
    {
        _currentLevelIndex = Mathf.Min(index -1, levelsCount -1);
        if (_currentLevel != null)
        {
            Destroy(_currentLevel.gameObject);
        }

        _currentLevel = Instantiate(_levels[_currentLevelIndex], Vector3.zero, Quaternion.identity);
    }
    
    public void OnStateChanged(GameState oldState, GameState newState)
    {
        if (newState == GameState.MENU)
        {
            SpawnLevel(GameManager.Instance.GameSave.level);
        }
    }

}