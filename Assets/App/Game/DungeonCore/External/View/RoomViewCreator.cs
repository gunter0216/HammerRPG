using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Room;
using UnityEngine;

namespace App.Game.GameManagers.External.Fabric.Room.View
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