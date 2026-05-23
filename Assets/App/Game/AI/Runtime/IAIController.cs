using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using UnityEngine;

namespace App.Game.AI.Runtime
{
    public interface IAIController
    {
        Optional<AIViewController> Create(string id, Vector3 position);
    }
}