using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public class Garage
    {
        private readonly Dictionary<string, VehicleRecord> r_Vehicles;

        public Garage()
        {
            r_Vehicles = new Dictionary<string, VehicleRecord>();
        }

        public bool CheckIfVehicleExists(string i_LicenseNumber)
        {
            return r_Vehicles.ContainsKey(i_LicenseNumber);
        }

        public void AddExistingVehicle(string i_LicenseNumber)
        {
            r_Vehicles[i_LicenseNumber].UpdateStatus(eVehicleStatus.InRepair);
        }

        public eVehicleType[] GetAvailableVehicleTypes()
        {
            return VehicleFactory.GetAvailableVehicleTypes();
        }

        public List<DataRequirement> CreateVehicleRecord(string i_LicenseNumber, eVehicleType i_VehicleType)
        {
            Vehicle vehicle = VehicleFactory.CreateVehicle(i_VehicleType);
            vehicle.LicenseNumber = i_LicenseNumber;
            VehicleRecord vehicleRecord = new VehicleRecord(vehicle);
            r_Vehicles.Add(i_LicenseNumber, vehicleRecord);

            return vehicleRecord.GetDataRequirements();
        }

        public void SetVehicleProperties(string i_LicenseNumber, Dictionary<string, object> i_UserInputs)
        {
            if (r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleRecord vehicleRecord))
            {
                vehicleRecord.AssignProperties(i_UserInputs);
            }
            else
            {
                throw new KeyNotFoundException("Vehicle not found in the garage.");
            }
        }

        public VehicleRecordInfo GetVehicleRecordInfo(string i_LicenseNumber)
        {
            if (!r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleRecord vehicleRecord))
            {
                throw new KeyNotFoundException("Vehicle not found in the garage.");
            }

            return vehicleRecord.CreateInfo();
        }

        public List<string> GetAllLicenseNumbers(eVehicleStatus? i_FilterByStatus = null)
        {
            List<string> licenseNumbers = new List<string>();

            foreach (var vehicleRecord in r_Vehicles.Values)
            {
                if (i_FilterByStatus == null || vehicleRecord.IsStatus(i_FilterByStatus.Value))
                {
                    licenseNumbers.Add(vehicleRecord.Vehicle.LicenseNumber);
                }
            }

            return licenseNumbers;
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, eVehicleStatus i_NewStatus)
        {
            if (r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleRecord vehicleRecord))
            {
                vehicleRecord.UpdateStatus(i_NewStatus);
            }
            else
            {
                throw new KeyNotFoundException("Vehicle not found in the garage.");
            }
        }

        public void InflateVehicleTiresToMax(string i_LicenseNumber)
        {
            if (r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleRecord vehicleRecord))
            {
                try
                {
                    foreach (var wheel in vehicleRecord.Vehicle.Wheels)
                    {
                        float amountToInflate = wheel.MaxAirPressure - wheel.CurrentAirPressure;
                        wheel.Inflate(amountToInflate);
                    }
                }
                catch (ValueOutOfRangeException ex)
                {
                    throw new Exception($"Error inflating tires: {ex.Message}", ex);
                }
            }
            else
            {
                throw new KeyNotFoundException("Vehicle not found in the garage.");
            }
        }

        public void RefuelVehicle(string i_LicenseNumber, eFuelType i_FuelType, float i_AmountToFuel)
        {
            if (r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleRecord vehicleRecord))
            {
                if (vehicleRecord.Vehicle.EnergySource is FuelEnergy fuelEnergy)
                {
                    if (fuelEnergy.FuelType != i_FuelType)
                    {
                        throw new ArgumentException("Incorrect fuel type.");
                    }

                    try
                    {
                        fuelEnergy.Refuel(i_AmountToFuel);
                    }
                    catch (ValueOutOfRangeException ex)
                    {
                        throw new Exception($"Error refueling vehicle: {ex.Message}", ex);
                    }
                }
                else
                {
                    throw new ArgumentException("Vehicle does not use fuel.");
                }
            }
            else
            {
                throw new KeyNotFoundException("Vehicle not found in the garage.");
            }
        }

        public void RechargeVehicle(string i_LicenseNumber, float i_HoursToCharge)
        {
            if (r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleRecord vehicleRecord))
            {
                if (vehicleRecord.Vehicle.EnergySource is ElectricEnergy electricEnergy)
                {
                    try
                    {
                        electricEnergy.Recharge(i_HoursToCharge);
                    }
                    catch (ValueOutOfRangeException ex)
                    {
                        throw new Exception($"Error recharging vehicle: {ex.Message}", ex);
                    }
                }
                else
                {
                    throw new ArgumentException("Vehicle does not use electricity.");
                }
            }
            else
            {
                throw new KeyNotFoundException("Vehicle not found in the garage.");
            }
        }
    }
}
