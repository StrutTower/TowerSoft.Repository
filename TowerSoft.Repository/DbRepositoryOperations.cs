using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository {
    public partial class DbRepository<T> : IDbRepository {
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
    }
}
