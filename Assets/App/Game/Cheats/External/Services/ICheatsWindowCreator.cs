using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.View;

namespace App.Game.Cheats.External.Services
{
    public interface ICheatsWindowCreator
    {
        Optional<CheatsWindow> Create();
    }
}

