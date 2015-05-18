using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AMF
{
    public class ClassMapper
    {
        /*
         * Static fields
         */
        private static MappingSet _map;

        /*
         * Static methods
         */

        static ClassMapper()
        {
            _map = new MappingSet();
        }

        /*
         * Fields
         */


        /*
         * Methods
         */


        //! Default constructor
        public ClassMapper()
        {

        }

        public void RegisterClassAlias(string classLocal, string classRemote)
        {
            _map.RegisterClassAlias(classLocal, classRemote);
        }

        //! classMap - dictionary with key - class name local, value - class name remote
        public void RegisterClasses(Dictionary<string, string> classMap)
        {
            foreach (KeyValuePair<string, string> kvPair in classMap)
            {
                RegisterClassAlias(kvPair.Key, kvPair.Value);
            }
        }

        public void Reset()
        {
            _map = new MappingSet();
        }

        public string GetClassNameLocal(string classNameRemote)
        {
            return _map.GetClassNameLocal(classNameRemote);
        }

        // Returns the other-language class name for the given ruby object.
        public string GetClassNameRemote(object value)
        {
            string className = null;

            if (value is ObjectWithType)
            {
                className = (value as ObjectWithType).className;
            }
            else
            {
                className = value.GetType().FullName;
            }

            return _map.GetClassNameRemote(className);
        }

        public object CreateObject(string classNameRemote)
        {
            object result = null;

            if (string.IsNullOrEmpty(classNameRemote))
            {
                result = new ObjectDynamic();
                return result;
            }

            string classNameLocal = _map.GetClassNameLocal(classNameRemote);

            if (string.IsNullOrEmpty(classNameLocal))
            {
                result = new ObjectWithType(classNameRemote);
            }
            else
            {
                Type objectType = Type.GetType(classNameLocal);
                result = Activator.CreateInstance(objectType);
            }

            return result;
        }

        public void ObjectDeserialize(object value, Dictionary<string, object> properties)
        {
            if (value is ObjectDynamic)
            {
                (value as ObjectDynamic).deserialize(properties);
                return;
            }

            Type targetType = value.GetType();

            PropertyInfo[] typeProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (KeyValuePair<string, object> kvPair in properties)
            {
                bool isValueEstablished = false;

                try
                {
                    PropertyInfo property = targetType.GetProperty(kvPair.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

                    if (property != null)
                    {
                        MethodInfo propertySetter = property.GetSetMethod();
                        if (propertySetter != null)
                        {
                            propertySetter.Invoke(value, new object[]
                            {
                                kvPair.Value
                            });
                        }
                    }

                    if (!isValueEstablished)
                    {
                        FieldInfo fieldInfo = targetType.GetField(kvPair.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);

                        if (fieldInfo != null)
                        {
                            fieldInfo.SetValue(value, kvPair.Value);
                        }
                    }
                }
                catch (Exception e)
                {
                    AmfLogger.LogError(e.Message);
                }

                if (!isValueEstablished)
                {
                    AmfLogger.LogError("Can't establish property: " + kvPair.Key + " for class: " + targetType.FullName);
                }
            }
        }

        /// <summary>
        /// Returns dictionary with key - property name, value - property value of target
        /// </summary>
        public SortedDictionary<string, object> ObjectSerialize(object target)
        {
            SortedDictionary<string, object> result = new SortedDictionary<string, object>();

            Type targetType = target.GetType();

            PropertyInfo[] typeProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (PropertyInfo propertyInfo in typeProperties)
            {
                MethodInfo propertyGetter = propertyInfo.GetGetMethod();

                //check here because  BindingFlags.GetProperty sometimes not work
                if (propertyGetter != null)
                {
                    result[propertyInfo.Name] = propertyGetter.Invoke(target, null);
                }
            }

            FieldInfo[] typeFields = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo fieldInfo in typeFields)
            {
                result[fieldInfo.Name] = fieldInfo.GetValue(target);
            }

            return result;
        }
    }
}
	
