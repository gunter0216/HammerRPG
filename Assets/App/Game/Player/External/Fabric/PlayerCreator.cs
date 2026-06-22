using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Experience.Runtime.Config;
using App.Game.Modules.Experience.Runtime.Data;
using App.Game.Modules.Level.Runtime.Config;
using App.Game.Modules.Level.Runtime.Data;
using App.Game.Modules.Name.Runtime.Config;
using App.Game.Modules.Name.Runtime.Data;
using App.Game.Modules.Race.Runtime.Config;
using App.Game.Modules.Race.Runtime.Data;
using App.Game.Modules.Stats.Runtime.Config;
using App.Game.Modules.Stats.Runtime.Data;

namespace App.Game.Player.External
{
    public class PlayerCreator
    {
        private readonly IModuleItemsManager _moduleItemsManager;

        public PlayerCreator(IModuleItemsManager moduleItemsManager)
        {
            _moduleItemsManager = moduleItemsManager;
        }

        public IModuleItem Create()
        {
            var moduleItem = _moduleItemsManager.Create("player");
            if (!moduleItem.HasValue)
            {
                HLogger.LogError("Cant create player.");
                return null;
            }
            
            InitStats(moduleItem.Value);
            InitName(moduleItem.Value);
            InitLevel(moduleItem.Value);
            InitExperience(moduleItem.Value);
            InitRace(moduleItem.Value);

            return moduleItem.Value;
        }

        private void InitStats(IModuleItem player)
        {
            if (!player.TryGetDataModule<StatsModuleData>(out var statsModuleData))
            {
                if (player.TryGetConfigModule<StatsModuleConfig>(out var config))
                {
                    statsModuleData = new StatsModuleData()
                    {
                        Agility = config.Agility,
                        Strength = config.Strength,
                        Intelligence = config.Intelligence
                    };
                    
                    player.AddDataModule(statsModuleData);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitName(IModuleItem player)
        {
            if (!player.TryGetDataModule<NameModuleData>(out var data))
            {
                if (player.TryGetConfigModule<NameModuleConfig>(out var config))
                {
                    data = new NameModuleData()
                    {
                        Name = config.Name,
                    };
                    
                    player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitExperience(IModuleItem player)
        {
            if (!player.TryGetDataModule<ExperienceModuleData>(out var data))
            {
                if (player.TryGetConfigModule<ExperienceModuleConfig>(out var config))
                {
                    data = new ExperienceModuleData();
                    
                    player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitLevel(IModuleItem player)
        {
            if (!player.TryGetDataModule<LevelModuleData>(out var data))
            {
                if (player.TryGetConfigModule<LevelModuleConfig>(out var config))
                {
                    data = new LevelModuleData()
                    {
                        Level = config.StartLevel,
                    };
                    
                    player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }

        private void InitRace(IModuleItem player)
        {
            if (!player.TryGetDataModule<RaceModuleData>(out var data))
            {
                if (player.TryGetConfigModule<RaceModuleConfig>(out var config))
                {
                    data = new RaceModuleData()
                    {
                        Race = config.Race,
                    };
                    
                    player.AddDataModule(data);
                }
                else
                {
                    HLogger.LogError("StatsModuleConfig not found.");   
                }
            }
        }
    }
}