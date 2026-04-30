using System.Collections.Generic;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition
{
    public class SquareGenerationCash : IGenerationCash
    {
        private readonly List<SquareArea> _areas;

        public SquareGenerationCash(List<SquareArea> areas)
        {
            _areas = areas;
        }

        public IReadOnlyList<SquareArea> Areas => _areas;
    }
}