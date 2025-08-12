using App.Game.Inventory.External.Dto;
using App.Game.Inventory.Runtime.Config;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Inventory.External.Config
{
    public class InventoryDtoToConfigConverter
    {
        public Optional<InventoryConfig> Convert(InventoryConfigDto dto)
        {
            if (dto == null || dto.Groups == null)
                return Optional<InventoryConfig>.Fail();

            var groups = new List<IInventoryGroupConfig>();
            foreach (var groupDto in dto.Groups)
            {
                var group = new InventoryGroupConfig(
                    groupDto.Id, 
                    groupDto.Icon,
                    groupDto.GameType);
                groups.Add(group);
            }

            var config = new InventoryConfig(
                groups,
                dto.Cols,
                dto.SlotWidth,
                dto.SlotHeight,
                dto.Rows
            );
            return Optional<InventoryConfig>.Success(config);
        }
    }
}
