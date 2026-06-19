using Drone_Fleet_Console.Models;
using Drone_Fleet_Console;
using Drone_Fleet_Console.Models.Interfaces;

var mainMenu = new Menu();
var droneMenu = new Menu();
var droneRegistry = new DroneRegistry();
var mainChoice = 0;

mainMenu.AddCaption("=== DRONE FLEET ===");
mainMenu.AddOption("List drones");
mainMenu.AddOption("Add drone");
mainMenu.AddOption("Pre-flight check (all)");
mainMenu.AddOption("Take off");
mainMenu.AddOption("Land");
mainMenu.AddOption("Set waypoint");
mainMenu.AddOption("Capability actions");
mainMenu.AddOption("Charge battery");
mainMenu.AddOption("Exit");

droneMenu.AddCaption("ADD DRONE");
droneMenu.AddOption("Survey Drone");
droneMenu.AddOption("Delivery Drone");
droneMenu.AddOption("Racing Drone");
droneMenu.AddOption("Back to main menu");

while (MainMenuOptionsEnum.Exit != (MainMenuOptionsEnum)mainChoice)
{
    mainMenu.DisplayMenu();
    mainChoice = mainMenu.ReadChoice();
    MainMenuOptionsEnum mainMenuSelected = (MainMenuOptionsEnum)mainChoice;
    switch (mainMenuSelected)
    {
        case MainMenuOptionsEnum.ListDrone:
            droneRegistry.ListDrone();
            break;

        case MainMenuOptionsEnum.AddDrone:
            droneRegistry.AddDrone(droneMenu);
            break;

        case MainMenuOptionsEnum.PreFlightCheck:
            droneRegistry.PreFlightCheck();
            break;

        case MainMenuOptionsEnum.TakeOff:
            droneRegistry.TakeOff();
            break;

        case MainMenuOptionsEnum.Land:
            droneRegistry.Land();
            break;

        case MainMenuOptionsEnum.SetWaypoint:
            droneRegistry.SetWaypoint();
            break;

        case MainMenuOptionsEnum.CapabilityActions:
            droneRegistry.CapabilityActions();
            break;

        case MainMenuOptionsEnum.ChargeBattery:
            droneRegistry.ChargeBattery();
            break;

        case MainMenuOptionsEnum.Exit:
            Console.WriteLine("Exiting...");
            break;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            Console.ReadLine();
            break;
    }
}

public class DroneRegistry
{
    private List<Drone> _droneList = new List<Drone>();
    public void ListDrone()
    {
        if (!EmptyList())
        {
            Console.WriteLine("Listing drones...");
            foreach (var drone in _droneList)
            {
                Console.WriteLine($"ID: {drone.Id}, Name: {drone.Name}, Type: {drone.Type}, Battery: {drone.BatteryPercent}, Is Airborne:{drone.IsAirborne}");
                if (drone is IPhotoCapture photoCapture)
                {
                    Console.WriteLine($"                                          Photo storage: {photoCapture.PhotoCount} ");
                }
                if (drone is INavigable navigable)
                {
                    Console.WriteLine($"                                          Waypoint: {navigable.CurrentWaypoint} ");
                }
                if (drone is ICargoCarrier cargoCarrier)
                {
                    Console.WriteLine($"                                          Current Load (kg): {cargoCarrier.CurrentLoadKg} ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }

    public void AddDrone(Menu droneMenu)
    {
        int droneChoice = 0;
        while (DroneMenuOptionsEnum.BackToMainMenu != (DroneMenuOptionsEnum)droneChoice)
        {
            droneMenu.DisplayMenu();
            droneChoice = droneMenu.ReadChoice();
            DroneMenuOptionsEnum droneMenuSelected = (DroneMenuOptionsEnum)droneChoice;
            switch (droneMenuSelected)
            {
                case DroneMenuOptionsEnum.SurveyDrone:
                    AddNewdrone(new SurveyDrone());
                    break;

                case DroneMenuOptionsEnum.DeliveryDrone:
                    AddNewdrone(new DeliveryDrone());
                    break;

                case DroneMenuOptionsEnum.RacingDrone:
                    AddNewdrone(new RacingDrone());
                    break;

                case DroneMenuOptionsEnum.BackToMainMenu:
                    Console.WriteLine("Returning to main menu...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void AddNewdrone(Drone drone)
    {
        drone.Initialize(Guid.NewGuid(), ReadDroneName());
        _droneList.Add(drone);
    }

    private string ReadDroneName()
    {
        string? name;

        do
        {
            Console.Write("Enter the drone's name: ");
            name = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(name));

        return name;
    }

    internal void PreFlightCheck()
    {
        if (!EmptyList())
        {
            foreach (var drone in _droneList)
            {
                drone.RunSelfTest();
            }
            Console.ReadLine();
        }
    }

    internal void TakeOff()
    {
        if (!EmptyList())
        {
            if (SelectDrone(out Drone? drone))
            {
                Console.WriteLine("Drone TakeOff:");
                drone.TakeOff();
            }
            Console.ReadLine();
        }
    }

    internal void Land()
    {
        if (!EmptyList())
        {
            if (SelectDrone(out Drone? drone))
            {
                Console.WriteLine("Drone Landing:");
                drone.Land();
            }
            Console.ReadLine();
        }
    }

    internal void SetWaypoint()
    {
        if (!EmptyList())
        {
            if (SelectDrone(out Drone? drone))
            {
                if (drone is INavigable navigable)
                {
                    Console.Write("Enter the latitide of WayPoint [-90 , 90]:");
                    var lat = int.TryParse(Console.ReadLine(), out int latitude);
                    if ((!lat) || (latitude < -90) || (latitude > 90))
                    {
                        Console.WriteLine("Invalid range.");
                    }
                    else
                    {
                        Console.Write("Enter the longitude of WayPoint [-180 , 180]:");
                        var lon = int.TryParse(Console.ReadLine(), out int longitude);
                        if ((!lon) || (longitude < -180) || (longitude > 180))
                        {
                            Console.WriteLine("Invalid range.");
                        }
                        else
                        {
                            if ((lat) && (lon))
                            {
                                navigable.SetWaypoint(latitude, longitude);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The drone is not navigable.");
                }
            }
            Console.ReadLine();
        }
    }

    internal void CapabilityActions()
    {
        if (!EmptyList())
        {
            if (SelectDrone(out Drone? drone))
            {
                if (drone is IPhotoCapture photoCapture)
                {
                    photoCapture.TakePhoto();
                }
                else
                if (drone is ICargoCarrier cargoCarrier)
                {
                    Console.Write("Enter the load height in kg:");
                    if (int.TryParse(Console.ReadLine(), out int kg) && (kg > 0))
                    {
                        cargoCarrier.Load(kg);
                    }
                    else
                    {
                        Console.WriteLine("Invalid load.");
                    }
                }
                if (drone.Type == DroneTypeEnum.Racing)
                {
                    Console.WriteLine("This is a racing drone.");
                }
            }
            Console.ReadLine();
        }
    }

    internal void ChargeBattery()
    {
        if (!EmptyList())
        {
            if (SelectDrone(out Drone? drone))
            {
                Console.Write("Enter desired battery level[20 , 100]:");
                var bat = int.TryParse(Console.ReadLine(), out int battery);
                if ((!bat) || (battery < drone.BatteryTakeOffLimit) || (battery > 100))
                {
                    Console.WriteLine("Invalid range.");

                }
                else
                {
                    drone.ChargeBattery(battery);
                }
            }
            Console.ReadLine();
        }
    }

    private bool EmptyList()
    {
        if (_droneList.Count == 0)
        {
            Console.WriteLine("No item found.");
            Console.ReadLine();
            return true;
        }
        return false;
    }

    private bool SelectDrone(out Drone? drone)
    {
        Console.Write("Enter the drone id:");
        if (!Guid.TryParse(Console.ReadLine(), out Guid droneId))
        {
            Console.WriteLine("Drone not found.");
            drone = null;
            return false;
        }

        drone = _droneList.FirstOrDefault(d => d.Id == droneId);

        return drone != null;
    }
}
