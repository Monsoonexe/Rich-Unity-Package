
namespace RichPackage
{
	public interface IDFactory<T>
	{
		T GetNext();

		void Reset();
	}
}
