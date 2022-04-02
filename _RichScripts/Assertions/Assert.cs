
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using RichPackage.Comparers;

namespace RichPackage.Assertions
{
	public class AssertionException : Exception
	{
		private string m_UserMessage;

		public override string Message
		{
			get
			{
				string text = base.Message;
				if (m_UserMessage != null)
				{
					text = text + "\n" + m_UserMessage;
				}

				return text;
			}
		}

		public AssertionException(string message) : base(message) { }

		public AssertionException(string message, string userMessage)
			: base(message)
		{
			m_UserMessage = userMessage;
		}
	}

	//
	// Summary:
	//     The Assert class contains assertion methods for setting invariants in the code.
	[DebuggerStepThrough]
	public static class Assert
	{
		internal const string UNITY_ASSERTIONS = "UNITY_ASSERTIONS";

        /// <summary>
        /// Method that should be used for logging to a console. <br/>
        /// By default it is set to <see cref="UnityEngine.Debug.LogAssertion"/>.
        /// </summary>
        public static Action<string> Logger = (str) => UnityEngine.Debug.LogAssertion(str); //lamba because LogAssertion is [Conditional]

		//
		// Summary:
		//     Set to true to throw a <see cref="AssertionException"/> when assertion methods fail and false if otherwise.
		//     This value defaults to false.
		public static bool raiseExceptions = false;

		private static void Fail(string message, string userMessage)
		{
			if (!raiseExceptions)
			{
				if (message == null)
					message = "Assertion has failed\n";

				if (userMessage != null)
					message = userMessage + "\n" + message;

				Logger(message);
				return;
			}

			throw new AssertionException(message, userMessage);
		}

		//
		// Summary:
		//     Asserts that the condition is true.
		//
		// Parameters:
		//   message:
		//     The string used to describe the Assert.
		//
		//   condition:
		//     true or false.
		[Conditional(UNITY_ASSERTIONS)]
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				IsTrue(condition, null);
			}
		}

		//
		// Summary:
		//     Asserts that the condition is true.
		//
		// Parameters:
		//   message:
		//     The string used to describe the Assert.
		//
		//   condition:
		//     true or false.
		[Conditional(UNITY_ASSERTIONS)]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				Fail(AssertionMessageUtil.BooleanFailureMessage(expected: true), message);
			}
		}

		//
		// Summary:
		//     Return true when the condition is false. Otherwise return false.
		//
		// Parameters:
		//   condition:
		//     true or false.
		//
		//   message:
		//     The string used to describe the result of the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void IsFalse(bool condition)
		{
			if (condition)
			{
				IsFalse(condition, null);
			}
		}

		//
		// Summary:
		//     Return true when the condition is false. Otherwise return false.
		//
		// Parameters:
		//   condition:
		//     true or false.
		//
		//   message:
		//     The string used to describe the result of the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				Fail(AssertionMessageUtil.BooleanFailureMessage(expected: false), message);
			}
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void Is(bool actual, bool expected, string because)
		{
			if (actual != expected)
			{
				Fail(AssertionMessageUtil.BooleanFailureMessage(expected), because);
			}
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNot(bool actual, bool expected, string because)
		{
			if (actual == expected)
			{
				Fail(AssertionMessageUtil.BooleanFailureMessage(expected), because);
			}
		}

		//
		// Summary:
		//     Assert the values are approximately equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreApproximatelyEqual(float expected, float actual)
		{
			AreEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		//
		// Summary:
		//     Assert the values are approximately equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreApproximatelyEqual(float expected, float actual, string message)
		{
			AreEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		//
		// Summary:
		//     Assert the values are approximately equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
		{
			AreApproximatelyEqual(expected, actual, tolerance, null);
		}

		//
		// Summary:
		//     Assert the values are approximately equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreApproximatelyEqual(float expected, 
			float actual, float tolerance, string message)
		{
			AreEqual(expected, actual, message, new FloatComparer(tolerance));
		}

		//
		// Summary:
		//     Asserts that the values are approximately not equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotApproximatelyEqual(float expected, float actual)
		{
			AreNotEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		//
		// Summary:
		//     Asserts that the values are approximately not equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotApproximatelyEqual(float expected, float actual, string message)
		{
			AreNotEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
		}

		//
		// Summary:
		//     Asserts that the values are approximately not equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotApproximatelyEqual(float expected, 
			float actual, float tolerance)
		{
			AreNotApproximatelyEqual(expected, actual, tolerance, null);
		}

		//
		// Summary:
		//     Asserts that the values are approximately not equal.
		//
		// Parameters:
		//   tolerance:
		//     Tolerance of approximation.
		//
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotApproximatelyEqual(float expected, 
			float actual, float tolerance, string message)
		{
			AreNotEqual(expected, actual, message, new FloatComparer(tolerance));
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void AreEqual<T>(T expected, T actual)
		{
			AreEqual(expected, actual, null);
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void AreEqual<T>(T expected, T actual, string message)
		{
			AreEqual(expected, actual, message, EqualityComparer<T>.Default);
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void AreEqual<T>(T expected, T actual, string message, 
			IEqualityComparer<T> comparer)
		{
			if (typeof(Object).IsAssignableFrom(typeof(T)))
			{
				AreEqual(expected as Object, actual as Object, message);
			}
			else if (!comparer.Equals(actual, expected))
			{
				Fail(AssertionMessageUtil.GetEqualityMessage(actual, 
					expected, expectEqual: true), message);
			}
		}

		//
		// Summary:
		//     Assert that the values are equal.
		//
		// Parameters:
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		//
		//   comparer:
		//     Method to compare expected and actual arguments have the same value.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreEqual(Object expected, Object actual, string message)
		{
			if (actual != expected)
			{
				Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, 
					expectEqual: true), message);
			}
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotEqual<T>(T expected, T actual)
		{
			AreNotEqual(expected, actual, null);
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotEqual<T>(T expected, T actual, string message)
		{
			AreNotEqual(expected, actual, message, EqualityComparer<T>.Default);
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotEqual<T>(T expected, T actual, 
			string message, IEqualityComparer<T> comparer)
		{
			if (typeof(Object).IsAssignableFrom(typeof(T)))
			{
				AreNotEqual(expected as Object, actual as Object, message);
			}
			else if (comparer.Equals(actual, expected))
			{
				Fail(AssertionMessageUtil.GetEqualityMessage(actual, 
					expected, expectEqual: false), message);
			}
		}

		//
		// Summary:
		//     Assert that the values are not equal.
		//
		// Parameters:
		//   expected:
		//     The assumed Assert value.
		//
		//   actual:
		//     The exact Assert value.
		//
		//   message:
		//     The string used to describe the Assert.
		//
		//   comparer:
		//     Method to compare expected and actual arguments have the same value.
		[Conditional(UNITY_ASSERTIONS)]
		public static void AreNotEqual(Object expected, Object actual, string message)
		{
			if (actual == expected)
			{
				Fail(AssertionMessageUtil.GetEqualityMessage(
					actual, expected, expectEqual: false), message);
			}
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNull<T>(T value) where T : class
		{
			IsNull(value, null);
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNull<T>(T value, string message) where T : class
		{
			if (typeof(Object).IsAssignableFrom(typeof(T)))
			{
				IsNull(value as Object, message);
			}
			else if (value != null)
			{
				Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: true), message);
			}
		}

		//
		// Summary:
		//     Assert the value is null.
		//
		// Parameters:
		//   value:
		//     The Object or type being checked for.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNull(Object value, string message)
		{
			if (value != null)
			{
				Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: true), message);
			}
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNotNull<T>(T value) where T : class
		{
			IsNotNull(value, null);
		}

		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNotNull<T>(T value, string message) where T : class
		{
			if (typeof(Object).IsAssignableFrom(typeof(T)))
			{
				IsNotNull(value as Object, message);
			}
			else if (value == null)
			{
				Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: false), message);
			}
		}

		//
		// Summary:
		//     Assert that the value is not null.
		//
		// Parameters:
		//   value:
		//     The Object or type being checked for.
		//
		//   message:
		//     The string used to describe the Assert.
		[Conditional(UNITY_ASSERTIONS)]
		public static void IsNotNull(Object value, string message)
		{
			if (value == null)
			{
				Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: false), message);
			}
		}

		#region Exceptions

		public static void ThrowsException<TException>(Action action)
			where TException : Exception
		{
			try
			{
				action(); //should throw
				Fail($"No exception was thrown when calling {action.Method.Name}. " +
					$"Expected {typeof(TException).Name}.", null);
			}
			catch (TException)
			{
				//all good
			}
			catch (Exception ex)
			{
				Fail($"{action.Method.Name} threw {ex.GetType().Name} but was " +
				$"expected to throw {typeof(TException).Name}. {ex.Message}.", null);
			}
		}

		public static void ThrowsException<TException>(Action action, string because)
			where TException : Exception
		{
			try
			{
				action(); //should throw
				Fail($"No exception was thrown when calling {action.Method.Name}. " +
					$"Expected {typeof(TException).Name}.", because);
			}
			catch (TException)
			{
				//all good
			}
			catch (Exception ex)
			{
				Fail($"{action.Method.Name} threw {ex.GetType().Name} but was " +
				$"expected to throw {typeof(TException).Name}. {ex.Message}.", because);
			}
		}

		public static void DoesNotThrowException<TException>(Action action)
			=> DoesNotThrowException<TException>(action, string.Empty);

		public static void DoesNotThrowException<TException>(Action action,
			string because)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Fail($"{action.Method.Name} threw {ex.GetType().Name} but was " +
				$"expected to not throw any exception. {ex.Message}.", because);
			}
		}

		#endregion

		public static string BecauseOf(string because, params object[] becauseArgs)
        {
            string reason;
            try
            {
                string becauseOrEmpty = because ?? string.Empty;
                reason =  (becauseArgs?.Contains((arg) => arg != null) == true) ? 
					string.Format(CultureInfo.InvariantCulture, becauseOrEmpty, becauseArgs)
					: becauseOrEmpty;
            }
            catch (FormatException formatException)
            {
                reason = $"**WARNING** because message '{because}' " +
					$"could not be formatted with " +
					$"string.Format{Environment.NewLine}{formatException.StackTrace}";
            }
            return reason;
        }
		
	}
}
