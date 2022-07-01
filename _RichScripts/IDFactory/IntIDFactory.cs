
namespace RichPackage
{
    public class IntIDFactory : AIDFactory<int>
    {
        public override int GetNext()
        {
            return previousID++;
        }

        public override void Reset()
        {
            previousID = 0;
        }

        public int Peek() => previousID + 1;
    }
}
