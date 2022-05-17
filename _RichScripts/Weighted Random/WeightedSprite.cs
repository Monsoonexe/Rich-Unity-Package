using System;
using UnityEngine;

namespace RichPackage.WeightedProbabilities
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WeightedSprite : AWeightedProbability<Sprite>
    {
        //exists

        #region Constructors

        public WeightedSprite(int weight, Sprite value)
            : base(weight, value)
        {

        }

        #endregion
    }
}
