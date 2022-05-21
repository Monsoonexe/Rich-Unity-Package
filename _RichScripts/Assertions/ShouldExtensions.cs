using System;
using System.Diagnostics;
using RichPackage;

using Object = UnityEngine.Object;

namespace RichPackage.Assertions
{
    public static class ShouldExtensions
    {
		#region Null

		[Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeNull(this object obj)
            => Assert.IsNull(obj);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeNull(this object obj, string because)
            => Assert.IsNull(obj, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeNull(this object obj, string because,
            params string[] becauseArgs)
            => Assert.IsNull(obj, Assert.BecauseOf(because, becauseArgs));

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotBeNull(this object obj)
            => Assert.IsNotNull(obj);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotBeNull(this object obj, Object unityContext)
            => Assert.IsNotNull(obj, unityContext);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotBeNull(this object obj, string because)
            => Assert.IsNotNull(obj, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotBeNull(this object obj, string because,
            params string[] becauseArgs)
            => Assert.IsNotNull(obj, Assert.BecauseOf(because, becauseArgs));

        #endregion Null

        #region Boolean

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBe(this bool actual, bool expected, string because = "")
		    => Assert.Is(actual, expected, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotBe(this bool actual, bool expected, string because = "")
            => Assert.IsNot(actual, expected, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeTrue(this bool obj)
            => Assert.IsTrue(obj);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeTrue(this bool obj, string because)
            => Assert.IsTrue(obj, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeTrue(this bool obj, string because, 
            params string[] becauseArgs)
            => Assert.IsTrue(obj, Assert.BecauseOf(because, becauseArgs));

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeFalse(this bool obj)
            => Assert.IsFalse(obj);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeFalse(this bool obj, string because)
            => Assert.IsFalse(obj, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeFalse(this bool obj, string because, 
            params string[] becauseArgs)
            => Assert.IsFalse(obj, Assert.BecauseOf(because, becauseArgs));

		#endregion

		#region Comparisons

		#region Integer Comparisons

		[Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBe(this int expected, int actual)
            => Assert.AreEqual(expected, actual, string.Empty);

		[Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBe(this int expected, int actual, string because)
            => Assert.AreEqual(expected, actual, because);
        
		[Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBe(this int expected, int actual, string because, 
            params string[] becauseArgs)
            => Assert.AreEqual(expected, actual, Assert.BecauseOf(because, becauseArgs));

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeGreaterThan(this int expected, int actual)
            => Assert.IsTrue(expected > actual);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeGreaterThan(this int expected, int actual, string because)
            => Assert.IsTrue(expected > actual, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldBeGreaterThan(this int expected, int actual, string because,
            params string[] becauseArgs)
            => Assert.IsTrue(expected > actual, Assert.BecauseOf(because, becauseArgs));

        #endregion

        //TODO greater than or equal to
        //TODO less than
        //TODO less than or equal to
        //TODO positive, negative, zero
        //TODO be in range
        //TODO be one of int[]
        //TODO the above, but for floats
        //TODO the above, but for the less-common primitives (long, byte...)

        #endregion Comparisons

        #region Exceptions

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldThrow<TException>(this Action subject)
            where TException : Exception
            => Assert.ThrowsException<TException>(subject);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldThrow<TException>(this Action subject,
            string because)
            where TException : Exception
            => Assert.ThrowsException<TException>(subject, because);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotThrow<TException>(this Action subject)
            where TException : Exception
            => Assert.DoesNotThrowException<TException>(subject);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void ShouldNotThrow<TException>(this Action subject,
            string because)
            where TException : Exception
            => Assert.DoesNotThrowException<TException>(subject, because);

        //TODO - lambda has no name and so report will be confusing
        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void InvokingShouldThrow<TException>(this object subject,
            Action<object> action)
            where TException : Exception
            => Assert.ThrowsException<TException>(() => action(subject));

        //TODO - lambda has no name and so report will be confusing
        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void InvokingShouldThrow<TException>(this object subject,
            Action<object> action, string because)
            where TException : Exception
            => Assert.ThrowsException<TException>(() => action(subject), because);

        //TODO - lambda has no name and so report will be confusing
        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void InvokingShouldNotThrow<TException>(this object subject,
            Action<object> action)
            where TException : Exception
            => Assert.DoesNotThrowException<TException>(() => action(subject));

        //TODO - lambda has no name and so report will be confusing
        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void InvokingShouldNotThrow<TException>(this object subject,
            Action<object> action, string because)
            where TException : Exception
            => Assert.DoesNotThrowException<TException>(() => action(subject), because);

        #endregion

        #region Lists

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void IndexShouldBeInRange(this System.Collections.IList list,
            int index)
            => Assert.IndexIsInRange(list, index);

        [Conditional(Assert.UNITY_ASSERTIONS)]
        public static void IndexShouldBeInRange(this System.Collections.IList list,
            int index, string message)
            => Assert.IndexIsInRange(list, index, message);

        #endregion
    }
}
