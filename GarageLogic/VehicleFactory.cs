using System;

namespace GarageLogic
{
    internal static class VehicleFactory
    {
        public static Vehicle CreateVehicle(eVehicleType i_VehicleType)
        {
            switch (i_VehicleType)
            {
                case eVehicleType.FuelCar:
                    return new Car(new FuelEnergy(50f, eFuelType.Octane95));
                case eVehicleType.ElectricCar:
                    return new Car(new ElectricEnergy(3.2f));
                case eVehicleType.FuelMotorcycle:
                    return new Motorcycle(new FuelEnergy(6f, eFuelType.Octane98));
                case eVehicleType.ElectricMotorcycle:
                    return new Motorcycle(new ElectricEnergy(1.8f));
                case eVehicleType.Truck:
                    return new Truck(new FuelEnergy(120f, eFuelType.Soler));
                default:
                    throw new ArgumentException("Invalid vehicle type.");
            }
        }

        public static eVehicleType[] GetAvailableVehicleTypes()
        {
            return (eVehicleType[])Enum.GetValues(typeof(eVehicleType));
        }
    }
}
