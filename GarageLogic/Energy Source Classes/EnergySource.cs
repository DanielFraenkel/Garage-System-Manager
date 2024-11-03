using System.Collections.Generic;

namespace GarageLogic
{
    internal abstract class EnergySource
    {
        public float EnergyPercentage { get; protected set; }

        protected void UpdateEnergyPercentage(float i_CurrentAmount, float i_MaxAmount)
        {
            EnergyPercentage = (i_CurrentAmount / i_MaxAmount) * 100;
        }

        public abstract void AssignProperties(Dictionary<string, object> i_UserInputs);

        public abstract EnergySourceInfo CreateInfo();

        public abstract List<DataRequirement> GetDataRequirements();
    }
}
