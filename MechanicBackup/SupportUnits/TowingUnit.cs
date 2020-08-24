using Rage;
using System.Drawing;

namespace MechanicBackup.SupportUnits
{
    class TowingUnit
    {
        public static void spawn(Player player)
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
    }
}
