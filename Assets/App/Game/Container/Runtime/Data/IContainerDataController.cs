using App.Common.Utilities.Utility.Runtime;
using App.Game.Container.Runtime.Data.Model;

namespace App.Game.Container.Runtime.Data
{
    public interface IContainerDataController
    {
        Optional<ContainerData> GetContainer(int guid);
        Optional<ContainerData> CreateContainer(int length);
        Optional<ContainerData> DestroyContainer(int guid);
    }
}