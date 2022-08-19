using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TowerSoft.Repository.Builders;

namespace TowerSoft.Repository {
    /// <summary>
    /// Extend this class to define the maps of a table.
    /// </summary>
    /// <typeparam name="TSource">Domain object type</typeparam>
    public abstract partial class EntityMap<TSource> {
        /// <summary>
        /// Initialize a new entity map
        /// </summary>
        /// <param name="tableName">Name of the database table</param>
        public EntityMap(string tableName) {
            TableName = tableName;
        }

        /// <summary>
        /// Initialize a new entity map
        /// </summary>
        /// <param name="tableName">Name of the table in the database</param>
        /// <param name="entityMapOption">Automapping option will enable automatically mapping properties to a column of the same name unless overridden.</param>
        public EntityMap(string tableName, AutoMappingOption entityMapOption = AutoMappingOption.None) {
            TableName = tableName;
            EntityMapOption = entityMapOption;
        }

        /// <summary>
        /// The table name
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// If true, the mapper will assume that any public non-virtual properties with a
        /// public getter and setter are the mapped to the same name as the property unless overridden in the entity map.
        /// </summary>
        public AutoMappingOption EntityMapOption { get; }

        /// <summary>
        /// Get all maps defined by this entity map
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IMap> GetMaps();

        /// <summary>
        /// Begins building a map for a property
        /// </summary>
        /// <typeparam name="TProperty">Property of the entity to map</typeparam>
        /// <param name="expression">Expression for the property of the entity to map</param>
        /// <returns></returns>
        protected PropertyMapBuilder MapProperty<TProperty>(Expression<Func<TSource, TProperty>> expression) {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            return new PropertyMapBuilder(memberExpression.Member.Name);
        }
    }
}
