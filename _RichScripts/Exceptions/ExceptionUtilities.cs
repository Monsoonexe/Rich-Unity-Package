﻿using System;

namespace RichPackage
{
	public static class ExceptionUtilities
	{
		public static string GetEnumMemberNotAccountedExceptionString<TEnum>(TEnum value)
			where TEnum : Enum
		{
			return $"{value} is not implemented. You should do so!";
		}

		public static Exception GetEnumNotAccountedException<TEnum>(TEnum value)
			where TEnum : Enum
		{
			return new ArgumentOutOfRangeException(
				GetEnumMemberNotAccountedExceptionString(value));
		}
	}
}
