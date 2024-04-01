using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ModestTree;

namespace RichPackage.Reflection
{
    public static class ReflectionUtility
    {
        public static Type[] GetAllClassesWithAttribute(Type attribute, bool inherit = true)
        {
            var allClasses = GetAllTypesThatImplement(typeof(object));
            return allClasses.Where(o => o.GetCustomAttributes(attribute, inherit).Length > 0).ToArray();
        }

        public static FieldInfo[] GetFieldsWithAttributeInherited(object obj, Type attribute)
        {
            var fieldInfo = new List<FieldInfo>();
            GetAllFieldsInherited(obj.GetType(), fieldInfo);

            return fieldInfo.Where(o => o.GetCustomAttributes(attribute, true).Length > 0).ToArray();
        }

        public static void GetAllSerializableFieldsInherited(Type startType, List<FieldInfo> appendList)
        {
            GetAllFieldsInherited(startType, appendList);
            appendList.RemoveAll(o => 
	            (
	                (o.IsPrivate || o.IsFamily) &&
	                o.GetCustomAttributes(typeof(SerializeField), true).Length == 0
	            ) ||
	            o.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0 ||
	            o.GetCustomAttributes(typeof(HideInInspector), true).Length > 0);
        }

        public static void GetAllFieldsInherited(Type startType, List<FieldInfo> appendList)
        {
            GetAllFieldsInherited(startType, appendList, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
        
        public static void GetAllFieldsInherited(Type startType, List<FieldInfo> appendList, BindingFlags flags)
        {
	        while (true)
	        {
                // base case
		        if (startType == typeof(MonoBehaviour) || startType == null
                    || startType == typeof(object))
		        {
			        return;
		        }

		        var fields = startType.GetFields(flags);
		        foreach (var fieldInfo in fields)
		        {
			        if (appendList.Any(o => o.Name == fieldInfo.Name) == false)
			        {
				        appendList.Add(fieldInfo);
			        }
		        }

		        // Keep going until we hit UnityEngine.MonoBehaviour type or null.
		        startType = startType.BaseType;
	        }
        }
        
        public static FieldInfo GetFieldInherited(Type startType, string fieldName)
        {
	        while (true)
	        {
                // base case
		        if (startType == typeof(MonoBehaviour) || startType == null
                    || startType == typeof(object))
                {
			        return null;
                }

		        // Copied fields can be restricted with BindingFlags
		        var field = startType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		        if (field != null)
		        {
			        return field;
		        }

		        // Keep going until we hit UnityEngine.MonoBehaviour type or null.
		        startType = startType.BaseType;
	        }
        }

        public static IEnumerable<FieldInfo> GetSerializedFields<T>(object target)
        {
            Type targetType = target.GetType();
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return targetType.GetFields(flags)
                .Where(field => field.FieldType.DerivesFromOrEqual(typeof(T)))
                .Where(field => field.HasCustomAttribute<SerializeField>());
        }

        public static IEnumerable<FieldInfo> GetPublicFields<T>(object target)
        {
            Type targetType = target.GetType();
            var flags = BindingFlags.Public | BindingFlags.Instance;
            return targetType.GetFields(flags)
                .Where(field => field.FieldType.DerivesFromOrEqual(typeof(T)));
        }

        public static bool HasCustomAttribute<TAttribute>(this MemberInfo member)
            where TAttribute : Attribute
        {
            return member.GetCustomAttribute<TAttribute>() != null;
        }

        public static IEnumerable<T> EnumerateMemberFieldValues<T>(object target)
        {
            Type targetType = target.GetType();
            var flags = BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance;
            FieldInfo[] fields = targetType.GetFields(flags);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(T))
                {
                    T dialogueAsset = (T)field.GetValue(target);
                    if (dialogueAsset != null)
                    {
                        yield return dialogueAsset;
                    }
                }
            }
        }

        public static IEnumerable<FieldInfo> EnumerateMemberFields<T>(object target)
        {
            return GetPublicFields<T>(target)
                .Concat(GetSerializedFields<T>(target));
        }

        /// <summary>
        /// Gets public/private member method.
        /// </summary>
        public static MethodInfo GetMethodByName(Type type, string methodName)
        {
            return type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        /// Gets public/private member methods.
        /// </summary>
		public static MethodInfo[] GetAllMethodsFromType(Type type)
        {
            return GetAllMethodsFromType(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static MethodInfo[] GetAllMethodsFromType(Type type, BindingFlags flags)
        {
            return type.GetMethods(flags);
        }

        public static Type[] GetAllTypesThatImplement(Type type)
        {
            return GetAllTypesThatImplement(type, true);
        }

        public static Type[] GetAllTypesThatImplement(Type type, bool creatableTypesOnly)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            var assemblyList = new List<Assembly>();
//            foreach (var assembly in assemblies)
//            {
//                if (assembly.FullName.StartsWith("Mono.Cecil") ||
//                    assembly.FullName.StartsWith("UnityScript") ||
//                    assembly.FullName.StartsWith("Boo.Lan") ||
//                    assembly.FullName.StartsWith("System") ||
//                    assembly.FullName.StartsWith("JetBrains") ||
//                    assembly.FullName.StartsWith("nunit") ||
//                    assembly.FullName.StartsWith("NUnit") ||
//                    assembly.FullName.StartsWith("I18N") ||
////                    assembly.FullName.StartsWith("UnityEngine") ||
//                    //assembly.FullName.StartsWith("UnityEditor") ||
//                    assembly.FullName.StartsWith("mscorlib"))
//                {
//                    continue;
//                }

//                assemblyList.Add(assembly);
//            }

			// TODO - optimize enumeration
            var types = new List<Type>(assemblies.Length);
            foreach(var assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            types = types.Where(type.IsAssignableFrom).ToList();
            if (creatableTypesOnly)
            {
                types = types.Where(o => o.IsAbstract == false && o.IsInterface == false).ToList();
            }

            return types.ToArray();
        }
        
        /// <summary>
        /// Note that this is a really slow method, and should be used with caution...
        /// </summary>
        public static void CopySerializableValues(object from, object to)
        {
            var fromFields = new List<FieldInfo>();
            GetAllSerializableFieldsInherited(from.GetType(), fromFields);

            var toFields = new List<FieldInfo>();
            GetAllSerializableFieldsInherited(to.GetType(), toFields);

            foreach (var fromField in fromFields)
            {
                var toField = toFields.FirstOrDefault(o => o.Name == fromField.Name);
                if (toField != null)
                {
                    try
                    {
                        toField.SetValue(to, fromField.GetValue(from));
                    }
                    catch (Exception)
                    {
                        // Ignored
                    }
                }
            }
        }
        
        public static bool IsBuiltInUnityObjectType(Type type)
        {
            return type.Namespace != null && type.Name.Contains("UnityEngine");
        }
    
        /// <returns>All the public instance properties that can be read and written.</returns>
        public static IEnumerable<PropertyInfo> GetAllProperties<T>()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where((p) => p.CanRead && p.CanWrite);
        }
    }
}