
namespace RichPackage.Logic
{
	/// <summary>
	/// 
	/// </summary>
	//[System.Flags]
	public enum ENumericComparisonLogic
	{
		None = 0,
		GreaterThan = (1 << 0),
		LesserThan = (1 << 1),
		EqualTo = (1 << 2),
		//Not = (1 << 4),
		NotEqualTo = (1 << 3),
		GreaterThanOrEqual = GreaterThan | EqualTo,
		LesserThanOrEqual = LesserThan | EqualTo,
	}
}
