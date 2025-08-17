namespace App.Game.Equipment.Runtime.Config.Converter
{
    public class EquipmentConfigConverter
    {
        public Model.EquipmentConfig Convert(Dto.EquipmentConfigDto dto)
        {
            return new Model.EquipmentConfig(dto);
        }
    }
}

