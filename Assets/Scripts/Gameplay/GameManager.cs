using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Button _rollCommandButton;
    [SerializeField] private Button _resetPawnBtn;
    [SerializeField] private List<Player> _players;
    [SerializeField] private Transform _boardTransform;


    private IDiceService _diceService;
    private IPlayer _currentPLayer;
    private bool canPlay;
    private Queue<IPlayer> _playerQueue;
    private static GameManager _instance;
    
    public List<Transform> Paths;
    public static GameManager Instance => _instance;

    private Action<int> OnDiceRollFinished;
    private Action OnNewTurn;

    private void Awake()
    {
        _instance = this;
        Initialize();
    }

    private void Initialize()
    {
        InitilizeDice();
        InitializeGame();
        InitializePawn();
        InitializeCallbacks();
    }

    private void InitilizeDice()
    {
        
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Dice.prefab").Completed += (handle) =>
        {
            if (handle.Result != null)
            {
                var gb = Instantiate(handle.Result);
                _diceService = gb.GetComponent<IDiceService>();
            }
            else
            {
                Debug.LogError("Failed to load prefab");
            }

        };
    }

    private void InitializeGame()
    {
        
        _players.OrderBy(x => x.ActorNumber);
        _playerQueue = new Queue<IPlayer>(_players);
        Paths = _boardTransform.GetComponentsInChildren<Transform>().ToList();
        canPlay = true;
        _currentPLayer = _playerQueue.Peek();
    }

    private void InitializePawn()
    {
        foreach (var player in _players)
        {
            player.Initialize();
        }
    }

    private void InitializeCallbacks()
    {
        _rollCommandButton.onClick.AddListener(delegate
        {
            if (!_currentPLayer.IsBot && canPlay)
            {
                Play();
                canPlay = false;
            }
        });
        
        _resetPawnBtn.onClick.AddListener(InitializePawn);
        
        OnDiceRollFinished += OnDiceRolledCallback;
        OnNewTurn += OnNewTurnCallback;
    }
    

    private async void Play()
    {
        await NetworkCommunication.Instance.GetNumber();
        _diceService.RollDice(OnDiceRollFinished);
    }

    private void OnDiceRolledCallback(int faceValue)
    {
        _currentPLayer.MovePawn(faceValue , OnNewTurn);
    }

    private void OnNewTurnCallback()
    {
        _playerQueue.Dequeue();
        _playerQueue.Enqueue(_currentPLayer);
        _currentPLayer = _playerQueue.Peek();
        
        if (_currentPLayer.IsBot)
        {
            Invoke("Play" , 3);
        }
        else
        {
            canPlay = true;
        }
        
        Debug.Log("New player " + _currentPLayer.ActorNumber);
    }
    
    
}
