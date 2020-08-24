using Rage;

[assembly: Rage.Attributes.Plugin("´MechanicBackupPlugin", Description = "Dispatches a mechanic or towing unit to fix your current vehicle.", Author = "craftycram")]
namespace MechanicBackup
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
            Menu.createMenu();



                while(true)
                {
                    GameFiber.Yield();
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.NumPad5) && Config.Debug)
                    {
                        GameFiber.StartNew(delegate
                        {
                            GameFiber.Yield();
                            testing(player);
                        });
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.Insert))
                    {
                        Menu.mainMenu.Visible = !Menu.mainMenu.Visible;
                    }

                    Menu._menuPool.ProcessMenus();
                }
            });
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
