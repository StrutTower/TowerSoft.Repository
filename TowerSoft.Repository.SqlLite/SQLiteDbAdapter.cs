using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace TowerSoft.Repository.SQLite {
    public class SQLiteDbAdapter : IDbAdapter {
        public bool LastInsertIdInSeparateQuery => false;

        public IDbConnection CreateNewDbConnection(string connectionString) {
            return new SQLiteConnection(connectionString);
        }

        public string GetLastInsertIdStatement() {
            return "SELECT last_insert_rowid();";
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
