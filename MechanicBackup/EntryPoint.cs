using MechanicBackup;
using Rage;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("´MechanicBackupPlugin", Description = "Dispatches a mechanic or towing unit to fix your current vehicle.", Author = "craftycram")]
namespace GarageVehicleSave
{
    public static class EntryPoint
    {
        public static void Main()
        {
            GameFiber.StartNew(delegate
            {
                //Game.DisplayHelp("Hello World!");

                Game.DisplayNotification("MechanicBackupPlugin loaded successfully");
                Game.LogTrivial("MechanicBackupPlugin loaded successfully");

                Player player = Game.LocalPlayer;



                while(true)
                {
                    GameFiber.Yield();

                    if(Game.IsKeyDown(System.Windows.Forms.Keys.Delete))
                    {
                        GameFiber.StartNew(delegate
                        {
                            GameFiber.Yield();
                            spawnMechanic(player);
                        });
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.Insert))
                    {
                        GameFiber.StartNew(delegate
                        {
                            GameFiber.Yield();
                            spawnTowUnit(player);
                        });
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.NumPad5))
                    {
                        GameFiber.StartNew(delegate
                        {
                            GameFiber.Yield();
                            testing(player);
                        });
                    }
                }
            });
        }

        public static void spawnMechanic(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;

            int locationIndex = Workshop.getNearestWorkshopIndex(playerVehicle.Position);
            Vector3 spawnLocation = Workshop.spawnLocations[locationIndex];
            spawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(100f));
            float spawnHeading = Workshop.spawnHeadings[locationIndex];

            Vehicle mechVehicle = new Vehicle("mesa3", spawnLocation, spawnHeading);
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
            driver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency).WaitForCompletion(60000);
            driver.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);
            driver.Tasks.GoStraightToPosition(playerVehicle.FrontPosition, 15f, playerVehicle.Heading + 180f, -1, 30000).WaitForCompletion(-1);

            playerVehicle.IsEngineOn = false;
            playerVehicle.FuelTankHealth = 0f;

            driver.Inventory.GiveNewWeapon("weapon_wrench", -1, true);
            if (playerVehicle.HasBone("bonnet"))
            {
                if (playerVehicle.Doors[4].IsValid())
                {
                    playerVehicle.Doors[4].Open(false);
                }
            }

            driver.Tasks.PlayAnimation("mini@repair", "fixing_a_ped", -1, AnimationFlags.Loop).WaitForCompletion(5000);
            //driver.Tasks.PlayAnimation("mp_weapons_deal_sting", "crackhead_bag_loop", -1, AnimationFlags.Loop).WaitForCompletion(5000);
            playerVehicle.Repair();
            playerVehicle.IsEngineOn = false;
            playerVehicle.IsEngineStarting = true;
            if (playerVehicle.HasBone("bonnet"))
            {
                if(playerVehicle.Doors[4].IsValid())
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

        public static void spawnTowUnit(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;

            int locationIndex = Workshop.getNearestWorkshopIndex(playerVehicle.Position);
            Vector3 spawnLocation = Workshop.spawnLocations[locationIndex];
            spawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(100f));
            float spawnHeading = Workshop.spawnHeadings[locationIndex];

            Vehicle mechVehicle = new Vehicle("mesa3", spawnLocation, spawnHeading);
            Ped driverMech = new Ped("s_m_y_xmech_01", mechVehicle.GetOffsetPositionRight(-mechVehicle.Width - 1f), spawnHeading);
            Blip vehicleBlipMech = new Blip(mechVehicle);

            Vehicle towVehicle = new Vehicle("towtruck2", mechVehicle.GetOffsetPositionFront(-mechVehicle.Length - 1f), spawnHeading);
            Ped driverTow = new Ped("s_m_y_xmech_01", towVehicle.GetOffsetPositionRight(-mechVehicle.Width - 1f), spawnHeading);
            Blip vehicleBlipTow = new Blip(towVehicle);

            driverMech.BlockPermanentEvents = true;
            driverMech.IsPersistent = true;
            driverMech.IsInvincible = true;
            mechVehicle.IsPersistent = true;
            vehicleBlipMech.Color = Color.Gray;
            vehicleBlipMech.IsFriendly = true;

            driverTow.BlockPermanentEvents = true;
            driverTow.IsPersistent = true;
            driverTow.IsInvincible = true;
            towVehicle.IsPersistent = true;
            vehicleBlipTow.Color = Color.Gray;
            vehicleBlipTow.IsFriendly = true;

            Game.DisplayNotification("Dispatching tow unit");

            driverMech.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None);
            driverTow.Tasks.EnterVehicle(towVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);

            
            var task1 = driverTow.Tasks.DriveToPosition(playerVehicle.Position, 15f, VehicleDrivingFlags.Emergency);
            var task2 = driverMech.Tasks.DriveToPosition(playerVehicle.Position, 15f, VehicleDrivingFlags.Emergency);

            while ((task1 != null && task1.IsActive) || (task2 != null && task2.IsActive))
            {
                GameFiber.Yield();
            }

            towVehicle.IsEngineOn = false;
            driverTow.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);
            driverTow.Tasks.EnterVehicle(mechVehicle, 10000, 0, 2, EnterVehicleFlags.None).WaitForCompletion(60000);
            vehicleBlipTow.Delete();
            vehicleBlipMech.Delete();

            driverMech.Tasks.DriveToPosition(spawnLocation, 15f, VehicleDrivingFlags.Normal).WaitForCompletion(240000);
            driverTow.Tasks.CruiseWithVehicle(mechVehicle, 15f, VehicleDrivingFlags.FollowTraffic);
            
            driverTow.Tasks.Clear();
            
            driverMech.IsPersistent = false;
            mechVehicle.IsPersistent = false;

            driverTow.IsPersistent = false;

            return;

        }
        public static void testing(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;
            /*
            VehicleDoor[] doors = playerVehicle.GetDoors();
            foreach (VehicleDoor door in doors)
            {
                Game.LogTrivial(Convert.ToString(door.Index));
                door.Close(false);
            }
            playerVehicle.GetDoors()[2].Close(false);

            Ped driverMech = new Ped("s_m_y_xmech_01", playerVehicle.GetOffsetPositionFront(-55f), playerVehicle.Heading);
            Vehicle mechVehicle = new Vehicle("mesa3", playerVehicle.GetOffsetPositionFront(-50f), playerVehicle.Heading);


            driverMech.BlockPermanentEvents = true;
            driverMech.IsPersistent = true;
            driverMech.IsInvincible = true;
            mechVehicle.IsPersistent = true;

            driverMech.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);

            driverMech.Tasks.ParkVehicle(mechVehicle, playerVehicle.GetOffsetPositionFront(-playerVehicle.Length - 1f), playerVehicle.Heading).WaitForCompletion(30000);
            mechVehicle.IsEngineOn = false;
            driverMech.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);
            driverMech.ClearLastVehicle();


            driverMech.IsPersistent = false;
            mechVehicle.IsPersistent = false;
            */
            int index = Workshop.getNearestWorkshopIndex(playerVehicle.Position);
            Blip blip = new Blip(Workshop.spawnLocations[index]);

            return;

        }
    }
}
