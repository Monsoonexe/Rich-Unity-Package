
/// <summary>
/// Weight and char.
/// </summary>
[System.Serializable]
public class WeightedChar : AWeightedProbability<char>
{
    //exists
    public WeightedChar(int weight, char value) 
        : base(weight, value)
    {
        //inherited Constructor
    }
}
