using Rage;
using System;
using System.Media;

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
                        spawnMechanic(player);
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.Insert))
                    {
                        spawnTowUnit(player);
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.NumPad5))
                    {
                        testing(player);
                    }
                }
            });
        }

        public static void spawnMechanic(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;

            Ped driver = new Ped("s_m_y_xmech_01", player.Character.GetOffsetPositionFront(-55f), player.Character.Heading);
            Vehicle mechVehicle = new Vehicle("mesa3", player.Character.GetOffsetPositionFront(-50f), player.Character.Heading);

            driver.BlockPermanentEvents = true;
            driver.IsPersistent = true;
            driver.IsInvincible = true;
            mechVehicle.IsPersistent = true;

            driver.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);
            driver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Normal).WaitForCompletion(60000);
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

            driver.Tasks.EnterVehicle(mechVehicle, -1, EnterVehicleFlags.None).WaitForCompletion(30000);
            driver.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.FollowTraffic);

            driver.IsPersistent = false;
            mechVehicle.IsPersistent = false;

        }

        public static void spawnTowUnit(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;

            Ped driverMech = new Ped("s_m_y_xmech_01", playerVehicle.GetOffsetPositionFront(-55f), playerVehicle.Heading);
            Vehicle mechVehicle = new Vehicle("mesa3", playerVehicle.GetOffsetPositionFront(-50f), playerVehicle.Heading);

            Ped driverTow = new Ped("s_m_y_xmech_01", playerVehicle.GetOffsetPositionFront(-65f), playerVehicle.Heading);
            Vehicle towVehicle = new Vehicle("towtruck2", playerVehicle.GetOffsetPositionFront(-60f), playerVehicle.Heading);

            driverMech.BlockPermanentEvents = true;
            driverMech.IsPersistent = true;
            driverMech.IsInvincible = true;
            mechVehicle.IsPersistent = true;
            
            driverTow.BlockPermanentEvents = true;
            driverTow.IsPersistent = true;
            driverTow.IsInvincible = true;
            towVehicle.IsPersistent = true;

            driverMech.Tasks.EnterVehicle(mechVehicle, 10000, -1, EnterVehicleFlags.None);
            driverTow.Tasks.EnterVehicle(towVehicle, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);

            
            var task1 = driverTow.Tasks.DriveToPosition(playerVehicle.Position, 15f, VehicleDrivingFlags.Normal);
            var task2 = driverMech.Tasks.DriveToPosition(playerVehicle.Position, 15f, VehicleDrivingFlags.Normal);

            while ((task1 != null && task1.IsActive) || (task2 != null && task2.IsActive))
            {
                GameFiber.Yield();
            }

            towVehicle.IsEngineOn = false;
            driverTow.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);
            driverTow.Tasks.EnterVehicle(mechVehicle, 10000, 0, 2, EnterVehicleFlags.None).WaitForCompletion(60000);

            driverMech.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.FollowTraffic);
            driverTow.Tasks.CruiseWithVehicle(mechVehicle, 15f, VehicleDrivingFlags.FollowTraffic);
            
            driverTow.Tasks.Clear();
            
            driverMech.IsPersistent = false;
            mechVehicle.IsPersistent = false;

            driverTow.IsPersistent = false;

        }
        public static void testing(Player player)
        {

            Vehicle playerVehicle = player.Character.CurrentVehicle != null ? player.Character.CurrentVehicle : player.Character.LastVehicle;

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

        }
    }
}
