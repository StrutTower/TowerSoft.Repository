using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TowerSoft.Repository.Proxies;

namespace TowerSoft.Repository.Helpers {
    public class LazyLoadInterceptor : IInterceptor {
        private readonly ISet<string> alreadyLoadedObjects = new HashSet<string>();
        private readonly IEnumerable<LazyLoadMap> _maps;

        public LazyLoadInterceptor(IEnumerable<LazyLoadMap> lazyLoadMaps) {
            _maps = lazyLoadMaps;
        }

        public void Intercept(IInvocation invocation) {
            if (_maps != null && _maps.Any()) {
                // Store a lazy load
                LazyLoadMap lazyLoadMap = _maps.Where(l =>
                    "get_" + l.PropertyName.ToLower() == invocation.Method.Name.ToLower()).FirstOrDefault();

                if (lazyLoadMap != null) {
                    if (alreadyLoadedObjects.Contains(lazyLoadMap.PropertyName) == false) {

                        // Get the lazy loaded object
                        PropertyInfo foreignProp = invocation.InvocationTarget.GetType()
                            .GetProperty(lazyLoadMap.ParameterPropertyName);


                        if (foreignProp == null) {
                            throw new Exception("Could not find property "
                                + lazyLoadMap.ParameterPropertyName
                                + " in class " + invocation.InvocationTarget.GetType().Name);
                        }

                        object loadingObject = Activator.CreateInstance(lazyLoadMap.LoadingClassType);
                        MethodInfo loadingMethod = lazyLoadMap.LoadingClassType.GetMethod(lazyLoadMap.LoadingMethodName);

                        if (loadingMethod == null) {
                            throw new Exception("Could not find method " + lazyLoadMap.LoadingMethodName
                                + " on loading object " + lazyLoadMap.LoadingClassType.Name);
                        }
                        object returnedObject = loadingMethod.Invoke(loadingObject, new object[] {
                            foreignProp.GetValue(invocation.InvocationTarget, null) });

                        // Store the lazy object using the target's setter
                        MethodInfo setMethod = invocation.InvocationTarget.GetType()
                            .GetMethod("set_" + lazyLoadMap.PropertyName);

                        if (setMethod == null) {
                            throw new Exception("Could not find setter for property "
                                + lazyLoadMap.PropertyName + " in class "
                                + invocation.InvocationTarget.GetType().Name);
                        }

                        setMethod.Invoke(invocation.InvocationTarget, new object[] { returnedObject });

                        // Remember the name of the object so it doesn't get loaded again
                        alreadyLoadedObjects.Add(lazyLoadMap.PropertyName);
                    }
                }
            }
            invocation.Proceed();
        }
    }
}
