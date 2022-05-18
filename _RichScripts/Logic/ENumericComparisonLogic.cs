
namespace RichPackage.Logic
{
	/// <summary>
	/// 
	/// </summary>
	//[System.Flags]
	public enum ENumericComparisonLogic
	{
		None = (1 << 0),
		GreaterThan = (1 << 1),
		LesserThan = (1 << 2),
		EqualTo = (1 << 3),
		//Not = (1 << 4),
		NotEqualTo = (1 << 4),
		GreaterThanOrEqual = GreaterThan | EqualTo,
		LesserThanOrEqual = LesserThan | EqualTo,
	}
}
