public abstract class Drone : ISelfTest, IFlightControl
{
    public int Id { get; private set; } = 0;
    public string Name { get; private set; } = string.Empty;
    public int BatteryPercent { get; set; } = 0;
    public int BatteryTakeOffLimit { get; private set; } = 20;
    public bool IsAirborne { get; set; } = false;
    public abstract bool RunSelfTest();
    public abstract void TakeOff();
    public abstract void Land();

}
