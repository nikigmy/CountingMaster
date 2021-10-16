using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : Singleton<PlayerController>
{
    public Action OnColorChanged;
    public int Score => _dummyController.activeDummies.Count;

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Material _goopMaterial;
    [SerializeField] private Material _splashMaterial;
    [SerializeField] private Material _dummyMaterial;
    [SerializeField] private DummyController _dummyController;
    [SerializeField] private float _formawrdMovementSpeed = 3;
    [SerializeField] private float _sideMovementSpeed = 3;
    [SerializeField]private float _playerWidth = 0.5f; 
    [SerializeField]private float _minXPosition;
    [SerializeField]private float _maxXPosition;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (_gameManager.currentGameState == GameState.PLAYING)
        {
            transform.position += Vector3.forward * _formawrdMovementSpeed * Time.deltaTime;
            UpdateConstaints();
            ConstraintPlayer();
            var currPos = transform.position.x;
            var input = _playerInput.CurrentInput.x;
            if (Input.GetKey(KeyCode.A))
            {
                input = -1;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                input = 1;
            }
            if (input < 0)
            {
                if (currPos > _minXPosition + _playerWidth)
                {
                    var maxDist = Mathf.Abs(currPos - (_minXPosition + _playerWidth));
                    currPos += Mathf.Min(_sideMovementSpeed * Time.deltaTime * input, maxDist);
                }

            }
            else if (input > 0)
            {
                if (currPos < _maxXPosition - _playerWidth)
                {
                    var maxDist = Mathf.Abs(currPos - (_maxXPosition - _playerWidth));
                    currPos += Mathf.Min(_sideMovementSpeed * Time.deltaTime * input, maxDist);
                }
            }

            transform.position = new Vector3(currPos, transform.position.y, transform.position.z);
        }
    }

    private void ConstraintPlayer()
    {
        var currPos = transform.position;
        if (currPos.x < _minXPosition)
        {
            currPos.x = _minXPosition;
        }
        if (currPos.x > _maxXPosition)
        {
            currPos.x = _maxXPosition;
        }

        transform.position = currPos;
    }

    private void UpdateConstaints()
    {
        var minX = float.MaxValue;
        var maxX = float.MinValue;
        foreach (var dummy in _dummyController.activeDummies)
        {
            var pos = dummy.transform.position;
            var rightPos = pos.x + _playerWidth;
            var leftPos = pos.x - _playerWidth;
            if (rightPos > maxX)
            {
                maxX = rightPos;
            }
            if (leftPos < minX)
            {
                minX = leftPos;
            }
        }

        var playerSize = maxX - minX;
        _minXPosition = -_gameManager.arenaWidth / 2 + playerSize / 2;
        _maxXPosition = _gameManager.arenaWidth / 2 - playerSize / 2;
    }

    [ContextMenu("Set Color")]
    public void SetNextColor()
    {
        SetColor(new Color(Random.Range(0,1.0f), Random.Range(0,1.0f), Random.Range(0,1.0f)));
        OnColorChanged?.Invoke();
        _gameManager.analytics?.ReportColorChanged();
    }
    
    public void SetColor(Color color)
    {
        _goopMaterial.color = color;
        _goopMaterial.SetColor("_EmissionColor", color);
        _splashMaterial.color = color;
        _splashMaterial.SetColor("_EmissionColor", color);
        _dummyMaterial.color = color;
    }

    public void AddCount(int number)
    {
        _dummyController.Add(number);
    }

    public void MultiplyCount(int number)
    {
        _dummyController.Multiply(number);
    }

    public void Die()
    {
        _gameManager.Die();
    }
}