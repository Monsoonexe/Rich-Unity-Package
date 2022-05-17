using System;
using ScriptableObjectArchitecture;

namespace RichPackage.WeightedProbabilities
{
    /// <summary>
    /// Like for percentages. Weighted chances 0.o
    /// </summary>
    [Serializable]
    public class WeightedSOCollection : AWeightedProbability<BaseCollection>
    {
        //exists

        public WeightedSOCollection(int weight, BaseCollection value)
            : base(weight, value)
        {
            //constructs
        }
    }
}
