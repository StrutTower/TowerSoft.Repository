using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TowerSoft.Repository.Maps;
using TowerSoft.Repository.Proxies;

namespace TowerSoft.Repository.Builders {
   public sealed class LazyLoadParameter<T> {
        private string LazyLoadPropertyName { get; set; }
        private Type LoadFromClassType { get; set; }
        private string LoadMethod { get; set; }
        private string ParameterPropertyName { get; set; }

        internal LazyLoadParameter(string lazyLoadPropertyName, Type type, string methodName) {
            LazyLoadPropertyName = lazyLoadPropertyName;
            LoadFromClassType = type;
            LoadMethod = methodName;
        }

        public LazyLoadMap WithParameter<TProperty>(Expression<Func<T, TProperty>> expression) {
            return new LazyLoadMap(LazyLoadPropertyName, LoadFromClassType, LoadMethod, ParameterPropertyName);
        }
    }
}
