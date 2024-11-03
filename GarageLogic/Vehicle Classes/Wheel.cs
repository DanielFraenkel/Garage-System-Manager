using System;
using System.Collections.Generic;

namespace GarageLogic
{
    internal class Wheel
    {
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;
        private readonly float r_MaxAirPressure;

        public string ManufacturerName
        {
            get => m_ManufacturerName;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Manufacturer name cannot be empty.");
                }
                m_ManufacturerName = value;
            }
        }

        public float CurrentAirPressure
        {
            get => m_CurrentAirPressure;
            private set
            {
                if (value < 0 || value > r_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, r_MaxAirPressure);
                }
                m_CurrentAirPressure = value;
            }
        }

        public float MaxAirPressure => r_MaxAirPressure;

        public Wheel(float i_MaxAirPressure)
        {
            r_MaxAirPressure = i_MaxAirPressure;
        }

        public void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            if (i_UserInputs.TryGetValue("ManufacturerName", out object manufacturerNameObj) && manufacturerNameObj is string manufacturerName)
            {
                ManufacturerName = manufacturerName;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for ManufacturerName.");
            }

            if (i_UserInputs.TryGetValue("CurrentAirPressure", out object currentAirPressureObj) && currentAirPressureObj is float currentAirPressure)
            {
                CurrentAirPressure = currentAirPressure;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for CurrentAirPressure.");
            }
        }

        public void Inflate(float i_AirPressureToAdd)
        {
            if (i_AirPressureToAdd <= 0 || m_CurrentAirPressure + i_AirPressureToAdd > r_MaxAirPressure)
            {
                throw new ValueOutOfRangeException(0, r_MaxAirPressure - m_CurrentAirPressure);
            }

            m_CurrentAirPressure += i_AirPressureToAdd;
        }

        public WheelInfo CreateInfo()
        {
            return new WheelInfo
            {
                ManufacturerName = this.ManufacturerName,
                CurrentAirPressure = this.CurrentAirPressure,
                MaxAirPressure = this.MaxAirPressure
            };
        }

        public List<DataRequirement> GetDataRequirements()
        {
            return new List<DataRequirement>
            {
                new DataRequirement(
                    "ManufacturerName",
                    typeof(string),
                    "Enter the wheel manufacturer name:"),
                new DataRequirement(
                    "CurrentAirPressure",
                    typeof(float),
                    $"Enter the current air pressure",
                    null,
                    0,
                    MaxAirPressure)
            };
        }
    }
}
