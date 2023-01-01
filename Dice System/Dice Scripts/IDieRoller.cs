using System.Collections.Generic;

namespace RichPackage.DiceSystem
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDieRoller
    {
        IList<int> RollDice(int diceNumber);
        
        void RollDice(int diceNumber, IList<int> results);

        IList<int> RollDiceOdds(int diceCount);

        void RollDiceOdds(int diceCount, IList<int> results);
    }
}
