using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerSoft.Repository.Attributes {
    public class JoinedMapAttribute : Attribute {
        public string TableAndColumnName { get; set; }
        public string JoinStatement { get; set; }

        public JoinedMapAttribute(string tableAndColumnName, string joinStatement) {
            TableAndColumnName = tableAndColumnName;
            JoinStatement = joinStatement;
        }

        public JoinedMapAttribute(string tableName, string columnName, string joinStatement) {
            TableAndColumnName = tableName + "." + columnName;
            JoinStatement = joinStatement;
        }
    }
}
