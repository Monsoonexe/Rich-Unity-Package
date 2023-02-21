using System;

namespace RichPackage
{
	public static class ExceptionUtilities
	{
		public static string GetEnumMemberNotAccountedExceptionString<TEnum>(TEnum value)
			where TEnum : struct, Enum
        {
			return $"{value} is not implemented. You should do so!";
		}

		public static Exception GetInvalidEnumCaseException<TEnum>(TEnum value)
			where TEnum : struct, Enum
		{
			return new NotImplementedException(
				GetEnumMemberNotAccountedExceptionString(value));
		}
	}
}
