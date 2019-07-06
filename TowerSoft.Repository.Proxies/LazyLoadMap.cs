using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TowerSoft.Repository.Proxies {
    public class LazyLoadMap {
        public string PropertyName { get; set; }
        public Type LoadingClassType { get; set; }
        public string LoadingMethodName { get; set; }
        public string ParameterPropertyName { get; set; }

        public LazyLoadMap(string propertyName, Type loaderClass, string loaderMethod, string parameterName) {
            PropertyName = propertyName;
            LoadingClassType = loaderClass;
            LoadingMethodName = loaderMethod;
            ParameterPropertyName = parameterName;
        }
    }
}
