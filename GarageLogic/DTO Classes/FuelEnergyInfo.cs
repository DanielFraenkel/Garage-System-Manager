namespace GarageLogic
{
    public class FuelEnergyInfo : EnergySourceInfo
    {
        public float CurrentFuelAmountLiters { get; set; }
        public float MaxFuelAmountLiters { get; set; }
        public eFuelType FuelType { get; set; }
    }
}
