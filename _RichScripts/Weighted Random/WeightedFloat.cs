using System;

/// <summary>
/// 
/// </summary>
/// <seealso cref="RandomFloatGenerator"/>
[Serializable]
public class WeightedFloat : AWeightedProbability<float>
{
    //exists

    #region Constructors

    public WeightedFloat(int weight, float value)
        : base(weight, value)
    {
        //constructs
    }

    #endregion
}
