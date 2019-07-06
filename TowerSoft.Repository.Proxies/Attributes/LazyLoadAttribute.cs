using System;
using System.Collections.Generic;
using System.Text;

namespace TowerSoft.Repository.Attributes {
   public class LazyLoadAttribute : Attribute {
        public Type ForeignClassType { get; }
        public string GetByForeignKeyMethodName { get; }
        public string ForeignKey { get; }

        public LazyLoadAttribute(Type foreignClassType, string getByForeignKeyMethodName, string foreignKey) {
            ForeignClassType = foreignClassType;
            GetByForeignKeyMethodName = getByForeignKeyMethodName;
            ForeignKey = foreignKey;
        }
    }
}
