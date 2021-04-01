﻿using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository {
    /// <summary>
    /// Repository wrapper for Dapper
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public partial class DbRepository<T> : IDbRepository {
        #region Constructors
        /// <summary>
        /// Creates a new Repository using attributes to define the database mapping.
        /// All non-virtual properties with public getters and setters will be mapped
        /// unless overridden with an attribute.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="overrideTableName">Overrides the table name generated from the object name or the table attribute.</param>
        /// <param name="ignoreObjectMaps">Sets if properties that end with _Object or _Objects will be ignored by the automatic mapping</param>
        public DbRepository(IDbAdapter dbAdapter, string overrideTableName = null, bool ignoreObjectMaps = true) {
            DbAdapter = dbAdapter;
            ConnectionString = dbAdapter.ConnectionString;
            if (string.IsNullOrWhiteSpace(overrideTableName))
                TableName = GetTableName();
            else
                TableName = overrideTableName;
            Mappings = new MappingModel<T>(ignoreObjectMaps);
        }

        /// <summary>
        /// Creates a new Repository using an EntityMap to define the database mapping.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="entityMap">Entity map class used to define the mapping for this repository</param>
        public DbRepository(IDbAdapter dbAdapter, EntityMap<T> entityMap) {
            DbAdapter = dbAdapter;
            ConnectionString = dbAdapter.ConnectionString;
            TableName = entityMap.TableName;
            Mappings = new MappingModel<T>(entityMap);
        }

        /// <summary>
        /// Creates a new Repository with the table name and maps passed directly into the contructor.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="maps">List of maps for this tabe</param>
        public DbRepository(IDbAdapter dbAdapter, string tableName, IEnumerable<IMap> maps) {
            DbAdapter = dbAdapter;
            ConnectionString = dbAdapter.ConnectionString;
            TableName = tableName;
            Mappings = new MappingModel<T>(maps);
        }
        #endregion

        #region Properties
        /// <summary>
        /// DBAdapter used by this Repository
        /// </summary>
        protected IDbAdapter DbAdapter { get; private set; }

        /// <summary>
        /// Connection string being used by this Repository
        /// </summary>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// Name of the database table
        /// </summary>
        protected string TableName { get; private set; }

        /// <summary>
        /// Mapping model object. Stores all of the database maps
        /// </summary>
        protected MappingModel<T> Mappings { get; private set; }
        #endregion

        #region Operations
        /// <summary>
        /// Get all entities from the database table
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetAll() {
            return GetEntities(GetQueryBuilder());
        }

        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        /// <returns></returns>
        public virtual long GetCount() {
            return GetCount(whereConditions: null);
        }

        /// <summary>
        /// Returns the number of rows that match the supplied WhereCondition
        /// </summary>
        /// <param name="whereCondition">WhereCondition to filter by</param>
        /// <returns></returns>
        protected virtual long GetCount(WhereCondition whereCondition) {
            return GetCount(new[] { whereCondition });
        }

        /// <summary>
        /// Return the number of rows that match the supplied WhereConditions
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereCondtions to filter by</param>
        /// <returns></returns>
        protected virtual long GetCount(IEnumerable<WhereCondition> whereConditions) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string query = $"SELECT COUNT(*) FROM {TableName} ";
            if (whereConditions != null && whereConditions.Any()) {
                List<string> whereStatements = new List<string>();
                int index = 1;
                foreach (WhereCondition whereCondition in whereConditions) {
                    if (whereCondition.IsNullEqualsOrNotEquals()) {
                        whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " NULL");
                    } else {
                        whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " " + DbAdapter.GetParameterPlaceholder(whereCondition.ColumnName, index));
                        parameters.Add(DbAdapter.GetParameterName(whereCondition.ColumnName, index), whereCondition.GetParameterValue());
                    }
                    index++;
                }
                query += "WHERE " + string.Join(" AND ", whereStatements);
            }

            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");

            return GetDbConnection().QuerySingle<long>(query, parameters, DbAdapter.DbTransaction);
        }

        /// <summary>
        /// Adds the supplied entity to the database
        /// </summary>
        /// <param name="entity">Entity to add to the database</param>
        public virtual void Add(T entity) {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            int index = 1;
            foreach (Map map in Mappings.AllMaps.Where(x => x != Mappings.AutonumberMap)) {
                columns.Add(map.ColumnName);
                values.Add(DbAdapter.GetParameterPlaceholder(map.ColumnName, index));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index), map.GetValue(entity));
                index++;
            }

            string query = $"INSERT INTO {TableName} " +
                $"({string.Join(",", columns)}) VALUES ({string.Join(",", values)})";

            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");

            if (Mappings.AutonumberMap == null) {
                GetDbConnection().Execute(query, parameters, DbAdapter.DbTransaction);
            } else {
                long autonumberValue = 0;
                if (DbAdapter.LastInsertIdInSeparateQuery) {
                    GetDbConnection().Execute(query, parameters, DbAdapter.DbTransaction);
                    autonumberValue = GetDbConnection().QuerySingle<long>(DbAdapter.GetLastInsertIdStatement(), null, DbAdapter.DbTransaction);
                } else {
                    query += ";" + DbAdapter.GetLastInsertIdStatement();
                    autonumberValue = GetDbConnection().QuerySingle<long>(query, parameters, DbAdapter.DbTransaction);
                }
                PropertyInfo prop = entity.GetType().GetProperty(Mappings.AutonumberMap.PropertyName);
                Mappings.AutonumberMap.SetValue(entity, Convert.ChangeType(autonumberValue, prop.PropertyType));
            }
        }

        /// <summary>
        /// Add multiple values to the database. Inserts are done in batches. Cache does not support this method of inserting multiple entities and will instead add them separately.
        /// </summary>
        /// <param name="entities">Entities to add to the database</param>
        public virtual void Add(IEnumerable<T> entities) {
            if (DbAdapter.ListInsertSupported) {
                List<string> columns = new List<string>();

                foreach (Map map in Mappings.AllMaps.Where(x => x != Mappings.AutonumberMap)) {
                    columns.Add(map.ColumnName);
                }

                // Caculate the batch size based on the number of parameters that will be generated in the SQL statements.
                // SQLite has a parameter limit of 999
                int batchSize = 900 / columns.Count;

                // Split entities into batches
                foreach (List<T> batchGroup in entities.Select((x, i) => new { Value = x, Index = i }).GroupBy(x => x.Index / batchSize).Select(x => x.Select(y => y.Value).ToList())) {
                    string query = $"INSERT INTO {TableName} ({string.Join(",", columns)}) VALUES ";

                    List<string> values = new List<string>();
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    int counter = 1;
                    foreach (T entity in batchGroup) {
                        List<string> vals = new List<string>();
                        int index = 1;
                        foreach (Map map in Mappings.AllMaps.Where(x => x != Mappings.AutonumberMap)) {
                            vals.Add(DbAdapter.GetParameterPlaceholder(map.ColumnName, index) + counter);
                            parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index) + counter, map.GetValue(entity));
                            index++;
                        }
                        values.Add($"({string.Join(",", vals)})");
                        counter++;
                    }
                    query += string.Join(",", values);

                    if (DbAdapter.DebugLogger != null)
                        DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");

                    GetDbConnection().Execute(query, parameters, DbAdapter.DbTransaction);
                }
            } else {
                foreach (T entity in entities) {
                    Add(entity);
                }
            }
        }

        /// <summary>
        /// Updates the matching row in the database
        /// </summary>
        /// <param name="entity">Entity to update in the database</param>
        public virtual void Update(T entity) {
            List<string> updateColumns = new List<string>();
            List<string> primaryKeyColumns = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            int index = 1;
            foreach (Map map in Mappings.StandardMaps) {
                updateColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName, index));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index), map.GetValue(entity));
                index++;
            }

            foreach (Map map in Mappings.PrimaryKeyMaps) {
                primaryKeyColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName, index));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index), map.GetValue(entity));
                index++;
            }

            string query = $"UPDATE {TableName} " +
                $"SET {string.Join(",", updateColumns)} " +
                $"WHERE {string.Join(" AND ", primaryKeyColumns)} ";

            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");

            GetDbConnection().Execute(query, parameters, DbAdapter.DbTransaction);
        }

        /// <summary>
        /// Removes the matching row from the database
        /// </summary>
        /// <param name="entity">Entity to remove from the database</param>
        public virtual void Remove(T entity) {
            List<string> primaryKeyColumns = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            int index = 1;
            foreach (Map map in Mappings.PrimaryKeyMaps) {
                primaryKeyColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName, index));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index), map.GetValue(entity));
                index++;
            }

            string query = $"DELETE FROM {TableName} WHERE {string.Join(" AND ", primaryKeyColumns)}";

            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");

            GetDbConnection().Execute(query, parameters, DbAdapter.DbTransaction);
        }
        #endregion

        #region Statement Builders
        /// <summary>
        /// Returns the first matching row from the database
        /// </summary>
        /// <param name="whereCondition">WhereCondition to filter by</param>
        /// <returns></returns>
        protected T GetSingleEntity(WhereCondition whereCondition) {
            return GetEntities(whereCondition).FirstOrDefault();
        }

        /// <summary>
        /// Returns the first matching row from the database
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereConditions to filter by</param>
        /// <returns></returns>
        protected T GetSingleEntity(IEnumerable<WhereCondition> whereConditions) {
            return GetEntities(whereConditions).FirstOrDefault();
        }

        /// <summary>
        /// Returns the first row from the database
        /// </summary>
        /// <param name="queryBuilder">QueryBuilder </param>
        /// <returns></returns>
        protected T GetSingleEntity(QueryBuilder queryBuilder) {
            return GetEntities(queryBuilder).FirstOrDefault();
        }

        /// <summary>
        /// Returns all matching rows from the database
        /// </summary>
        /// <param name="whereCondition">WhereCondition to filter by</param>
        /// <returns></returns>
        protected List<T> GetEntities(WhereCondition whereCondition) {
            return GetEntities(new[] { whereCondition });
        }

        /// <summary>
        /// Returns all matching rows from the database
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereConditions to filter by</param>
        /// <returns></returns>
        protected List<T> GetEntities(IEnumerable<WhereCondition> whereConditions) {
            QueryBuilder query = GetQueryBuilder();
            List<string> whereStatements = new List<string>();
            int index = 1;
            foreach (WhereCondition whereCondition in whereConditions) {
                if (whereCondition.IsNullEqualsOrNotEquals()) {
                    whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " NULL");
                } else {
                    whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " " + DbAdapter.GetParameterPlaceholder(whereCondition.ColumnName, index));
                    query.AddParameter(DbAdapter.GetParameterName(whereCondition.ColumnName, index), whereCondition.GetParameterValue());
                }
                index++;
            }
            query.SqlQuery += "WHERE " + string.Join(" AND ", whereStatements);
            return GetEntities(query);
        }

        /// <summary>
        /// Returns all rows returns from the SQL query
        /// </summary>
        /// <param name="queryBuilder">QueryBuilder</param>
        /// <returns></returns>
        protected virtual List<T> GetEntities(QueryBuilder queryBuilder) {
            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {queryBuilder.SqlQuery} /Parameters/ {string.Join(", ", queryBuilder.Parameters.Select(x => x.Key + ":" + x.Value))}");
            List<T> entities = GetDbConnection().Query<T>(queryBuilder.SqlQuery, queryBuilder.Parameters, DbAdapter.DbTransaction).ToList();
            PostProcessEntities(entities);
            return entities;
        }

        /// <summary>
        /// Returns a new instance of QueryBuilder for this repository
        /// </summary>
        /// <returns></returns>
        protected QueryBuilder GetQueryBuilder() {
            return new QueryBuilder(TableName, Mappings.AllMaps, DbAdapter, typeof(T));
        }

        /// <summary>
        /// Creates a new WhereCondition with the equal comparison using a strongly-typed property name
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Entity property</param>
        /// <param name="value">Value to compare to</param>
        /// <returns></returns>
        protected WhereCondition WhereEqual<TProperty>(Expression<Func<T, TProperty>> propertyExpression, object value) {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (!Mappings.AllMapsDictionary.ContainsKey(memberExpression.Member.Name)) {
                throw new Exception("Unable to find a map for the property '" + memberExpression.Member.Name + "'.");
            }
            string columnName = Mappings.AllMapsDictionary[memberExpression.Member.Name];
            return new WhereCondition(columnName, value, Comparison.Equals);
        }

        /// <summary>
        /// Creates a new WhereCondition using a strongly-typed property name
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Entity property</param>
        /// <param name="comparison">Comparision type</param>
        /// <param name="value">Value to compare to</param>
        /// <returns></returns>
        protected WhereCondition Where<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Comparison comparison, object value) {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (!Mappings.AllMapsDictionary.ContainsKey(memberExpression.Member.Name)) {
                throw new Exception("Unable to find a map for the property '" + memberExpression.Member.Name + "'.");
            }
            string columnName = Mappings.AllMapsDictionary[memberExpression.Member.Name];
            return new WhereCondition(columnName, value, comparison);
        }
        #endregion

        /// <summary>
        /// Get the current DbConnection or creates a new one
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetDbConnection() {
            if (DbAdapter.IsDisposed)
                throw new Exception("This DbAdapter has already been disposed. " +
                    "Make sure you are calling the DbAdapter and your Unit of Work class in a using statement.");

            if (DbAdapter.DbConnection == null)
                DbAdapter.DbConnection = DbAdapter.CreateNewDbConnection(ConnectionString);

            if (DbAdapter.DbConnection.State != ConnectionState.Open)
                DbAdapter.DbConnection.Open();

            DbAdapter.ConfigureDbConnection();

            return DbAdapter.DbConnection;
        }

        /// <summary>
        /// Returns the basic SELECT and FROM  parts of a SQL statement for the datebase table
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBasicSelectText() {
            List<string> columns = new List<string>();
            List<string> joins = new List<string>();
            foreach (Map map in Mappings.AllMaps) {
                columns.Add(DbAdapter.GetSelectColumnCast(typeof(T), TableName, map));
            }

            string query = $"SELECT {string.Join(",", columns)} FROM {TableName} ";
            if (joins.Any())
                query += string.Join(" ", joins.Distinct()) + " ";
            return query;
        }

        /// <summary>
        /// Can be overriden to run code on entities after GetEntities or GetSingleEntity. This method does nothing unless overriden.
        /// </summary>
        /// <param name="entities">Entities retrieved from the database</param>
        protected virtual void PostProcessEntities(List<T> entities) { }

        private string GetTableName() {
            Type domainType = typeof(T);

            string tableName = domainType.Name;
            if (domainType.IsDefined(typeof(TableAttribute))) {
                TableAttribute tableAttr = (TableAttribute)domainType.GetCustomAttribute(typeof(TableAttribute));
                if (!string.IsNullOrWhiteSpace(tableAttr.Name)) {
                    tableName = tableAttr.Name;
                }
            }

            return tableName;
        }
    }
}