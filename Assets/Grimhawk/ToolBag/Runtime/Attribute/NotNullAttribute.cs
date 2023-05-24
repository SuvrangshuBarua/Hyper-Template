using System;
using System.Diagnostics;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class NotNullAttribute: PropertyAttribute
    {
        public NotNullAttribute() : this("Variable has to be assigned") { }
        public NotNullAttribute(string label) { Label = label; }

        public string Label { get; private set; }
    }
}

