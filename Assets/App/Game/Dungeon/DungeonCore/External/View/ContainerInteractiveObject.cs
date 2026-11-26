using App.Common.ModuleItem.Runtime;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Modules.Container.Runtime;
using App.Game.Modules.IssuedItems.Runtime;
using Assets.App.Game.Interactions.Runtime;
using UnityEngine;
using Zenject;

namespace App.Game.Dungeon.DungeonCore.External.View
{
    public class ContainerInteractiveObject : InteractableView
    {
        private IContainerWindowController _window;
        private IModuleItemsManager _itemsManager;

        [Inject]
        private void Construct([InjectOptional] IContainerWindowController window,
            [InjectOptional] IModuleItemsManager itemsManager)
        {
            _window = window;
            _itemsManager = itemsManager;
            if (_window != null)
                OnClickCallback = OpenContainer;
        }

        private void OpenContainer()
        {
            if (!TryGetComponent<CreatingModuleItemView>(out var view) || view.ModuleItem == null ||
                !view.ModuleItem.TryGetModule<ContainerModule>(out var container))
            {
                Debug.LogError("The interactive object has no bound container module.", this);
                return;
            }

            if (view.ModuleItem.TryGetModule<IssuedItemsModule>(out var items) &&
                !items.TryIssueItems(_itemsManager, container))
                Debug.LogError("Could not stock the container with its configured items.", this);

            _window.OpenWindow(container.Container);
        }
    }
}
