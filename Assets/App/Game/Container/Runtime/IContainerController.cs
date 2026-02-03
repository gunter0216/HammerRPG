using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Container.Runtime
{
    public interface IContainerController
    {
        Optional<Container> CreateContainer(IReadOnlyList<IModuleItem> items, int length);
        Optional<Container> GetContainer(int guid);
        void DestroyContainer(int guid);
        void DestroyContainer(Container container);
    }
}