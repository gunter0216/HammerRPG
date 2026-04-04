using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.ContainerModule.Runtime
{
    public class ContainerModuleConfig : IModuleConfig
    {
        private readonly int _rows;
        private readonly int _cols;

        public ContainerModuleConfig(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
        }

        public int Rows => _rows;

        public int Cols => _cols;
    }
}