using App.Game.Canvases.External;

namespace App.Common.Canvases.External
{
    public interface ICanvasController
    {
         ICanvas GetHudCanvas();
         ICanvas GetWindowCanvas();
         ICanvas GetMenuCanvas();
    }
}