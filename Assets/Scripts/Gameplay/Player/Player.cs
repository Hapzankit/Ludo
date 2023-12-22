using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour , IPlayer
{
    [SerializeField] private Pawn _pawns;
    [SerializeField] private int _actorNumber;
    [SerializeField] private bool _isBot;
    public int ActorNumber => _actorNumber;
    public bool IsBot => _isBot;

    public void Initialize()
    {
        _pawns.isBot = _isBot;
        _pawns.Init();
    }
    
    public void MovePawn(int diceValue, Action OnMovementFinished)
    {
        _pawns.canTouch = true;
        StartCoroutine(_pawns.StartMovingPawn(diceValue, OnMovementFinished));
    }

   
}
