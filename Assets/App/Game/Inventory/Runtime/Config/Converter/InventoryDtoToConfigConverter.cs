using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config.Dto;
using App.Game.Inventory.Runtime.Config.Model;

namespace App.Game.Inventory.Runtime.Config.Converter
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
