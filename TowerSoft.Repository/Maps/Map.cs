using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TowerSoft.Repository.Maps {
    public class Map : IMap {
        public string PropertyName { get; private set; }
        public string ColumnName { get; private set; }

        public Map(string propertyAndColumnName) {
            PropertyName = propertyAndColumnName;
            ColumnName = propertyAndColumnName;
        }

        public Map(string propertyName, string columnName) {
            PropertyName = propertyName;
            ColumnName = columnName;
        }

        public object GetValue(object entity) {
            PropertyInfo prop = entity.GetType().GetProperty(PropertyName);
            return prop.GetValue(entity);
        }

        public void SetValue(object entity, object value) {
            PropertyInfo prop = entity.GetType().GetProperty(PropertyName);
            prop.SetValue(entity, value);
        }
    }
}
