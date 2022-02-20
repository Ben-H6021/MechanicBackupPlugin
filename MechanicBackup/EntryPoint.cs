using Rage;
using System.Threading;
using System.Net;
using System;

[assembly: Rage.Attributes.Plugin("MechanicBackupPlugin", Description = "Dispatches a mechanic to fix your current vehicle.", Author = "craftycram")]
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

                Player player = Game.LocalPlayer;
                Menu.createMenu();
            });
        }
    }
}
