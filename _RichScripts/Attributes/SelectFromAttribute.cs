using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SelectFromAttribute : PropertyAttribute
    {
        public string MethodName { get; set; }

        public IEnumerable<string> Options { get; set; }

        public SelectFromAttribute(string methodName, IEnumerable<string> options)
        {
            MethodName = methodName ?? string.Empty;
            Options = options ?? Enumerable.Empty<string>();
        }
        public SelectFromAttribute(string methodName) : this(methodName, null) { }
        public SelectFromAttribute(IEnumerable<string> options) : this(null, options) { }
        public SelectFromAttribute() : this(null, null) { }
    }
}
