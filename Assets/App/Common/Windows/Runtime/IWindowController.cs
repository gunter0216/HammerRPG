using App.Common.Windows.Runtime;

namespace App.Common.Windows.External
{
    public interface IWindowController
    {
        void SetActive(bool status);
        WindowNames GetName();
    }
}