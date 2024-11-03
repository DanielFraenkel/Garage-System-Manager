using System.Collections.Generic;

namespace GarageLogic
{
    internal class FuelEnergy : EnergySource
    {
        private readonly eFuelType r_FuelType;
        private float m_CurrentFuelAmountLiters;
        private readonly float r_MaxFuelAmountLiters;

        public eFuelType FuelType => r_FuelType;

        public float CurrentFuelAmountLiters
        {
            get => m_CurrentFuelAmountLiters;
            private set
            {
                if (value < 0 || value > r_MaxFuelAmountLiters)
                {
                    throw new ValueOutOfRangeException(0, r_MaxFuelAmountLiters);
                }
                m_CurrentFuelAmountLiters = value;
                UpdateEnergyPercentage(m_CurrentFuelAmountLiters, r_MaxFuelAmountLiters);
            }
        }

        public float MaxFuelAmountLiters => r_MaxFuelAmountLiters;

        public FuelEnergy(float i_MaxFuelAmountLiters, eFuelType i_FuelType)
        {
            r_MaxFuelAmountLiters = i_MaxFuelAmountLiters;
            r_FuelType = i_FuelType;
        }

        public override void AssignProperties(Dictionary<string, object> i_UserInputs)
        {
            if (i_UserInputs.TryGetValue("CurrentFuelAmountLiters", out object currentFuelObj) && currentFuelObj is float currentFuel)
            {
                CurrentFuelAmountLiters = currentFuel;
            }
            else
            {
                throw new ArgumentException("Invalid or missing value for CurrentFuelAmountLiters.");
            }
        }

        public void Refuel(float i_AmountToFuel)
        {
            if (i_AmountToFuel <= 0 || m_CurrentFuelAmountLiters + i_AmountToFuel > r_MaxFuelAmountLiters)
            {
                throw new ValueOutOfRangeException(0, r_MaxFuelAmountLiters - m_CurrentFuelAmountLiters);
            }

            m_CurrentFuelAmountLiters += i_AmountToFuel;
            UpdateEnergyPercentage(m_CurrentFuelAmountLiters, r_MaxFuelAmountLiters);
        }

        public override EnergySourceInfo CreateInfo()
        {
            return new FuelEnergyInfo
            {
                EnergyType = "Fuel",
                EnergyPercentage = this.EnergyPercentage,
                CurrentFuelAmountLiters = this.CurrentFuelAmountLiters,
                MaxFuelAmountLiters = this.MaxFuelAmountLiters,
                FuelType = this.FuelType
            };
        }

        public override List<DataRequirement> GetDataRequirements()
        {
            return new List<DataRequirement>
            {
                new DataRequirement(
                    "CurrentFuelAmountLiters",
                    typeof(float),
                    $"Enter the current fuel amount in liters",
                    null,
                    0,
                    MaxFuelAmountLiters)
            };
        }
    }
}
