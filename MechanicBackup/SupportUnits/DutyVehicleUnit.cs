using Rage;
using System.Drawing;

namespace MechanicBackup.SupportUnits
{
    class DutyVehicleUnit
    {
        public static void spawn(Player player)
        {

            int locationIndex = Workshop.getNearestWorkshopIndex(player.Character.Position);
            Vector3 spawnLocation = Workshop.spawnLocations[locationIndex];
            spawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(100f));
            float spawnHeading = Workshop.spawnHeadings[locationIndex];

            Vehicle vehicleDriver = new Vehicle(Config.SpawnVehicleNameDutyDriver, spawnLocation, spawnHeading);
            Ped pedDriver = new Ped("s_m_y_cop_01", vehicleDriver.GetOffsetPositionRight(-vehicleDriver.Width - 1f), spawnHeading);
            Blip blipVehicleDriver = new Blip(vehicleDriver);

            Vehicle vehicleDuty = new Vehicle(Config.SpawnVehicleNameDuty, vehicleDriver.GetOffsetPositionFront(-vehicleDriver.Length - 1f), spawnHeading);
            Ped pedDutyDriver = new Ped("s_f_y_cop_01", vehicleDuty.GetOffsetPositionRight(-vehicleDriver.Width - 1f), spawnHeading);
            Blip blipDuty = new Blip(vehicleDuty);

            pedDriver.BlockPermanentEvents = true;
            pedDriver.IsPersistent = true;
            pedDriver.IsInvincible = true;
            vehicleDriver.IsPersistent = true;
            vehicleDriver.IsSirenOn = true;
            vehicleDriver.IsSirenSilent = false;
            blipVehicleDriver.Color = Color.Gray;
            blipVehicleDriver.IsFriendly = true;

            pedDutyDriver.BlockPermanentEvents = true;
            pedDutyDriver.IsPersistent = true;
            pedDutyDriver.IsInvincible = true;
            vehicleDuty.IsPersistent = true;
            vehicleDuty.IsSirenOn = true;
            vehicleDuty.IsSirenSilent = false;
            blipDuty.Color = Color.Gray;
            blipDuty.IsFriendly = true;

            Game.DisplayNotification("Dispatching duty vehicle unit");

            pedDriver.Tasks.EnterVehicle(vehicleDriver, 10000, -1, EnterVehicleFlags.None);
            pedDutyDriver.Tasks.EnterVehicle(vehicleDuty, 10000, -1, EnterVehicleFlags.None).WaitForCompletion(30000);


            var task1 = pedDutyDriver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency);
            var task2 = pedDriver.Tasks.DriveToPosition(player.Character.Position, 15f, VehicleDrivingFlags.Emergency);

            while ((task1 != null && task1.IsActive) || (task2 != null && task2.IsActive))
            {
                GameFiber.Yield();
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

            pedDriver.IsPersistent = false;
            vehicleDriver.IsPersistent = false;

            pedDutyDriver.IsPersistent = false;


            return;

        }
    }
}
