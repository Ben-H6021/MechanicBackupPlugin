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

        public static readonly string SpawnVehicleNameTowingDriver = INIFile.ReadString("TowingUnit", "DriverVehicleModel", "mesa3");
        public static readonly string SpawnVehicleNameTow = INIFile.ReadString("TowingUnit", "TowTruckModel", "towtruck2");

        public static readonly string SpawnVehicleNamePickup = INIFile.ReadString("PickupUnit", "VehicleModel", "police");

        public static readonly string SpawnVehicleNameDutyDriver = INIFile.ReadString("DutyVehicleUnit", "DriverVehicleModel", "police");
        public static readonly string SpawnVehicleNameDuty = INIFile.ReadString("DutyVehicleUnit", "DutyVehicleModel", "police2");

    }
}
