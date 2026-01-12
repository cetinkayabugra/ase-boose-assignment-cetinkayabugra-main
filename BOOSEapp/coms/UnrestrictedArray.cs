using System;
using System.Collections;
using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Unrestricted Array class - complete custom implementation
    /// Does not extend BOOSE.Array to avoid restrictions
    /// </summary>
    public class UnrestrictedArray
    {
        private string arrayName = "";
        private string arrayType = "";
        private int rows = 0;
        private int cols = 1;
        private ArrayList arrayData = new ArrayList();

        public UnrestrictedArray(string name, string type, int dimension1, int dimension2 = 1)
        {
            arrayName = name;
            arrayType = type.ToLower();
            rows = dimension1;
            cols = dimension2;

            // Initialize array with default values
            for (int i = 0; i < rows; i++)
            {
                if (cols > 1)
                {
                    // 2D array
                    ArrayList row = new ArrayList();
                    for (int j = 0; j < cols; j++)
                    {
                        row.Add(GetDefaultValue());
                    }
                    arrayData.Add(row);
                }
                else
                {
                    // 1D array
                    arrayData.Add(GetDefaultValue());
                }
            }
        }

        private object GetDefaultValue()
        {
            return arrayType == "int" ? (object)0 : (object)0.0;
        }

        public object GetValue(int index1, int index2 = 0)
        {
            if (index1 < 0 || index1 >= rows)
            {
                throw new Exception($"Array index {index1} out of bounds");
            }

            if (cols > 1)
            {
                ArrayList? row = arrayData[index1] as ArrayList;
                if (row != null && index2 >= 0 && index2 < cols)
                {
                    return row[index2] ?? GetDefaultValue();
                }
                throw new Exception("Array index out of bounds");
            }

            return arrayData[index1] ?? GetDefaultValue();
        }

        public void SetValue(int index1, int index2, object value)
        {
            if (index1 < 0 || index1 >= rows)
            {
                throw new Exception($"Array index {index1} out of bounds");
            }

            object convertedValue = arrayType == "int" ?
                (object)Convert.ToInt32(value) :
                (object)Convert.ToDouble(value);

            if (cols > 1)
            {
                ArrayList? row = arrayData[index1] as ArrayList;
                if (row != null && index2 >= 0 && index2 < cols)
                {
                    row[index2] = convertedValue;
                    return;
                }
                throw new Exception("Array index out of bounds");
            }

            arrayData[index1] = convertedValue;
        }

        public string GetName() => arrayName;
        public string GetArrayType() => arrayType;
    }
}