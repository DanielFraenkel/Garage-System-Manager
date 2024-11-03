using System.Collections.Generic;

namespace GarageLogic
{
    internal class Truck : Vehicle
    {
        private const int k_NumberOfWheels = 12;
        private const float k_MaxAirPressure = 28f;

        public bool IsCarryingHazardousMaterials { get; private set; }
        public float CargoVolume { get; private set; }

        public Truck(FuelEnergy i_EnergySource)
            : base(k_NumberOfWheels, k_MaxAirPressure)
        {
            EnergySource = i_EnergySource;
        }

        public override void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            base.AssignProperties(i_UserInputs);

            if (i_UserInputs.TryGetValue("IsCarryingHazardousMaterials", out object hazardousObj) && hazardousObj is bool isHazardous)
            {
                IsCarryingHazardousMaterials = isHazardous;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for IsCarryingHazardousMaterials.");
            }

            if (i_UserInputs.TryGetValue("CargoVolume", out object cargoVolumeObj) && cargoVolumeObj is float cargoVolume)
            {
                CargoVolume = cargoVolume;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for CargoVolume.");
            }
        }

        public override VehicleInfo CreateInfo()
        {
            VehicleInfo baseInfo = base.CreateInfo();

            var truckInfo = new TruckInfo
            {
                LicenseNumber = baseInfo.LicenseNumber,
                ModelName = baseInfo.ModelName,
                WheelInfo = baseInfo.WheelInfo,
                EnergySourceInfo = baseInfo.EnergySourceInfo,
                IsCarryingHazardousMaterials = this.IsCarryingHazardousMaterials,
                CargoVolume = this.CargoVolume
            };

            return truckInfo;
        }

        public override List<DataRequirement> GetDataRequirements()
        {
            var dataRequirements = base.GetDataRequirements();

            dataRequirements.Add(new DataRequirement(
                "IsCarryingHazardousMaterials",
                typeof(bool),
                "Is the truck carrying hazardous materials? (true/false):"));

            dataRequirements.Add(new DataRequirement(
                "CargoVolume",
                typeof(float),
                "Enter the cargo volume:",
                null,
                0));

            return dataRequirements;
        }
    }
}
