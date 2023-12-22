using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public int ActorNumber { get; }
    public bool IsBot { get; }

    public void MovePawn(int diceValue, Action OnMovementFinished);
    public void Initialize();

}
