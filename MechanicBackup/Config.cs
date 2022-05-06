using Rage;
using System.Windows.Forms;

namespace MechanicBackup
{
    internal static class Config
    {
        public static readonly InitializationFile INIFile = new InitializationFile(@"Plugins\MechanicBackup.ini");

        public static readonly bool Debug = INIFile.ReadBoolean("General", "Debug", false);
        public static readonly Keys MenuKey = INIFile.ReadEnum<Keys>("General", "MenuKey", Keys.Insert);

        public static readonly string SpawnVehicleNameMechanic = INIFile.ReadString("MechanicUnit", "VehicleModel", "mesa3");
        public static readonly string SpawnVehicleNameMechanic2 = INIFile.ReadString("MechanicUnit2", "VehicleModel", "mesa3");
    }
}
