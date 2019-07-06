using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TowerSoft.Repository.MicrosoftSql {
    public class MicrosoftSqlDbAdapter : IDbAdapter {
        public IDbConnection CreateNewDbConnection(string connectionString) {
            return new SqlConnection(connectionString);
        }

        public string GetLastInsertIdStatement() {
            return "SELECT SCOPE_IDENTITY();";
        }

        public string GetLimitStatement() {
            return " FETCH NEXT @LimitRowCount ROWS ONLY ";
        }

        public string GetOffsetStatement() {
            return " OFFSET @OffsetRowCount ROWS ";
        }

        public string GetParameterPlaceholder(string columnName) {
            return $"@{columnName}";
        }

        public string GetSelectColumnCast(string tableName, string columnName) {
            return $"@{tableName}.{columnName}";
        }
    }
}
