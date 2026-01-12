using System;
using System.Collections.Generic;

namespace BOOSEapp
{
    /// <summary>
    /// Stores method definitions
    /// </summary>
    public class MethodDefinition
    {
        public string Name { get; set; } = "";
        public string ReturnType { get; set; } = "";
        public List<(string type, string name)> Parameters { get; set; } = new();
        public int StartLine { get; set; }
        public int EndLine { get; set; }
    }

    public class MethodStore
    {
        private readonly Dictionary<string, MethodDefinition> _methods = new();

        public void AddMethod(string name, MethodDefinition method)
        {
            _methods[name.ToLower()] = method;
        }

        public MethodDefinition GetMethod(string name)
        {
            var key = name.ToLower();
            if (_methods.TryGetValue(key, out var method))
                return method;
            throw new Exception($"Method '{name}' not found");
        }

        public bool HasMethod(string name)
        {
            return _methods.ContainsKey(name.ToLower());
        }
    }
}