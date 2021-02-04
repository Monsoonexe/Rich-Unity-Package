/// <summary>
/// Weight and int.
/// </summary>
/// <seealso cref="RandomIntGenerator"/>
[System.Serializable]
public class WeightedInt : AWeightedProbability<int>
{
    //exists

    public WeightedInt(int weight, int value) 
        : base(weight, value)
    {
        //constructs
    }
}
