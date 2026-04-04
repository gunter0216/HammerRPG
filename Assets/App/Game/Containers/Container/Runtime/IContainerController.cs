using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Containers.Container.Runtime
{
    public interface IContainerController
    {
        Optional<Container> CreateContainer(int length);
        Optional<Container> GetContainer(int guid);
        void DestroyContainer(int guid);
        void DestroyContainer(Container container);
    }
}