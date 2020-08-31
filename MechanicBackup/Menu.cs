

using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

namespace MechanicBackup
{
    class Menu
    {
        public static UIMenu mainMenu;
        public static MenuPool _menuPool;

        private static UIMenuItem spawnMechanicUnit;
        private static UIMenuItem spawnTowingUnit;
        private static UIMenuItem spawnPickupUnit;
        private static UIMenuItem spawnDutyVehicleUnit;

        public static void createMenu()
        {
            _menuPool = new MenuPool();
            mainMenu = new UIMenu("MechanicBackup", "Select a unit to spawn");
            _menuPool.Add(mainMenu);

            mainMenu.MouseControlsEnabled = false;
            mainMenu.AllowCameraMovement = true;

            mainMenu.AddItem(spawnMechanicUnit = new UIMenuItem("Mechanic Unit"));
            mainMenu.AddItem(spawnTowingUnit = new UIMenuItem("Towing Unit"));
            mainMenu.AddItem(spawnPickupUnit = new UIMenuItem("Pickup Unit"));
            mainMenu.AddItem(spawnDutyVehicleUnit = new UIMenuItem("DutyVehicle Unit"));

            mainMenu.RefreshIndex();
            mainMenu.OnItemSelect += OnItemSelect;
        }

        public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (sender == mainMenu)
            {
                if(selectedItem == spawnMechanicUnit)
                {
                    GameFiber.StartNew(delegate
                    {
                        GameFiber.Yield();
                        sender.Close();
                        SupportUnits.MechanicUnit.spawn(Game.LocalPlayer);
                    });
                }
                if (selectedItem == spawnTowingUnit)
                {
                    GameFiber.StartNew(delegate
                    {
                        GameFiber.Yield();
                        sender.Close();
                        SupportUnits.TowingUnit.spawn(Game.LocalPlayer);
                    });
                }
                if (selectedItem == spawnPickupUnit)
                {
                    GameFiber.StartNew(delegate
                    {
                        GameFiber.Yield();
                        sender.Close();
                        SupportUnits.PickupUnit.spawn(Game.LocalPlayer);
                    });
                }
                if (selectedItem == spawnDutyVehicleUnit)
                {
                    GameFiber.StartNew(delegate
                    {
                        GameFiber.Yield();
                        sender.Close();
                        SupportUnits.DutyVehicleUnit.spawn(Game.LocalPlayer);
                    });
                }
            }
        }
        /*public static void MainLogic()
        {
            createMenu();
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.Insert))
                    {
                        mainMenu.Visible = !mainMenu.Visible;
                    }

                    _menuPool.ProcessMenus();
                }
            });
        }*/
    }
}
