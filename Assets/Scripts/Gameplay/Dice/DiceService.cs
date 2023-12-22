using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceService : MonoBehaviour , IDiceService
{

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private List<Sprite> _diceFaces = new();
    
    [Tooltip("Give the sprites for each frame of animation ")]
    [SerializeField] List<Sprite> _animationFrames = new();
    
    [Tooltip("This is used to calculate random number between the x and y to roll the dice random number of time")]
    [SerializeField] private Vector2 _diceRollRange;
    
    
    public void RollDice(Action<int> OnDiceRollCompleted)
    {
        var value = NetworkCommunication.Instance.randomNumber;
        StartCoroutine(PlayFrame(value , OnDiceRollCompleted));
    }

    private IEnumerator PlayFrame(int value , Action<int> OnDiceRollCompleted)
    {
        int numberOfRoll = (int)Random.Range(_diceRollRange.x, _diceRollRange.y);
        for(int i = 0 ; i < numberOfRoll ; i++)
        {
            foreach (var frame in _animationFrames)
            {
                _renderer.sprite = frame;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }
   
        Debug.Log("value " + value);
        _renderer.sprite = _diceFaces[value - 1];
        OnDiceRollCompleted?.Invoke(value);
    }
}
