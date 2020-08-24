using Rage;

namespace MechanicBackup
{
    internal static class Config
    {
        public static readonly InitializationFile INIFile = new InitializationFile(@"Plugins\MechanicBackup.ini");

        public static readonly bool Debug = INIFile.ReadBoolean("General", "Debug", false);
        public static readonly bool Repair = INIFile.ReadBoolean("General", "Repair", true);

        public static readonly bool SpawnVehicleLeftOnLoad = INIFile.ReadBoolean("Spawning", "SpawnVehicleLeftOnLoad", false);
        public static readonly bool SpawnVehicleRightOnLoad = INIFile.ReadBoolean("Spawning", "SpawnVehicleRightOnLoad", false);

        public static readonly string SpawnVehicleNameLeft = INIFile.ReadString("Spawning", "SpawnVehicleNameLeft", "");
        public static readonly string SpawnVehicleNameRight = INIFile.ReadString("Spawning", "SpawnVehicleNameRight", "");

        public static readonly bool SpawnVehicleParkingLotOnLoad = INIFile.ReadBoolean("Spawning", "SpawnVehicleParkingLotOnLoad", false);
        public static readonly string SpawnVehicleNameParkingLot = INIFile.ReadString("Spawning", "SpawnVehicleNameParkingLot", "");

    }
}
