using System.Collections.Generic;
using System.Reflection;

namespace RichPackage.Collections
{
    /// <summary>
    /// A quick and dirty table of untyped properties.
    /// </summary>
    public sealed class PropertyTable : Dictionary<string, object>
    {
        #region Constructors

        public PropertyTable() { }

        public PropertyTable(IDictionary<string, object> dictionary) : base(dictionary) { }

        /// <summary>
        /// Uses reflection to map all of <paramref name="obj"/> public fields.
        /// </summary>
        public PropertyTable(object obj)
        {
            if (obj == null)
                return;

            // shortcut if obj is already a dictionary
            if (obj is IDictionary<string, object> dic)
            {
                foreach (KeyValuePair<string, object> item in dic)
                    Add(item.Key, item.Value);

                return;
            }

            System.Type objectType = obj.GetType();

            // Get all public fields of the object
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public);

            // Iterate over the fields and add them to the table
            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(obj);
                this[field.Name] = value;
            }
        }

        #endregion Constructors

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static PropertyTable FromJson(string jsonString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PropertyTable>(jsonString);
        }
    }
}
