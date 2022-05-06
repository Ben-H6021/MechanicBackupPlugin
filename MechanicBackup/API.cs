using Rage;

namespace MechanicBackup
{
    public static class API
    {

        public static void spawnMechanicUnit1()
        {
            SupportUnits.MechanicUnit.spawn(Game.LocalPlayer);
        }
        public static void spawnMechanicUnit2()
        {
            SupportUnits.MechanicUnit.spawn2(Game.LocalPlayer);
        }
    }
}
