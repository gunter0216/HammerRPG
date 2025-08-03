using App.Game.Inventory.External.Dto;
using App.Game.Inventory.Runtime.Config;
using App.Common.Utility.Runtime;
using System.Collections.Generic;

namespace App.Game.Inventory.External.Config
{
    public class InventoryDtoToConfigConverter
    {
        public Optional<InventoryConfig> Convert(InventoryConfigDto dto)
        {
            if (dto == null || dto.Groups == null)
                return Optional<InventoryConfig>.Fail();

            var groups = new List<InventoryGroup>();
            foreach (var groupDto in dto.Groups)
            {
                var group = new InventoryGroup(groupDto.Id, groupDto.Icon);
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
