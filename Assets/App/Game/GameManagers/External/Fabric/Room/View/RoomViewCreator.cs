using App.Common.Utility.Runtime;
using App.Game.GameManagers.External.View;
using UnityEngine;

namespace App.Game.GameManagers.External.Services
{
    public class RoomViewCreator
    {
        public Optional<IRoomView> Create()
        {
            var content = new GameObject();
            var view = new RoomView(content.transform);
            
            return Optional<IRoomView>.Success(view);
        }
    }
}