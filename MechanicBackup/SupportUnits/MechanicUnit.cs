using Rage;
using System.Drawing;
using Rage.Native;


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
            Ped driver = new Ped("mp_m_freemode_01", mechVehicle.GetOffsetPositionRight(-mechVehicle.Width /*- 1f*/), spawnHeading); ;
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(driver, 3, 1, 0, 2);
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(driver, 4, 168, 0, 2);
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(driver, 8, 15, 0, 2);
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(driver, 6, 12, 6, 2);
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(driver, 11, 453, 0, 2);
            RandomCharacter.RandomizeCharacter(driver);
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(driver, 2, 10, 0, 2);
            Blip vehicleBlip = new Blip(mechVehicle);
            driver.BlockPermanentEvents = true;
            driver.IsPersistent = true;
            driver.IsInvincible = true;
            mechVehicle.IsPersistent = true;
            vehicleBlip.Color = Color.Gray;
            vehicleBlip.IsFriendly = true;

            Game.DisplayNotification("Dispatching mechanic");

            //driver.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);

            driver.WarpIntoVehicle(mechVehicle, -1);

            var task1 = driver.Tasks.DriveToPosition(playerVehicle.GetOffsetPositionFront(-playerVehicle.Length - 5f), 8f, VehicleDrivingFlags.Normal);
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
                if (mechVehicle.DistanceTo(player.Character.Position) < 25f)
                {
                    mechVehicle.IsSirenOn = true;
                    mechVehicle.IsSirenOn = true;
                }
            }
            driver.Tasks.ParkVehicle(playerVehicle.GetOffsetPositionFront(-playerVehicle.Length - 1f), playerVehicle.Heading).WaitForCompletion(10000);
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
            playerVehicle.DirtLevel = 0f;
            if (playerVehicle.HasBone("bonnet"))
            {
                if (playerVehicle.Doors[4].IsValid())
                {
                    playerVehicle.Doors[4].Close(false);
                }
            }
            player.Character.PlayAmbientSpeech("generic_thanks");
            driver.Tasks.EnterVehicle(mechVehicle, -1, EnterVehicleFlags.None).WaitForCompletion(60000);
            vehicleBlip.Delete();
            driver.Tasks.PerformDrivingManeuver(mechVehicle, VehicleManeuver.ReverseStraight).WaitForCompletion(1000);
            mechVehicle.IsSirenOn = false;
            mechVehicle.IsSirenOn = false;
            driver.Tasks.DriveToPosition(spawnLocation, 15f, VehicleDrivingFlags.Normal).WaitForCompletion(240000);
            driver.IsPersistent = false;
            mechVehicle.IsPersistent = false;
            mechVehicle.Delete();
            driver.Delete();
            return;

        }
    }
}
