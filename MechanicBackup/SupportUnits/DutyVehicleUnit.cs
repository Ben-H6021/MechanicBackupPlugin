using Rage;
using System.Drawing;

namespace MechanicBackup.SupportUnits
{
    class DutyVehicleUnit
    {
        public static void spawn(Player player)
        {

            bool persist = true;

            int locationIndex = Stations.getNearestStationIndex(player.Character.Position);
            Vector3 spawnLocation = Stations.spawnLocations[locationIndex];
            //spawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(100f));
            float spawnHeading = Stations.spawnHeadings[locationIndex];

            Vehicle vehicleDriver = new Vehicle(Config.SpawnVehicleNameDutyDriver, spawnLocation, spawnHeading);
            Ped pedDriver = new Ped("s_m_y_cop_01", vehicleDriver.GetOffsetPositionRight(-vehicleDriver.Width /*- 1f*/), spawnHeading);
            Blip blipVehicleDriver = new Blip(vehicleDriver);

            Vehicle vehicleDuty = new Vehicle(Config.SpawnVehicleNameDuty, vehicleDriver.GetOffsetPositionFront(-vehicleDriver.Length - 1f), spawnHeading);
            Ped pedDutyDriver = new Ped("s_f_y_cop_01", vehicleDuty.GetOffsetPositionRight(-vehicleDriver.Width /*- 1f*/), spawnHeading);
            Blip blipDuty = new Blip(vehicleDuty);

            pedDriver.BlockPermanentEvents = true;
            pedDriver.MakePersistent();
            pedDriver.IsInvincible = true;
            vehicleDriver.MakePersistent();
            vehicleDriver.IsSirenOn = true;
            vehicleDriver.IsSirenSilent = false;
            blipVehicleDriver.Color = Color.Gray;
            blipVehicleDriver.IsFriendly = true;

            pedDutyDriver.BlockPermanentEvents = true;
            pedDutyDriver.MakePersistent();
            pedDutyDriver.IsInvincible = true;
            vehicleDuty.MakePersistent();
            vehicleDuty.IsSirenOn = true;
            vehicleDuty.IsSirenSilent = false;
            blipDuty.Color = Color.Gray;
            blipDuty.IsFriendly = true;

            /*GameFiber.StartNew(delegate
            {
            GameFiber.Yield();
                while (persist)
                {
                    GameFiber.Yield();
                    pedDriver.MakePersistent();
                    vehicleDriver.MakePersistent();
                    pedDutyDriver.MakePersistent();
                    vehicleDuty.MakePersistent();
                }
            });*/

            Game.DisplayNotification("Dispatching duty vehicle unit");

            //pedDutyDriver.Tasks.EnterVehicle(vehicleDuty, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);
            //pedDriver.Tasks.EnterVehicle(vehicleDriver, 10000, -1, EnterVehicleFlags.None);

            pedDutyDriver.WarpIntoVehicle(vehicleDuty, -1);
            pedDriver.WarpIntoVehicle(vehicleDriver, -1);

            var task1 = pedDutyDriver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency);
            var task2 = pedDriver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency);

            Game.DisplayHelp("Hold Back to warp the vehicles closer");

            while ((task1 != null && task1.IsActive) || (task2 != null && task2.IsActive))
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
                            vehicleDriver.Position = World.GetNextPositionOnStreet(player.Character.Position.Around(50f));
                            vehicleDuty.Position = World.GetNextPositionOnStreet(player.Character.Position.Around(50f));
                            GameFiber.Sleep(1000);
                        }
                    });
                }
                if (vehicleDriver.DistanceTo(player.Character.Position) < 500f || vehicleDriver.DistanceTo(player.Character.Position) < 500f)
                {
                    vehicleDriver.IsSirenOn = true;
                    vehicleDuty.IsSirenOn = true;
                }
            }

            vehicleDuty.IsEngineOn = false;
            vehicleDuty.IsSirenOn = false;
            pedDutyDriver.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(10000);
            pedDutyDriver.Tasks.EnterVehicle(vehicleDriver, 10000, 0, 2, EnterVehicleFlags.None).WaitForCompletion(60000);
            blipDuty.Delete();
            blipVehicleDriver.Delete();
            vehicleDriver.IsSirenOn = false;

            pedDriver.Tasks.DriveToPosition(spawnLocation, 15f, VehicleDrivingFlags.Normal).WaitForCompletion(240000);
            pedDutyDriver.Tasks.CruiseWithVehicle(vehicleDriver, 15f, VehicleDrivingFlags.FollowTraffic);

            pedDutyDriver.Tasks.Clear();

            persist = false;

            pedDriver.IsPersistent = false;
            vehicleDriver.IsPersistent = false;

            pedDutyDriver.IsPersistent = false;


            return;

        }
    }
}
