using System.Collections.Generic;

namespace GarageLogic
{
    internal class VehicleRecord
    {
        public Vehicle Vehicle { get; }
        public string OwnerName { get; private set; }
        public string OwnerPhoneNumber { get; private set; }
        public eVehicleStatus VehicleStatus { get; private set; }

        public VehicleRecord(Vehicle i_Vehicle)
        {
            Vehicle = i_Vehicle;
            VehicleStatus = eVehicleStatus.InRepair;
        }

        public void UpdateStatus(eVehicleStatus i_NewStatus)
        {
            VehicleStatus = i_NewStatus;
        }

        public bool IsStatus(eVehicleStatus i_Status)
        {
            return this.VehicleStatus == i_Status;
        }

        public void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            if (i_UserInputs.TryGetValue("OwnerName", out object ownerNameObj) && ownerNameObj is string ownerName)
            {
                OwnerName = ownerName;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for OwnerName.");
            }

            if (i_UserInputs.TryGetValue("OwnerPhoneNumber", out object ownerPhoneObj) && ownerPhoneObj is string ownerPhone)
            {
                OwnerPhoneNumber = ownerPhone;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for OwnerPhoneNumber.");
            }

            Vehicle.AssignProperties(i_UserInputs);
        }

        public VehicleRecordInfo CreateInfo()
        {
            var vehicleInfo = Vehicle.CreateInfo();

            return new VehicleRecordInfo
            {
                OwnerName = this.OwnerName,
                OwnerPhoneNumber = this.OwnerPhoneNumber,
                VehicleStatus = this.VehicleStatus,
                VehicleInfo = vehicleInfo
            };
        }

        public List<DataRequirement> GetDataRequirements()
        {
            var dataRequirements = new List<DataRequirement>
            {
                new DataRequirement("OwnerName", typeof(string), "Enter the owner's name:"),
                new DataRequirement("OwnerPhoneNumber", typeof(string), "Enter the owner's phone number:")
            };

            // Add vehicle's data requirements
            dataRequirements.AddRange(Vehicle.GetDataRequirements());

            return dataRequirements;
        }
    }
}
