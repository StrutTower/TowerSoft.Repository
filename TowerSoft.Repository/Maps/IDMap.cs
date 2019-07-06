using System;
using System.Collections.Generic;
using System.Text;

namespace TowerSoft.Repository.Maps {
    public class IDMap : Map {
        public IDMap(string propertyAndColumnName) : base(propertyAndColumnName) { }
        public IDMap(string propertyName, string columnName) : base(propertyName, columnName) { }
    }
}
