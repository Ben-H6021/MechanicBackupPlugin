using Rage;

namespace MechanicBackup
{
    public static class API
    {

        public static void spawnMechanicUnit()
        {
            SupportUnits.MechanicUnit.spawn(Game.LocalPlayer);
        }
    }
}
