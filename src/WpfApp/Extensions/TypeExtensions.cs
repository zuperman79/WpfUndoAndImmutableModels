using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WpfApp.Model;

namespace WpfApp.Extensions
{
    public static class TypeExtensions
    {
        private static Dictionary<Type, PropertyInfo[]> propertyStore = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Copies the properties of an object to another, except a given property that will have a different given value
        /// </summary>
        /// <typeparam name="T">the object type</typeparam>
        /// <param name="type">the object type</param>
        /// <param name="parent">the "donor" object</param>
        /// <param name="propertyName">the property name that will receive the new value</param>
        /// <param name="value">the new value</param>
        /// <returns>the "cloned" object</returns>
        public static T Copy<T>(this T type, T parent, string propertyName, object value) where T : IStateObject
        {
            PropertyInfo[] typeProperties;

            if (propertyStore.TryGetValue(typeof(T), out var properties))
            {
                typeProperties = properties;
            }
            else
            {
                typeProperties = type.GetType().GetProperties();
                propertyStore.Add(typeof(T), typeProperties);
            }

            foreach (var property in typeProperties.Where(p => p.CanWrite))
            {
                if (property.Name == propertyName)
                {
                    property.SetValue(type, value);
                }
                else
                {
                    property.SetValue(type, property.GetValue(parent));
                }
            }

            return type;
        }
    }
}