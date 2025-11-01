using System;

namespace TowerSoft.Repository.Utilities {
    internal static class StringUtilities {
        internal static void TrimProperties(object obj) {
            if (obj == null) return;
            Type type = obj.GetType();

            foreach (System.Reflection.PropertyInfo prop in type.GetProperties()) {
                if (prop.PropertyType == typeof(string)) {
                    string value = (string)prop.GetValue(obj);
                    if (value != null)
                        prop.SetValue(obj, value.Trim());
                }
            }
        }

        internal static void NullOutEmptyStrings(object obj) {
            if (obj == null) return;
            Type type = obj.GetType();

            foreach (System.Reflection.PropertyInfo prop in type.GetProperties()) {
                if (prop.PropertyType == typeof(string)) {
                    string value = (string)prop.GetValue(obj);
                    if (value != null && string.IsNullOrWhiteSpace(value))
                        prop.SetValue(obj, null);
                }
            }
        }
    }
}
