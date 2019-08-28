using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TowerSoft.Repository.MySql {
    public class MySqlDbAdapter : IDbAdapter {
        public bool LastInsertIdInSeparateQuery => false;

        public IDbConnection CreateNewDbConnection(string connectionString) {
            return new MySqlConnection(connectionString);
        }

        public string GetLastInsertIdStatement() {
            return "SELECT LAST_INSERT_ID();";
        }

        public string GetLimitStatement() {
            return " LIMIT @LimitRowCount ";
        }

        public string GetOffsetStatement() {
            return " OFFSET @OffsetRowCount ";
        }

        public string GetParameterPlaceholder(string columnName) {
            return $"@{columnName}";
        }

        public string GetParameterName(string columnName) {
            return $"@{columnName}";
        }

        public string GetSelectColumnCast(Type type, string tableName, IMap map) {
            return $"{tableName}.{map.ColumnName} {map.PropertyName}";
        }
    }
}
