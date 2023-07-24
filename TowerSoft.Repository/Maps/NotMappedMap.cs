using System;
using System.Collections.Generic;
using System.Text;

namespace TowerSoft.Repository.Maps {
    internal class NotMappedMap : Map {
        public NotMappedMap(string propertyAndColumnName) : base(propertyAndColumnName) {
        }

        public NotMappedMap(string propertyName, string columnName, string functionName = null) : base(propertyName, columnName, functionName) {
        }
    }
}
