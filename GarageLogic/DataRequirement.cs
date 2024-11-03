using System;

namespace GarageLogic
{
    public class DataRequirement
    {
        public string FieldName { get; }
        public Type DataType { get; }
        public string Prompt { get; }
        public object[] PossibleValues { get; }
        public float? MinValue { get; }
        public float? MaxValue { get; }

        public DataRequirement(
            string i_FieldName,
            Type i_DataType,
            string i_Prompt,
            object[] i_PossibleValues = null,
            float? i_MinValue = null,
            float? i_MaxValue = null)
        {
            FieldName = i_FieldName;
            DataType = i_DataType;
            Prompt = i_Prompt;
            PossibleValues = i_PossibleValues;
            MinValue = i_MinValue;
            MaxValue = i_MaxValue;
        }
    }
}
