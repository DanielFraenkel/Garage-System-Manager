using System.Collections.Generic;

namespace GarageLogic
{
    internal class ElectricEnergy : EnergySource
    {
        private float m_CurrentBatteryTimeHours;
        private readonly float r_MaxBatteryTimeHours;

        public float CurrentBatteryTimeHours
        {
            get => m_CurrentBatteryTimeHours;
            private set
            {
                if (value < 0 || value > r_MaxBatteryTimeHours)
                {
                    throw new ValueOutOfRangeException(0, r_MaxBatteryTimeHours);
                }
                m_CurrentBatteryTimeHours = value;
                UpdateEnergyPercentage(m_CurrentBatteryTimeHours, r_MaxBatteryTimeHours);
            }
        }

        public float MaxBatteryTimeHours => r_MaxBatteryTimeHours;

        public ElectricEnergy(float i_MaxBatteryTimeHours)
        {
            r_MaxBatteryTimeHours = i_MaxBatteryTimeHours;
        }

        public override void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            if (i_UserInputs.TryGetValue("CurrentBatteryTimeHours", out object currentBatteryObj) && currentBatteryObj is float currentBattery)
            {
                CurrentBatteryTimeHours = currentBattery;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for CurrentBatteryTimeHours.");
            }
        }

        public void Recharge(float i_HoursToCharge)
        {
            if (i_HoursToCharge <= 0 || m_CurrentBatteryTimeHours + i_HoursToCharge > r_MaxBatteryTimeHours)
            {
                throw new ValueOutOfRangeException(0, r_MaxBatteryTimeHours - m_CurrentBatteryTimeHours);
            }

            m_CurrentBatteryTimeHours += i_HoursToCharge;
            UpdateEnergyPercentage(m_CurrentBatteryTimeHours, r_MaxBatteryTimeHours);
        }

        public override EnergySourceInfo CreateInfo()
        {
            return new ElectricEnergyInfo
            {
                EnergyType = "Electric",
                EnergyPercentage = this.EnergyPercentage,
                CurrentBatteryTimeHours = this.CurrentBatteryTimeHours,
                MaxBatteryTimeHours = this.MaxBatteryTimeHours
            };
        }

        public override List<DataRequirement> GetDataRequirements()
        {
            return new List<DataRequirement>
            {
                new DataRequirement(
                    "CurrentBatteryTimeHours",
                    typeof(float),
                    $"Enter the current battery time in hours",
                    null,
                    0,
                    MaxBatteryTimeHours)
            };
        }
    }
}
