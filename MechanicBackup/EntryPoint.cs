using Rage;
using System.Threading;
using System.Net;
using System;

[assembly: Rage.Attributes.Plugin("MechanicBackupPlugin", Description = "Dispatches a mechanic or towing unit to fix your current vehicle.", Author = "craftycram")]
namespace MechanicBackup
{
    public static class EntryPoint
    {
        public static void Main()
        {
            GameFiber.StartNew(delegate
            {

            Version currentVersion = new Version("1.1.0");
            Version newVersion = new Version();

            Game.DisplayNotification("MechanicBackupPlugin " + currentVersion + " loaded successfully");
            Game.LogTrivial("MechanicBackupPlugin " + currentVersion + " loaded successfully");

                try
            {
                Thread FetchVersionThread = new Thread(() =>
                {

                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            string s = client.DownloadString("http://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=29789&textOnly=1");                              // <= CHANGE FILE ID!!!!

                            newVersion = new Version(s);
                        }
                        catch (Exception e) { Game.LogTrivial("MechanicBackupPlugin: LSPDFR Update API down. Aborting checks."); }
                    }
                });
                FetchVersionThread.Start();
                while (FetchVersionThread.ThreadState != System.Threading.ThreadState.Stopped)
                {
                    GameFiber.Yield();
                }

                // compare the versions  
                if (currentVersion.CompareTo(newVersion) < 0)
                {
                    Game.LogTrivial("MechanicBackupPlugin: Update Available for MechanicBackupPlugin. Installed Version " + currentVersion + "New Version " + newVersion);
                    Game.DisplayNotification("~g~Update Available~w~ for ~b~MechanicBackupPlugin~w~.\nInstalled Version: ~y~" + currentVersion + "\n~w~New Version~y~ " + newVersion);
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Game.LogTrivial("MechanicBackupPlugin: Error while checking for updates.");
            }
            catch (Exception e)
            {
                Game.LogTrivial("MechanicBackupPlugin: Error while checking for updates.");
            }

                Player player = Game.LocalPlayer;
            Menu.createMenu();



                while(true)
                {
                    GameFiber.Yield();
                    if (Game.IsKeyDown(Config.MenuKey))
                    {
                        Menu.mainMenu.Visible = !Menu.mainMenu.Visible;
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.NumPad5) && Config.Debug)
                    {
                        GameFiber.StartNew(delegate
                        {
                            GameFiber.Yield();
                            testing(player);
                        });
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
