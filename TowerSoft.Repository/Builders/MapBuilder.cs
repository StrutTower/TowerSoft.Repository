using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository.Builders
{
    /// <summary>
    /// Map builder to help define maps in the repository constructor. Can implicitly be converted to List<IMap>
    /// </summary>
    /// <typeparam name="T">Domain object type</typeparam>
    public class MapBuilder<T>
    {
        private List<IMap> Maps { get; set; } = new List<IMap>();

        /// <summary>
        /// Add an autonumber map to the list of maps
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Expression for the domain object property</param>
        /// <param name="overrideDatabaseColumnName">
        /// Name of the column in the database.
        /// Uses the name of the C# property if omitted or null.
        /// </param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        /// <returns></returns>
        public MapBuilder<T> AutonumberMap<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string overrideDatabaseColumnName = null, string functionName = null)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            Maps.Add(new AutonumberMap(memberExpression.Member.Name, overrideDatabaseColumnName, functionName));
            return this;
        }

        /// <summary>
        /// Add an ID map to the list of maps
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Expression for the domain object property</param>
        /// <param name="overrideDatabaseColumnName">
        /// Name of the column in the database.
        /// Uses the name of the C# property if omitted or null.
        /// </param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        /// <returns></returns>
        public MapBuilder<T> IDMap<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string overrideDatabaseColumnName = null, string functionName = null)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            Maps.Add(new IDMap(memberExpression.Member.Name, overrideDatabaseColumnName, functionName));
            return this;
        }

        /// <summary>
        /// Add an standard map to the list of maps
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Expression for the domain object property</param>
        /// <param name="overrideDatabaseColumnName">
        /// Name of the column in the database.
        /// Uses the name of the C# property if omitted or null.
        /// </param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        /// <returns></returns>
        public MapBuilder<T> Map<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string overrideDatabaseColumnName = null, string functionName = null)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            Maps.Add(new Map(memberExpression.Member.Name, overrideDatabaseColumnName, functionName));
            return this;
        }

        /// <summary>
        /// Add an Cache/Iris Fileman date map to the list of maps
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Expression for the domain object property</param>
        /// <param name="overrideDatabaseColumnName">
        /// Name of the column in the database.
        /// Uses the name of the C# property if omitted or null.
        /// </param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        /// <returns></returns>
        public MapBuilder<T> FilemanDateMap<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string overrideDatabaseColumnName = null, string functionName = null)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            Maps.Add(new CacheFilemanDateMap(memberExpression.Member.Name, overrideDatabaseColumnName, functionName));
            return this;
        }

        /// <summary>
        /// Add an Cache/Iris Fileman datetime map to the list of maps
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Expression for the domain object property</param>
        /// <param name="overrideDatabaseColumnName">
        /// Name of the column in the database.
        /// Uses the name of the C# property if omitted or null.
        /// </param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        /// <returns></returns>
        public MapBuilder<T> FilemanDateTimeMap<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string overrideDatabaseColumnName = null, string functionName = null)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            Maps.Add(new CacheFilemanDateTimeMap(memberExpression.Member.Name, overrideDatabaseColumnName, functionName));
            return this;
        }

        /// <summary>
        /// Add an Cache/Iris horolog date map to the list of maps
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Expression for the domain object property</param>
        /// <param name="overrideDatabaseColumnName">
        /// Name of the column in the database.
        /// Uses the name of the C# property if omitted or null.
        /// </param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        /// <returns></returns>
        public MapBuilder<T> HorologDateMap<TProperty>(Expression<Func<T, TProperty>> propertyExpression, string overrideDatabaseColumnName = null, string functionName = null)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            Maps.Add(new CacheHorologDateMap(memberExpression.Member.Name, overrideDatabaseColumnName, functionName));
            return this;
        }


        public static implicit operator List<IMap>(MapBuilder<T> builder)
        {
            return builder.Maps;
        }
    }
}
