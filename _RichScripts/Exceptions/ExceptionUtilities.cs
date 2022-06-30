using System;

namespace RichPackage
{
	public static class ExceptionUtilities
	{
		public static string GetEnumMemberNotImplementedExceptionString<TEnum>(TEnum value)
			where TEnum : Enum
		{
			return $"{value} is not implemented. You should do so!";
		}

		public static Exception GetEnumNotImplementedException<TEnum>(TEnum value)
			where TEnum : Enum
		{
			return new NotImplementedException(GetEnumMemberNotImplementedExceptionString(value));
		}
	}
}
