
using System;
using System.Collections.Generic;

namespace RichPackage.Comparers
{
	//
	// Summary:
	//     A float comparer used by Assertions.Assert performing approximate comparison.
	public class FloatComparer : IEqualityComparer<float>
	{
		private readonly float m_Error;

		private readonly bool m_Relative;

		//
		// Summary:
		//     Default instance of a comparer class with deafult error epsilon and absolute
		//     error check.
		public static readonly FloatComparer s_ComparerWithDefaultTolerance = new FloatComparer(1E-05f);

		//
		// Summary:
		//     Default epsilon used by the comparer.
		public const float kEpsilon = 1E-05f;

		//
		// Summary:
		//     Creates an instance of the comparer.
		//
		// Parameters:
		//   relative:
		//     Should a relative check be used when comparing values? By default, an absolute
		//     check will be used.
		//
		//   error:
		//     Allowed comparison error. By default, the FloatComparer.kEpsilon is used.
		public FloatComparer()
			: this(1E-05f, relative: false)
		{
		}

		//
		// Summary:
		//     Creates an instance of the comparer.
		//
		// Parameters:
		//   relative:
		//     Should a relative check be used when comparing values? By default, an absolute
		//     check will be used.
		//
		//   error:
		//     Allowed comparison error. By default, the FloatComparer.kEpsilon is used.
		public FloatComparer(bool relative)
			: this(1E-05f, relative)
		{
		}

		//
		// Summary:
		//     Creates an instance of the comparer.
		//
		// Parameters:
		//   relative:
		//     Should a relative check be used when comparing values? By default, an absolute
		//     check will be used.
		//
		//   error:
		//     Allowed comparison error. By default, the FloatComparer.kEpsilon is used.
		public FloatComparer(float error)
			: this(error, relative: false)
		{
		}

		//
		// Summary:
		//     Creates an instance of the comparer.
		//
		// Parameters:
		//   relative:
		//     Should a relative check be used when comparing values? By default, an absolute
		//     check will be used.
		//
		//   error:
		//     Allowed comparison error. By default, the FloatComparer.kEpsilon is used.
		public FloatComparer(float error, bool relative)
		{
			m_Error = error;
			m_Relative = relative;
		}

		public bool Equals(float a, float b)
		{
			return m_Relative ? AreEqualRelative(a, b, m_Error) : AreEqual(a, b, m_Error);
		}

		public int GetHashCode(float obj)
		{
			return base.GetHashCode();
		}

		//
		// Summary:
		//     Performs equality check with absolute error check.
		//
		// Parameters:
		//   expected:
		//     Expected value.
		//
		//   actual:
		//     Actual value.
		//
		//   error:
		//     Comparison error.
		//
		// Returns:
		//     Result of the comparison.
		public static bool AreEqual(float expected, float actual, float error)
		{
			return Math.Abs(actual - expected) <= error;
		}

		//
		// Summary:
		//     Performs equality check with relative error check.
		//
		// Parameters:
		//   expected:
		//     Expected value.
		//
		//   actual:
		//     Actual value.
		//
		//   error:
		//     Comparison error.
		//
		// Returns:
		//     Result of the comparison.
		public static bool AreEqualRelative(float expected, float actual, float error)
		{
			if (expected == actual)
			{
				return true;
			}

			float num = Math.Abs(expected);
			float num2 = Math.Abs(actual);
			float num3 = Math.Abs((actual - expected) / ((num > num2) ? num : num2));
			return num3 <= error;
		}
	}
}