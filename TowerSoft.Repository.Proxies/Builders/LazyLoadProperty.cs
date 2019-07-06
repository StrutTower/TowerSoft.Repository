using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TowerSoft.Repository.Builders {
    public sealed class LazyLoadProperty<T> {
        private string LazyLoadPropertyName { get; set; }

        internal LazyLoadProperty(string propertyName) {
            LazyLoadPropertyName = propertyName;
        }

        public LazyLoadParameter<T> LoadFrom(Type type, string methodName) {
            return new LazyLoadParameter<T>(LazyLoadPropertyName, type, methodName);
        }
    }
}
