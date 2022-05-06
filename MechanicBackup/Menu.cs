
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;

namespace MechanicBackup
{
    class Menu
    {
        public static UIMenu mainMenu;
        public static MenuPool _menuPool;

        private static UIMenuItem spawnMechanicUnit;
        private static readonly Random rand = new Random();

        public static void createMenu()
        {
            _menuPool = new MenuPool();
            mainMenu = new UIMenu("MechanicBackup", "Select a unit to Deply");
            _menuPool.Add(mainMenu);

            mainMenu.MouseControlsEnabled = false;
            mainMenu.AllowCameraMovement = true;

            mainMenu.AddItem(spawnMechanicUnit = new UIMenuItem("Mechanic Unit"));


            mainMenu.RefreshIndex();
            mainMenu.OnItemSelect += OnItemSelect;
        }

        public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (sender == mainMenu)
            {
                if (selectedItem == spawnMechanicUnit)
                {
                    GameFiber.StartNew(delegate
                    {
                        GameFiber.Yield();
                        sender.Close();
                        if (rand.Next(1, 3) == 1)
                        {
                            SupportUnits.MechanicUnit.spawn(Game.LocalPlayer);
                        }
                        else SupportUnits.MechanicUnit.spawn2(Game.LocalPlayer);
                    });
                }
            }
        }
    }
}
