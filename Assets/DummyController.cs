using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyController : Singleton<DummyController>
{
    public Action onDummyCountChanged;
    private PlayerController _player;
    [SerializeField] private Dummy _dummyPrefab;
    [SerializeField] private Transform _dummyContaner;
    [SerializeField] private Transform _dummyPoolContaner;
    [SerializeField] private float _dummySpeed = 0.02f;
    [SerializeField] private float _maxSpawnPositionOffset = 0.1f;
    [SerializeField] private int _startPoolSize = 200;
    [SerializeField] private int _maxDummyCreationAmmount = 20;
    [SerializeField] private int _maxDummySpawnAmmount = 20;

    [SerializeField] private List<Dummy> _activeDummies = new List<Dummy>();
    [SerializeField] private List<Dummy> _dummyPool = new List<Dummy>();

    private int _menuPoolSize => GameManager.Instance.GameSave.startCount;

    public List<Dummy> activeDummies => _activeDummies;

    private void Awake()
    {
        _player = PlayerController.Instance;
    }

    public void OnStateChanged(GameState oldState, GameState newState)
    {
        if (oldState == GameState.MENU && newState == GameState.PLAYING)
        {
            PopulateDummyPool(_startPoolSize);
        }

        if (newState == GameState.MENU &&
            (oldState == GameState.PLAYING || oldState == GameState.DEAD || oldState == GameState.FINISH))
        {
            TrimPool();
            CleanupPlayers();
        }
    }

    private void CleanupPlayers()
    {
        var target = GameManager.Instance.GameSave.startCount;
        for (int i = activeDummies.Count - 1; i >= target; i--)
        {
            Destroy(activeDummies[i].gameObject);
            activeDummies.RemoveAt(i);
        }
    }

    private void Start()
    {
        Add(1);
    }

    private void Update()
    {
        for (var i = activeDummies.Count - 1; i >= 0; i--)
        {
            var dummy = activeDummies[i];
            var dir = -dummy.transform.localPosition;
            if (dir.magnitude > 0.02f)
            {
                var newPos = dummy.transform.localPosition + dir.normalized * _dummySpeed * Time.deltaTime;
                dummy.transform.localPosition = newPos;
            }
        }
    }

    public void Add(int number)
    {
        StartCoroutine(AddDummies(number));
    }

    public void Multiply(int number)
    {
        Add(activeDummies.Count * (number -1));
    }

    private IEnumerator AddDummies(int number, List<Dummy> dummies = null)
    {
        if (number <= 0)
        {
            yield break;
        }

        if (dummies == null)
        {
            dummies = new List<Dummy>(number);
        }
        if (number > dummies.Count)
        {
            dummies.AddRange(GetDummiesFromPool(number - dummies.Count));
        }

        while (number > 0)
        {
            var dummiesToSpawn = Mathf.Min(number, activeDummies.Count);
            dummiesToSpawn = Mathf.Min(dummiesToSpawn, _maxDummySpawnAmmount);
            if(dummiesToSpawn == 0)
            {
                dummiesToSpawn = 1;
                AddDummy(_player.transform.position, ref dummies, true);
            }
            else
            {
                for (int i = dummiesToSpawn - 1; i >= 0; i--)
                {
                    AddDummy(activeDummies[i].transform.position, ref dummies, true);
                }
            }

            var dummiesLeft = number - dummiesToSpawn;
            number = dummiesLeft;
            if (dummiesLeft <= 0)
            {
                break;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void AddDummy(Vector3 position, ref List<Dummy> dummies, bool addRandom)
    {
        var dummy = dummies[dummies.Count - 1];
        activeDummies.Add(dummy);
        dummies.Remove(dummy);
        if (addRandom)
        {
            position += GetRandomPosition();
        }

        dummy.transform.position = position;
        dummy.transform.SetParent(_dummyContaner);
        dummy.gameObject.SetActive(true);
        onDummyCountChanged?.Invoke();
    }

    private void RemoveDummy(Dummy dummy)
    {
        dummy.transform.position = _dummyPoolContaner.position;
        dummy.transform.SetParent(_dummyPoolContaner);
        _dummyPool.Add(dummy);
        dummy.gameObject.SetActive(false);
    }

    private void OnDummyDestroyed(Dummy dummy)
    {
        activeDummies.Remove(dummy);
        onDummyCountChanged?.Invoke();
    }
    
    private void OnDummyCleanup(Dummy dummy)
    {
        RemoveDummy(dummy);
        if (_activeDummies.Count == 0)
        {
            _player.Die();
        }
    }

    #region Pool
    
    private IEnumerator PopulatePoolRoutine(int amount, bool force)
    {
        while (true)
        {
            var dummiesToGenerate = force ? amount : Mathf.Min(amount, _maxDummyCreationAmmount);
            for (int i = 0; i < dummiesToGenerate; i++)
            {
                var dummy = Instantiate(_dummyPrefab, _dummyPoolContaner.position,
                    Quaternion.identity, _dummyPoolContaner);
                dummy.onDestroyed += OnDummyDestroyed;
                dummy.onCleanup += OnDummyCleanup;
                dummy.gameObject.SetActive(false);
                _dummyPool.Add(dummy);
            }

            amount -= dummiesToGenerate;
            if (amount <= 0)
            {
                break;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }

        }
    }
    
    private List<Dummy> GetDummiesFromPool(int count)
    {
        var result = new List<Dummy>(count);
        if (_dummyPool.Count > count)
        {
            var startIndex = _dummyPool.Count - count;
            result = _dummyPool.Skip(startIndex).ToList();
            _dummyPool.RemoveRange(startIndex, count);
        }
        else if(_dummyPool.Count > 0)
        {
            result.AddRange(_dummyPool);
            _dummyPool.Clear();
        }

        var dummiesToSpawn = count - result.Count;
        for (int i = 0; i < dummiesToSpawn; i++)
        {
            var dummy = Instantiate(_dummyPrefab, Vector3.zero,
                Quaternion.identity, _dummyContaner);
            dummy.onDestroyed += OnDummyDestroyed;
            dummy.onCleanup += OnDummyCleanup;
            result.Add(dummy);
            dummy.gameObject.SetActive(false);
        }

        return result;
    }

    private void PopulateDummyPool(int amount = 0, bool force = false)
    {
        if (amount <= 0)
        {
            amount = GameManager.Instance.GameSave.startCount;
        }

        StartCoroutine(PopulatePoolRoutine(amount, force));
    }
                                                                         
    private void TrimPool(int targetSize = 0)                                        
    {
        if (targetSize == 0)
        {
            targetSize = _menuPoolSize;
        }
        for (int i = _dummyPool.Count - 1; i >= targetSize; i--)
        {
            Destroy(_dummyPool[i].gameObject);
            _dummyPool.RemoveAt(i);
        }
    }
    
    #endregion
    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(0, _maxSpawnPositionOffset), 0, Random.Range(0, _maxSpawnPositionOffset));
    }
    
#if UNITY_EDITOR
    [SerializeField] private int multAmount;
    [ContextMenu("Multiply")]
    public void MultiplyDebug()
    {
        Multiply(multAmount);
    }
    
    [SerializeField] private int addAmount;
    [ContextMenu("Add")]
    
    public void AddDebug()
    {
        Add(addAmount);
    }
#endif
}
