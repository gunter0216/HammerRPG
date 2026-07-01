using System.Collections.Generic;
using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Inventory.Runtime.Config.Converter;
using App.Game.Inventory.Runtime.Config.Loader;
using App.Game.Inventory.Runtime.Config.Model;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryConfigService : IInventoryConfigService
    {
        private readonly IConfigLoader _configLoader;
        
        private InventoryGameConfig _config;

        public InventoryConfigService(IConfigLoader configLoader)
        {
            _configLoader = configLoader;
        }
        
        public bool Initialize()
        {
            var configLoader = new InventoryConfigLoader(_configLoader);
            var config = configLoader.Load();
            if (!config.HasValue)
            {
                HLogger.LogError("InventoryConfig is null");
                return false;
            }
            
            _config = config.Value;

            return true;
        }

        public int GetCols()
        {
            return _config.Cols;
        }

        public int GetRows()
        {
            return _config.Rows;
        }
    }
}