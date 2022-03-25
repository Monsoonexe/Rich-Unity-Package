
using System;

namespace RichPackage.Assertions
{
	internal class AssertionMessageUtil
	{
		private const string Expected = "Expected:";

		private const string AssertionFailed = "Assertion failure.";

		public static string GetMessage(string failureMessage)
		{
			return string.Format("{0} {1}", AssertionFailed, failureMessage);
		}

		public static string GetMessage(string failureMessage, string expected)
		{
			return GetMessage(string.Format("{0}{1}{2} {3}", failureMessage, Environment.NewLine, Expected, expected));
		}

		public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
		{
			return GetMessage(string.Format("Values are {0}equal.", expectEqual ? "not " : ""), string.Format("{0} {2} {1}", actual, expected, expectEqual ? "==" : "!="));
		}

		//public static string GetExceptionMessage<TExpectedEx, TActualEx>(TActualEx actual) 
		//	where TActualEx : Exception
		//	where TExpectedEx : Exception
		//	=> GetMessage($"Call threw {typeof(TActualEx).Name} but was " +
		//		$"expected to throw {typeof(TExpectedEx).Name}. {actual.Message}.");

		public static string NullFailureMessage(object value, bool expectNull)
		{
			return GetMessage(string.Format("Value was {0}Null", expectNull ? "not " : ""), string.Format("Value was {0}Null", expectNull ? "" : "not "));
		}

		public static string BooleanFailureMessage(bool expected)
		{
			return GetMessage("Value was " + !expected, expected.ToString());
		}
	}
}