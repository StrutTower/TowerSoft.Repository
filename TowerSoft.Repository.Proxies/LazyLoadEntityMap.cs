using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TowerSoft.Repository.Builders;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository.Proxies {
    public abstract class LazyLoadEntityMap<T> : EntityMap<T> {
        public LazyLoadEntityMap(string tableName)
            : base(tableName) { }

        public LazyLoadEntityMap(string tableName, AutoMappingOption autoMappingOption)
            : base(tableName, autoMappingOption) { }

        public override abstract IEnumerable<IMap> GetMaps();

        public abstract IEnumerable<LazyLoadMap> GetLazyLoadMaps();

        public LazyLoadProperty<T> MapLazyLoadProperty<TProperty>(Expression<Func<T, TProperty>> expression) {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            return new LazyLoadProperty<T>(memberExpression.Member.Name);
        }
    }
}
