using System;
using System.Collections.Generic;
using System.Text;

namespace TowerSoft.Repository.Maps {
    public class JoinedMap {
        public string PropertyName { get; set; }
        public string TableAndColumnName { get; set; }
        public string JoinStatement { get; set; }

        public JoinedMap(string propertyName, string tableAndColumnName, string joinStatement) {
            PropertyName = propertyName;
            TableAndColumnName = tableAndColumnName;
            JoinStatement = joinStatement;
        }

        public JoinedMap(string propertyName, string tableName, string columnName, string joinStatement) {
            PropertyName = propertyName;
            TableAndColumnName = tableName + "." + columnName;
            JoinStatement = joinStatement;
        }
    }
}
