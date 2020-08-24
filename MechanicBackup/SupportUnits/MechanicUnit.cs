using Rage;
using System.Drawing;

namespace MechanicBackup.SupportUnits
{
    class MechanicUnit
    {
        public static void spawn(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;

            int locationIndex = Workshop.getNearestWorkshopIndex(playerVehicle.Position);
            Vector3 spawnLocation = Workshop.spawnLocations[locationIndex];
            spawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(100f));
            float spawnHeading = Workshop.spawnHeadings[locationIndex];

            Vehicle mechVehicle = new Vehicle(Config.SpawnVehicleNameMechanic, spawnLocation, spawnHeading);
            Ped driver = new Ped("s_m_y_xmech_01", mechVehicle.GetOffsetPositionRight(-mechVehicle.Width - 1f), spawnHeading);
            Blip vehicleBlip = new Blip(mechVehicle);

            driver.BlockPermanentEvents = true;
            driver.IsPersistent = true;
            driver.IsInvincible = true;
            mechVehicle.IsPersistent = true;
            vehicleBlip.Color = Color.Gray;
            vehicleBlip.IsFriendly = true;

            Game.DisplayNotification("Dispatching mechanic unit");

            driver.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);
            var task1 = driver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency);
            task1.WaitForCompletion(60000);
            if (task1.IsActive)
            {
                mechVehicle.SetPositionWithSnap(playerVehicle.GetOffsetPositionFront(-playerVehicle.Length - 1f));
            }
            driver.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);

            playerVehicle.IsEngineOn = false;
            playerVehicle.FuelTankHealth = 0f;

            driver.Tasks.GoStraightToPosition(playerVehicle.FrontPosition, 15f, playerVehicle.Heading + 180f, -1, 30000).WaitForCompletion(-1);

            driver.Inventory.GiveNewWeapon("weapon_wrench", -1, true);
            if (playerVehicle.HasBone("bonnet"))
            {
                if (playerVehicle.Doors[4].IsValid())
                {
                    playerVehicle.Doors[4].Open(false);
                }
            }

            driver.Tasks.PlayAnimation("mini@repair", "fixing_a_ped", -1, AnimationFlags.Loop).WaitForCompletion(5000);
            playerVehicle.Repair();
            playerVehicle.IsEngineOn = false;
            playerVehicle.IsEngineStarting = true;
            if (playerVehicle.HasBone("bonnet"))
            {
                if (playerVehicle.Doors[4].IsValid())
                {
                    playerVehicle.Doors[4].Close(false);
                }
            }
            //driver.Inventory.EquippedWeapon = null;

            driver.Tasks.EnterVehicle(mechVehicle, -1, EnterVehicleFlags.None).WaitForCompletion(60000);
            vehicleBlip.Delete();
            driver.Tasks.DriveToPosition(spawnLocation, 15f, VehicleDrivingFlags.Normal).WaitForCompletion(240000);

            driver.IsPersistent = false;
            mechVehicle.IsPersistent = false;

            return;

        }
    }
}
