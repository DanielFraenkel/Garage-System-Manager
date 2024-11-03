using System.Collections.Generic;
using System.Dynamic;

namespace GarageLogic
{
    internal class Car : Vehicle
    {
        private const int k_NumberOfWheels = 5;
        private const float k_MaxAirPressure = 30f;

        public eColor Color { get; private set; }
        public eNumberOfDoors NumberOfDoors { get; private set; }

        public Car(EnergySource i_EnergySource)
            : base(k_NumberOfWheels, k_MaxAirPressure)
        {
            EnergySource = i_EnergySource;
        }

        public override void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            base.AssignProperties(i_UserInputs);

            if (i_UserInputs.TryGetValue("Color", out object colorObj) && colorObj is eColor color)
            {
                Color = color;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for Color.");
            }

            if (i_UserInputs.TryGetValue("NumberOfDoors", out object doorsObj) && doorsObj is eNumberOfDoors numberOfDoors)
            {
                NumberOfDoors = numberOfDoors;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for NumberOfDoors.");
            }
        }

        public override VehicleInfo CreateInfo()
        {
            VehicleInfo baseInfo = base.CreateInfo();

            var carInfo = new CarInfo
            {
                LicenseNumber = baseInfo.LicenseNumber,
                ModelName = baseInfo.ModelName,
                WheelInfo = baseInfo.WheelInfo,
                EnergySourceInfo = baseInfo.EnergySourceInfo,
                Color = this.Color,
                NumberOfDoors = this.NumberOfDoors
            };

            return carInfo;
        }

        public override List<DataRequirement> GetDataRequirements()
        {
            var dataRequirements = base.GetDataRequirements();

            dataRequirements.Add(new DataRequirement(
                "Color",
                typeof(eColor),
                "Select the car color:",
                Enum.GetValues(typeof(eColor)).Cast<object>().ToArray()));

            dataRequirements.Add(new DataRequirement(
                "NumberOfDoors",
                typeof(eNumberOfDoors),
                "Select the number of doors:",
                Enum.GetValues(typeof(eNumberOfDoors)).Cast<object>().ToArray()));

            return dataRequirements;
        }
    }
}
