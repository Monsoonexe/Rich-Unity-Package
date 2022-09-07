using Cysharp.Threading.Tasks;

namespace RichPackage.UI
{
    public interface IMessageBox
    {
        EMessageBoxButton Style { get; }

        void Show(in MessageBox.Payload payload);
        void Show(string messageBoxText, string messageBoxTitle = "", EMessageBoxButton style = EMessageBoxButton.OK, bool animate = false, MessageBoxResultCallback resultCallback = null, string button1Text = null, string button2Text = null, string button3Text = null);
        UniTask<EMessageBoxResult> ShowAsync(string messageBoxText, string messageBoxTitle = "", EMessageBoxButton style = EMessageBoxButton.OK, bool animate = false, string button1Text = null, string button2Text = null, string button3Text = null);
    }
}