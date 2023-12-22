
using System;

public interface IDiceService
{
    void RollDice(Action<int> OnDiceRollCompleted);
    
}
