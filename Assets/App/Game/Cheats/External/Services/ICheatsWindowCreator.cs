using App.Common.AssetSystem.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.View;
using App.Common.Utility.Runtime;

namespace App.Game.Cheats.External.Services
{
    public interface ICheatsWindowCreator
    {
        Optional<CheatsWindow> Create();
    }
}

