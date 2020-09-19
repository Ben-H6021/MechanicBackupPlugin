using Rage;

namespace MechanicBackup
{
    public static class API
    {

        public static void spawnMechanicUnit()
        {
            SupportUnits.MechanicUnit.spawn(Game.LocalPlayer);
        }
        public static void spawnTowingUnit()
        {
            SupportUnits.TowingUnit.spawn(Game.LocalPlayer);
        }
        public static void spawnPickupUnit()
        {
            SupportUnits.PickupUnit.spawn(Game.LocalPlayer);
        }
        public static void spawnDutyVehicleUnit()
        {
            SupportUnits.DutyVehicleUnit.spawn(Game.LocalPlayer);
        }

    }
}
