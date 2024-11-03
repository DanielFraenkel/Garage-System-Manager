namespace GarageLogic
{
    public class VehicleInfo
    {
        public string LicenseNumber { get; set; }
        public string ModelName { get; set; }
        public WheelInfo WheelInfo { get; set; }
        public EnergySourceInfo EnergySourceInfo { get; set; }
    }
}
