/* TODO - null-object
    TODO - build from scratch if null
*/

using CommonServiceLocator;
using RichPackage.GuardClauses;
using RichPackage.UI;

namespace RichPackage
{
    public sealed partial class UnityServiceLocator : ServiceLocatorImplBase
    {
        public static IMessageBox MessageBox { get; private set; }

        public void RegisterMessageBox(IMessageBox service)
        {
            GuardAgainst.ArgumentIsNull(service, nameof(service));
            RegisterService(typeof(IMessageBox), service);
            MessageBox = service;
        }

        public void DeregisterMessageBox(IMessageBox service)
        {
            GuardAgainst.ArgumentIsNull(service, nameof(service));
            DeregisterService(typeof(IMessageBox));
            MessageBox = null;
        }
    }
}
