using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{

    [SerializeField] private int initialPosition;
    private int currentPos;
    private bool canMove;
    [HideInInspector]public bool isBot;
    [HideInInspector]public bool canTouch;


    public void Init()
    {
       transform.position = GameManager.Instance.Paths.Find(x => x.name == initialPosition.ToString()).position;
       currentPos = initialPosition;
    }

    private void OnMouseDown()
    {
        if (canTouch)
        {
            
         canMove = true;
        }
    }

    public void Move()
    {
        currentPos += 1;
        transform.position = GameManager.Instance.Paths.Find(x => x.name == currentPos.ToString()).position;
    }
    
    public IEnumerator StartMovingPawn(int diceValue , Action callback)
    {
        if (!isBot)
        {
            yield return new WaitUntil(() => canMove);
        }
        
        
        for (int i = 0; i < diceValue; i++)
        {
            Move();
            yield return new WaitForSeconds(0.5f);
        }

        callback?.Invoke();
        canMove = false;
        canTouch = false;
    }
}
