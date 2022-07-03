using System.Threading;

namespace Cysharp.Threading.Tasks
{
	public static class UnityEventTaskExtensions
	{
		public static UniTask OnInvokeAsync(this UnityEngine.Events.UnityEvent e)
		{
			return e.OnInvokeAsync(CancellationToken.None);
		}
	}
}
