using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const string LEVEL_FORMAT = "Level: {0}";
    private const string COUNT_FORMAT = "Count: {0}";
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _failScreen;
    [SerializeField] private GameObject _nextlevelButton;
    [SerializeField] private TextMeshProUGUI _gameScreenLevel;
    [SerializeField] private TextMeshProUGUI _gameScreenCount;
    
    public void OnStateChanged(GameState oldState, GameState newState)
    {
        switch (oldState)
        {
            case GameState.MENU:
                _menuScreen.SetActive(false);
                break;
            case GameState.PLAYING:
                _gameScreen.SetActive(false);
                break;
            case GameState.PAUSED:
                _pauseScreen.SetActive(false);
                break;
            case GameState.DEAD:
                _failScreen.SetActive(false);
                break;
            case GameState.FINISH:
                _nextlevelButton.SetActive(false);
                break;
            default:
                break;
        }
        
        switch (newState)
        {
            case GameState.MENU:
                _menuScreen.SetActive(true);
                break;
            case GameState.PLAYING:
                UpdateLevelText();
                UpdateCountText();
                _gameScreen.SetActive(true);
                break;
            case GameState.PAUSED:
                _pauseScreen.SetActive(true);
                break;
            case GameState.DEAD:
                _failScreen.SetActive(true);
                break;
            case GameState.FINISH:
                _gameScreen.SetActive(true);
                _nextlevelButton.SetActive(true);
                break;
            case GameState.NONE:
                _menuScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DummyController.Instance.onDummyCountChanged += UpdateCountText;
    }

    private void UpdateCountText()
    {
        _gameScreenCount.text = string.Format(COUNT_FORMAT, PlayerController.Instance.Score);
    }

    private void UpdateLevelText()
    {
        _gameScreenLevel.text = string.Format(LEVEL_FORMAT, GameManager.Instance.GameSave.level);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
