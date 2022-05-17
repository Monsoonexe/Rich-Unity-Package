using System;

namespace RichPackage.RPGStats
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AgilityBuffItemEffect"/>
    /// <seealso cref="IntelligenceStat"/>
    [Serializable]
    public class AgilityStat : CharacterStat
    {
        public const string _AGILITY = "Agility";
        public override string statName => _AGILITY;
        public new static string StatName => _AGILITY;

        #region Constructors

        public AgilityStat() : base()
        {
            //
        }

        public AgilityStat(float baseValue) : base(baseValue)
        {
            //
        }

        #endregion
    }
}
