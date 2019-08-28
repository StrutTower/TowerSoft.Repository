using InterSystems.Data.CacheClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace TowerSoft.Repository.Cache {
    public class CacheDbAdapter : IDbAdapter {
        public bool LastInsertIdInSeparateQuery => true;


        public IDbConnection CreateNewDbConnection(string connectionString) {
            return new CacheConnection(connectionString);
        }

        public string GetLastInsertIdStatement() {
            return "SELECT LAST_IDENTITY()";
        }

        public string GetLimitStatement() {
            throw new NotImplementedException();
        }

        public string GetOffsetStatement() {
            throw new NotImplementedException();
        }

        public string GetParameterPlaceholder(string columnName) {
            return "?";
        }

        public string GetParameterName(string columnName) {
            return $"?{columnName}";
        }

        public string GetSelectColumnCast(Type type, string tableName, IMap map) {
            PropertyInfo pi = type.GetProperty(map.PropertyName);

            Type propertyType = pi.PropertyType;

            // Get underlying types for nullable objects
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                propertyType = pi.PropertyType.GetGenericArguments()[0];
            }

            string cast = "";

            if (Type.Equals(propertyType, typeof(short))) {
                cast = "SMALLINT";
            } else if (Type.Equals(propertyType, typeof(int))
                  || Type.Equals(propertyType, typeof(int))
                  || Type.Equals(propertyType.BaseType, typeof(Enum))) {
                cast = "INT";
            } else if (Type.Equals(propertyType, typeof(long))) {
                cast = "BIGINT";
            } else if (Type.Equals(propertyType, typeof(bool))) {
                cast = "INT";
            } else if (Type.Equals(propertyType, typeof(DateTime))) {
                cast = "DATETIME";
            }

            if (string.IsNullOrEmpty(cast)) {
                return tableName + "." + map.ColumnName;
            } else {
                return "CAST(" + tableName + "." + map.ColumnName + " AS " + cast + ") " + map.ColumnName;
            }
        }
    }
}
