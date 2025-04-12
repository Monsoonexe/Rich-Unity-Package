using System;

namespace RichPackage.Logic
{
    /// <summary>
    /// 
    /// </summary>
    //[System.Flags]
    public enum ENumericComparisonLogic
    {
        None = 0,
        GreaterThan = 1 << 0,
        LesserThan = 1 << 1,
        EqualTo = 1 << 2,
        //Not = (1 << 4),
        NotEqualTo = 1 << 3,
        GreaterThanOrEqual = GreaterThan | EqualTo,
        LesserThanOrEqual = LesserThan | EqualTo,
    }

    public static class ComparisonLogicExtensions
    {
        public static string GetOperationSymbol(this ENumericComparisonLogic operation)
        {
            switch (operation)
            {
                case ENumericComparisonLogic.GreaterThan:
                    return ">";
                case ENumericComparisonLogic.LesserThan:
                    return "<";
                case ENumericComparisonLogic.EqualTo:
                    return "=";
                case ENumericComparisonLogic.NotEqualTo:
                    return "!=";
                case ENumericComparisonLogic.GreaterThanOrEqual:
                    return ">=";
                case ENumericComparisonLogic.LesserThanOrEqual:
                    return "<=";
                default:
                    throw new Exception($"{operation} not implemented!");
            }
        }

        public static ENumericComparisonLogic FromString(this string src)
        {
            switch (src)
            {
                case ">":
                    return ENumericComparisonLogic.GreaterThan;
                case "<":
                    return ENumericComparisonLogic.LesserThan;
                case "=":
                    return ENumericComparisonLogic.EqualTo;
                case "!=":
                    return ENumericComparisonLogic.NotEqualTo;
                case ">=":
                    return ENumericComparisonLogic.GreaterThanOrEqual;
                case "<=":
                    return ENumericComparisonLogic.LesserThanOrEqual;
                default:
                    throw new Exception($"{src} not implemented!");
            }
        }

        public static bool Evaluate<T>(this ENumericComparisonLogic operation, T a, T b)
            where T : IComparable<T>
        {
            int compareValue = a.CompareTo(b);
            bool result;
            switch (operation)
            {
                //case EComparisonLogic.None:
                //                Result = null;
                //                return; //do not continue
                case ENumericComparisonLogic.GreaterThan:
                    result = compareValue > 0;
                    break;
                case ENumericComparisonLogic.LesserThan:
                    result = compareValue < 0;
                    break;
                case ENumericComparisonLogic.EqualTo:
                    result = compareValue == 0;
                    break;
                case ENumericComparisonLogic.NotEqualTo:
                    result = compareValue != 0;
                    break;
                case ENumericComparisonLogic.GreaterThanOrEqual:
                    result = compareValue >= 0;
                    break;
                case ENumericComparisonLogic.LesserThanOrEqual:
                    result = compareValue <= 0;
                    break;
                default:
                    throw new Exception($"{operation} not implemented!");
            }

            return result;
        }
    }
}
