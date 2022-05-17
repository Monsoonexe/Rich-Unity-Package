using System;
using UnityEngine;

namespace RichPackage.WeightedProbabilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WeightedInt"/>
    /// <seealso cref="RandomVector2Generator"/>
    [Serializable]
    public class WeightedVector2 : AWeightedProbability<Vector2>
    {
        //exists

        #region Constructors

        public WeightedVector2(int weight, Vector2 value)
            : base(weight, value)
        {
            //constructs
        }

        #endregion
    }
}
