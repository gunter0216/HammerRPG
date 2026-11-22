using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace App.Game.Modules.Container.Runtime.Config
{
    [MovedFrom(true, sourceNamespace: "App.Game.Modules.ContainerModule.Runtime.Config")]
    public class ContainerModuleConfig : ModuleConfig
    {
        [SerializeField] private int _rows;
        [SerializeField] private int _cols;

        public ContainerModuleConfig(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
        }

        public int Rows => _rows;

        public int Cols => _cols;
    }
}
