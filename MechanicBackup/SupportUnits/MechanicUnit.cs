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
            //spawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(100f));
            float spawnHeading = Workshop.spawnHeadings[locationIndex];

            Vehicle mechVehicle = new Vehicle(Config.SpawnVehicleNameMechanic, spawnLocation, spawnHeading);
            Ped driver = new Ped("s_m_y_xmech_01", mechVehicle.GetOffsetPositionRight(-mechVehicle.Width /*- 1f*/), spawnHeading);
            Blip vehicleBlip = new Blip(mechVehicle);

            driver.BlockPermanentEvents = true;
            driver.IsPersistent = true;
            driver.IsInvincible = true;
            mechVehicle.IsPersistent = true;
            vehicleBlip.Color = Color.Gray;
            vehicleBlip.IsFriendly = true;

            Game.DisplayNotification("Dispatching mechanic unit");

            //driver.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);

            driver.WarpIntoVehicle(mechVehicle, -1);
            
            var task1 = driver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency);
            Game.DisplayHelp("Hold Back to warp the vehicles closer");

            while ((task1 != null && task1.IsActive))
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(System.Windows.Forms.Keys.Back))
                {
                    GameFiber.StartNew(delegate
                    {
                        GameFiber.Sleep(2000);
                        if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Back))
                        {
                            Game.DisplayHelp("Warping closer");
                            mechVehicle.SetPositionWithSnap(playerVehicle.GetOffsetPositionFront(-playerVehicle.Length - 1f));
                            GameFiber.Sleep(1000);
                        }
                    });
                }
                if (mechVehicle.DistanceTo(player.Character.Position) < 500f)
                {
                    mechVehicle.IsSirenOn = true;
                    mechVehicle.IsSirenOn = true;
                }
            }
            
            driver.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);

            playerVehicle.IsEngineOn = false;
            playerVehicle.FuelTankHealth = 0f;

            driver.PlayAmbientSpeech("generic_hi");

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

            player.Character.PlayAmbientSpeech("generic_thanks");

            driver.Tasks.EnterVehicle(mechVehicle, -1, EnterVehicleFlags.None).WaitForCompletion(60000);
            vehicleBlip.Delete();
            driver.Tasks.DriveToPosition(spawnLocation, 15f, VehicleDrivingFlags.Normal).WaitForCompletion(240000);

            driver.IsPersistent = false;
            mechVehicle.IsPersistent = false;

            return;

        }
    }
}
