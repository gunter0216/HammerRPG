using System.Collections.Generic;
using App.Generation.DungeonGenerator.External.Dto;
using App.Generation.DungeonGenerator.External.Dto.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Config;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator.Config;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator.Config;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Config;

namespace App.Generation.DungeonGenerator.External
{
    public class DungeonGenerationDtoToConfigConverter
    {
        public DungeonGenerationConfig Convert(DungeonGenerationConfigDto generationDto)
        {
            var generationConfigs = new List<IGenerationConfig>();
            generationConfigs.Add(CreateSelectBorderingRoomsGenerationConfig(generationDto.BorderingRooms));
            generationConfigs.Add(CreateRoomsGenerationConfig(generationDto.RoomsGeneration));
            generationConfigs.Add(CreateSeparateRoomsGenerationConfig(generationDto.SeparationConfig));
            generationConfigs.Add(CreateSelectSmallRoomsGenerationConfig(generationDto.SmallRooms));
            
            var generationConfig = new DungeonGenerationConfig(generationConfigs);
            return generationConfig;
        }

        private SelectBorderingRoomsGenerationConfig CreateSelectBorderingRoomsGenerationConfig(
            SelectBorderingRoomsGenerationConfigDto dto)
        {
            return new SelectBorderingRoomsGenerationConfig(
                minCorridorSize: dto.MinCorridorSize);
        }
        
        private CreateRoomsGenerationConfig CreateRoomsGenerationConfig(
            CreateRoomsGenerationConfigDto dto)
        {
            return new CreateRoomsGenerationConfig(
                countRooms: dto.CountRooms,
                minHeightRoom: dto.MinRoomSize.Height,
                minWidthRoom: dto.MinRoomSize.Width,
                maxWidthRoom: dto.MaxRoomSize.Width,
                maxHeightRoom: dto.MaxRoomSize.Height,
                radius: dto.Radius);
        }
        
        private SeparateRoomsGenerationConfig CreateSeparateRoomsGenerationConfig(
            SeparateRoomsGenerationConfigDto dto)
        {
            return new SeparateRoomsGenerationConfig(speed: dto.Speed);
        }
        
        private SelectSmallRoomsGenerationConfig CreateSelectSmallRoomsGenerationConfig(
            SelectSmallRoomsGenerationConfigDto dto)
        {
            return new SelectSmallRoomsGenerationConfig(
                heightRoomThreshold: dto.RoomThreshold.Height,
                widthRoomThreshold: dto.RoomThreshold.Width);
        }
    }
}