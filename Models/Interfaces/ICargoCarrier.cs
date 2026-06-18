interface ICargoCarrier
{
    double CapacityKg { get; }
    double CurrentLoadKg { get; }
    bool Load(double kg); void UnloadAll();
}
