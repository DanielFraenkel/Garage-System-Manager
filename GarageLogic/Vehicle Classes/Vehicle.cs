using System;
using System.Collections.Generic;
using System.Linq;

namespace GarageLogic
{
    internal abstract class Vehicle
    {
        public string LicenseNumber { get; set; }
        public string ModelName { get; protected set; }
        public List<Wheel> Wheels { get; }
        public EnergySource EnergySource { get; protected set; }

        protected Vehicle(int i_NumberOfWheels, float i_MaxAirPressure)
        {
            Wheels = new List<Wheel>(i_NumberOfWheels);
            for (int i = 0; i < i_NumberOfWheels; i++)
            {
                Wheels.Add(new Wheel(i_MaxAirPressure));
            }
        }

        public virtual void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            if (i_UserInputs.TryGetValue("ModelName", out object modelNameObj) && modelNameObj is string modelName)
            {
                ModelName = modelName;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for ModelName.");
            }

            foreach (Wheel wheel in Wheels)
            {
                wheel.AssignProperties(i_UserInputs);
            }

            EnergySource.AssignProperties(i_UserInputs);
        }

        public virtual VehicleInfo CreateInfo()
        {
            var vehicleInfo = new VehicleInfo
            {
                LicenseNumber = this.LicenseNumber,
                ModelName = this.ModelName,
                WheelInfo = this.Wheels.FirstOrDefault()?.CreateInfo(),
                EnergySourceInfo = this.EnergySource.CreateInfo()
            };

            return vehicleInfo;
        }

        public virtual List<DataRequirement> GetDataRequirements()
        {
            var dataRequirements = new List<DataRequirement>
            {
                new DataRequirement("ModelName", typeof(string), "Enter the model name:")
            };

            // Add wheel data requirements
            dataRequirements.AddRange(Wheels[0].GetDataRequirements());

            // Add energy source data requirements
            dataRequirements.AddRange(EnergySource.GetDataRequirements());

            return dataRequirements;
        }
    }
}
