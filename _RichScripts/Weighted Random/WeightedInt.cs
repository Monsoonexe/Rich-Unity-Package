using System;

namespace RichPackage.WeightedProbabilities
{
    /// <summary>
    /// Weight and int.
    /// </summary>
    /// <seealso cref="RandomIntGenerator"/>
    [Serializable]
    public class WeightedInt : AWeightedProbability<int>
    {
        //exists

        public WeightedInt(int weight, int value) 
            : base(weight, value)
        {
            //constructs
        }
    }
}
