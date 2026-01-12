using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Stores all variables (int, real, boolean) and arrays
    /// No restrictions on number of variables
    /// </summary>
    public class VariableStore
    {
        private readonly Dictionary<string, int> _ints = new();
        private readonly Dictionary<string, double> _reals = new();
        private readonly Dictionary<string, bool> _booleans = new();
        private readonly Dictionary<string, int[]> _intArrays = new();
        private readonly Dictionary<string, double[]> _realArrays = new();

        private static string Key(string name) => name.Trim().ToLowerInvariant();

        // Int variables
        public void SetInt(string name, int value) => _ints[Key(name)] = value;

        public int GetInt(string name)
        {
            var key = Key(name);
            if (_ints.TryGetValue(key, out var value)) return value;
            throw new Exception($"Int variable '{name}' not found");
        }

        public bool HasInt(string name) => _ints.ContainsKey(Key(name));

        // Real variables
        public void SetReal(string name, double value) => _reals[Key(name)] = value;

        public double GetReal(string name)
        {
            var key = Key(name);
            if (_reals.TryGetValue(key, out var value)) return value;
            throw new Exception($"Real variable '{name}' not found");
        }

        public bool HasReal(string name) => _reals.ContainsKey(Key(name));

        // Boolean variables
        public void SetBoolean(string name, bool value) => _booleans[Key(name)] = value;

        public bool GetBoolean(string name)
        {
            var key = Key(name);
            if (_booleans.TryGetValue(key, out var value)) return value;
            throw new Exception($"Boolean variable '{name}' not found");
        }

        public bool HasBoolean(string name) => _booleans.ContainsKey(Key(name));

        // Int arrays
        public void CreateIntArray(string name, int size)
        {
            _intArrays[Key(name)] = new int[size];
        }

        public void SetIntArrayValue(string name, int index, int value)
        {
            var key = Key(name);
            if (!_intArrays.TryGetValue(key, out var array))
                throw new Exception($"Int array '{name}' not found");
            if (index < 0 || index >= array.Length)
                throw new Exception($"Array index {index} out of bounds");
            array[index] = value;
        }

        public int GetIntArrayValue(string name, int index)
        {
            var key = Key(name);
            if (!_intArrays.TryGetValue(key, out var array))
                throw new Exception($"Int array '{name}' not found");
            if (index < 0 || index >= array.Length)
                throw new Exception($"Array index {index} out of bounds");
            return array[index];
        }

        public bool HasIntArray(string name) => _intArrays.ContainsKey(Key(name));

        // Real arrays
        public void CreateRealArray(string name, int size)
        {
            _realArrays[Key(name)] = new double[size];
        }

        public void SetRealArrayValue(string name, int index, double value)
        {
            var key = Key(name);
            if (!_realArrays.TryGetValue(key, out var array))
                throw new Exception($"Real array '{name}' not found");
            if (index < 0 || index >= array.Length)
                throw new Exception($"Array index {index} out of bounds");
            array[index] = value;
        }

        public double GetRealArrayValue(string name, int index)
        {
            var key = Key(name);
            if (!_realArrays.TryGetValue(key, out var array))
                throw new Exception($"Real array '{name}' not found");
            if (index < 0 || index >= array.Length)
                throw new Exception($"Array index {index} out of bounds");
            return array[index];
        }

        public bool HasRealArray(string name) => _realArrays.ContainsKey(Key(name));

        // Get value of any type (for expressions)
        public double GetValue(string name)
        {
            var key = Key(name);
            if (_ints.TryGetValue(key, out var intVal)) return intVal;
            if (_reals.TryGetValue(key, out var realVal)) return realVal;
            throw new Exception($"Variable '{name}' not found");
        }

        public void Clear()
        {
            _ints.Clear();
            _reals.Clear();
            _booleans.Clear();
            _intArrays.Clear();
            _realArrays.Clear();
        }
    }
}