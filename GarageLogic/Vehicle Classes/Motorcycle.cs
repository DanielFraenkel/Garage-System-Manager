using System.Collections.Generic;

namespace GarageLogic
{
    internal class Motorcycle : Vehicle
    {
        private const int k_NumberOfWheels = 2;
        private const float k_MaxAirPressure = 29f;

        public eLicenseType LicenseType { get; private set; }
        public int EngineVolume { get; private set; }

        public Motorcycle(EnergySource i_EnergySource)
            : base(k_NumberOfWheels, k_MaxAirPressure)
        {
            EnergySource = i_EnergySource;
        }

        public override void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            base.AssignProperties(i_UserInputs);

            if (i_UserInputs.TryGetValue("LicenseType", out object licenseTypeObj) && licenseTypeObj is eLicenseType licenseType)
            {
                LicenseType = licenseType;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for LicenseType.");
            }

            if (i_UserInputs.TryGetValue("EngineVolume", out object engineVolumeObj) && engineVolumeObj is int engineVolume)
            {
                EngineVolume = engineVolume;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for EngineVolume.");
            }
        }

        public override VehicleInfo CreateInfo()
        {
            VehicleInfo baseInfo = base.CreateInfo();

            var motorcycleInfo = new MotorcycleInfo
            {
                LicenseNumber = baseInfo.LicenseNumber,
                ModelName = baseInfo.ModelName,
                WheelInfo = baseInfo.WheelInfo,
                EnergySourceInfo = baseInfo.EnergySourceInfo,
                LicenseType = this.LicenseType,
                EngineVolume = this.EngineVolume
            };

            return motorcycleInfo;
        }

        public override List<DataRequirement> GetDataRequirements()
        {
            var dataRequirements = base.GetDataRequirements();

            dataRequirements.Add(new DataRequirement(
                "LicenseType",
                typeof(eLicenseType),
                "Select the license type:",
                Enum.GetValues(typeof(eLicenseType)).Cast<object>().ToArray()));

            dataRequirements.Add(new DataRequirement(
                "EngineVolume",
                typeof(int),
                "Enter the engine volume:",
                null,
                0));

            return dataRequirements;
        }
    }
}
