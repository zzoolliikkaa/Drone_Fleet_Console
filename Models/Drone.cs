public abstract class Drone : ISelfTest, IFlightControl
{
    public Guid Id { get; private set; } = Guid.Empty;
    public string Name { get; private set; } = string.Empty;
    public abstract DroneTypeEnum Type { get; }
    public int BatteryPercent { get; set; } = 0;
    public int BatteryTakeOffLimit { get; private set; } = 20;
    public bool IsAirborne { get; set; } = false;
    public abstract bool RunSelfTest();
    public abstract void TakeOff();
    public abstract void Land();
    public void Initialize(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public bool ChargeBattery(int DesiredBatteryPercent)
    {
        if (BatteryPercent < DesiredBatteryPercent)
        {
            BatteryPercent = DesiredBatteryPercent;
            Console.WriteLine($"The {Name} drone (ID {Id}) battery charging . . .");
            Console.WriteLine($"The {Name} drone (ID {Id}) battery level is {BatteryPercent}.");
            return true;
        }
        else
        {
            Console.WriteLine($"The {Name} drone (ID {Id}) battery level is full.");
            return false;
        }
    }
}
