interface INavigable
{
    (double lat, double lon)? CurrentWaypoint { get; }
    void SetWaypoint(double lat, double lon);
}