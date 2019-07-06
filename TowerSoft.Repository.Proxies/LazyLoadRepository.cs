using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Attributes;
using TowerSoft.Repository.Helpers;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository.Proxies {
    public class LazyLoadRepository<T> : Repository<T> {
        private readonly static ProxyGenerator _generator = new ProxyGenerator();

        protected List<LazyLoadMap> LazyLoadMaps { get; private set; }

        #region Constructors
        public LazyLoadRepository(string connectionString, IDbAdapter dbAdapter)
            : base(connectionString, dbAdapter) {
            GetLazyLoadMapping();
        }

        public LazyLoadRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) {
            GetLazyLoadMapping();
        }

        public LazyLoadRepository(string connectionString, IDbAdapter dbAdapter, EntityMap<T> entityMap)
            : base(connectionString, dbAdapter, entityMap) { }

        public LazyLoadRepository(IUnitOfWork unitOfWork, EntityMap<T> entityMap)
            : base(unitOfWork, entityMap) { }
        #endregion

        #region Overrides
        protected override List<T> GetEntities(QueryBuilder queryBuilder) {
            List<T> output = new List<T>();
            List<T> entities = base.GetEntities(queryBuilder);

            IInterceptor lazyLoadInterceptor = new LazyLoadInterceptor(LazyLoadMaps);
            foreach (T entity in entities) {
                T proxy = (T)_generator.CreateClassProxy(typeof(T), lazyLoadInterceptor);
                foreach (IMap map in Mappings.AllMaps) {
                    object value = map.GetValue(entity);
                    map.SetValue(proxy, value);
                    output.Add(proxy);
                }
            }

            return output;
        }
        #endregion

        private void GetLazyLoadMapping() {
            Type domainType = typeof(T);
            LazyLoadMaps = new List<LazyLoadMap>();

            foreach (PropertyInfo prop in domainType.GetProperties()) {
                if (prop.IsDefined(typeof(NotMappedAttribute))) continue;

                if (prop.GetMethod != null && prop.GetMethod.IsPublic &&
                    prop.SetMethod != null && prop.SetMethod.IsPublic &&
                    prop.IsDefined(typeof(LazyLoadAttribute))) {
                    LazyLoadAttribute attr = (LazyLoadAttribute)prop.GetCustomAttribute(typeof(LazyLoadAttribute));
                    LazyLoadMaps.Add(new LazyLoadMap(prop.Name, attr.ForeignClassType, attr.GetByForeignKeyMethodName, attr.ForeignKey));
                }
            }
        }

        private void GetLazyLoadMappingFromEntityMap(LazyLoadEntityMap<T> entityMap) {
            LazyLoadMaps = new List<LazyLoadMap>();
            //TODO Add 

            LazyLoadMaps = entityMap.GetLazyLoadMaps
        }
    }
}
