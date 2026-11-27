namespace App.Game.Equipment.Runtime.Constants
{
    public class EquipmentTypeConstants
    {
        public const string Helmet = "Helmet";
        public const string Armor = "Armor";
        public const string Shoulder = "Shoulder";
        public const string Gloves = "Gloves";
        public const string Amulet = "Amulet";
        public const string Bracer = "Bracer";
        public const string Ring = "Ring";
        public const string Belt = "Belt";
        public const string Pants = "Pants";
        public const string Boots = "Boots";
        public const string OneHanded = "One-handed";
        public const string TwoHanded = "Two-handed";
        public const string Shield = "Shield";

        public static bool TryGetType(string name, out EquipmentType type)
        {
            EquipmentType? match = name switch
            {
                Helmet => EquipmentType.Helmet,
                Armor => EquipmentType.Armor,
                Shoulder => EquipmentType.Shoulder,
                Gloves => EquipmentType.Gloves,
                Amulet => EquipmentType.Amulet,
                Bracer => EquipmentType.Bracer,
                Ring => EquipmentType.Ring,
                Belt => EquipmentType.Belt,
                Pants => EquipmentType.Pants,
                Boots => EquipmentType.Boots,
                OneHanded => EquipmentType.OneHanded,
                TwoHanded => EquipmentType.TwoHanded,
                Shield => EquipmentType.Shield,
                _ => null
            };
            type = match.GetValueOrDefault();
            return match.HasValue;
        }
    }
}
