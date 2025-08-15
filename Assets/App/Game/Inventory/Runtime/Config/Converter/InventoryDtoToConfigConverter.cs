using App.Game.Inventory.External.Dto;
using App.Game.Inventory.Runtime.Config;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Inventory.External.Config
{
    public class InventoryDtoToConfigConverter
    {
        public Optional<InventoryConfig> Convert(InventoryConfigDto dto)
        {
            if (dto == null || dto.Groups == null)
                return Optional<InventoryConfig>.Fail();

            var config = new InventoryConfig(dto);
            
            return Optional<InventoryConfig>.Success(config);
        }
    }
}
