using RichPackage.GuardClauses;
using RichPackage.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace RichPackage.Editor
{
    /// <summary>
    /// Copies components
    /// </summary>
    public class ObjectCopierWindow : OdinEditorWindow
    {
        #region Window

        public static ObjectCopierWindow Instance { get; private set; }

        [MenuItem(RichEditorUtility.WindowMenuName + nameof(ObjectCopierWindow))]
        private static void Init()
        {
            Instance = GetWindow<ObjectCopierWindow>();
            Instance.Show();
        }

        #endregion Window

        [Required]
        public Object source;

        [Required]
        public Object destination;

        [Button]
        public void Copy() => Copy(source, destination);

        public void Copy(Object src, Object dest)
        {
            // validate
            GuardAgainst.ArgumentIsNull(src, nameof(src));
            GuardAgainst.ArgumentIsNull(dest, nameof(dest));

            // operate
            Type srcType = src.GetType();
            Type destType = dest.GetType();

            var srcFields = new List<FieldInfo>();
            var destFields = new List<FieldInfo>();

            // find all public and serialized fields
            ReflectionUtility.GetAllSerializableFieldsInherited(srcType, srcFields);
            ReflectionUtility.GetAllSerializableFieldsInherited(destType, destFields);

            foreach (FieldInfo srcField in srcFields)
            {
                if (destFields.TryFind((f) => AreEqual(f, srcField), out FieldInfo destField))
                {
                    try
                    {
                        destField.SetValue(dest, srcField.GetValue(src));
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogWarning($"Couldn't set {srcField.Name} on {dest.name} due to {ex.GetType().Name}\n{ex.Message}");
                    }
                }
            }
        }

        private static bool AreEqual(FieldInfo a, FieldInfo b)
        {
            return a.Name == b.Name;
            //return a.DeclaringType == b.DeclaringType
            //    && a.Name == b.Name;
        }
    }
}
