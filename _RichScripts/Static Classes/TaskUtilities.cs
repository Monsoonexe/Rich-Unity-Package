using System.Diagnostics;
using System.Threading.Tasks;

namespace RichPackage.Threading.Tasks
{
    public static class TaskUtilities
    {
        /// <summary>
        /// Removes compiler warning caused by not awaiting an awaitable.
        /// </summary>
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public static void Forget(this Task t)
        {
            // nada
        }
    }
}
