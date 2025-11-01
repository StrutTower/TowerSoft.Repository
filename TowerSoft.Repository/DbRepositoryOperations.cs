using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.Maps;
using TowerSoft.Repository.Utilities;

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
        /// Get all entities from the database table
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAsync() {
            return await GetEntitiesAsync(GetQueryBuilder());
        }

        /// <summary>
        /// Adds the supplied entity to the database
        /// </summary>
        /// <param name="entity">Entity to add to the database</param>
        public virtual void Add(T entity) {
            AddAsync(entity).Wait();
        }

        /// <summary>
        /// Adds the supplied entity to the database
        /// </summary>
        /// <param name="entity">Entity to add to the database</param>
        public virtual async Task AddAsync(T entity) {
            if (DbAdapter.AutomaticallyTrimStrings)
                StringUtilities.TrimProperties(entity);
            if (DbAdapter.AutomaticallyConvertWhiteSpaceStringsToNull)
                StringUtilities.NullOutEmptyStrings(entity);

            List<string> columns = [];
            List<string> values = [];
            Dictionary<string, object> parameters = [];

            int index = 1;
            foreach (Map map in Mappings.AllMaps.Where(x => x != Mappings.AutonumberMap)) {
                columns.Add(map.ColumnName);
                values.Add(DbAdapter.GetParameterPlaceholder(map.ColumnName, index));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index), map.GetValue(entity));
                index++;
            }

            string query = $"INSERT INTO {TableName} " +
                $"({string.Join(",", columns)}) VALUES ({string.Join(",", values)})";

            WriteLog(GetType().Name, query, parameters);

            if (Mappings.AutonumberMap == null) {
                await GetDbConnection().ExecuteAsync(query, parameters, DbAdapter.DbTransaction);
            } else {
                long autonumberValue = 0;
                if (DbAdapter.LastInsertIdInSeparateQuery) {
                    GetDbConnection().Execute(query, parameters, DbAdapter.DbTransaction);
                    autonumberValue = await GetDbConnection().QuerySingleAsync<long>(DbAdapter.GetLastInsertIdStatement(), null, DbAdapter.DbTransaction);
                } else {
                    query += ";" + DbAdapter.GetLastInsertIdStatement();
                    autonumberValue = await GetDbConnection().QuerySingleAsync<long>(query, parameters, DbAdapter.DbTransaction);
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
            AddAsync(entities).Wait();
        }

        /// <summary>
        /// Add multiple values to the database. Inserts are done in batches. Cache does not support this method of inserting multiple entities and will instead add them separately.
        /// </summary>
        /// <param name="entities">Entities to add to the database</param>
        public virtual async Task AddAsync(IEnumerable<T> entities) {
            if (DbAdapter.ListInsertSupported) {

                if (DbAdapter.AutomaticallyTrimStrings) {
                    foreach (var entity in entities)
                        StringUtilities.TrimProperties(entity);
                }
                if (DbAdapter.AutomaticallyConvertWhiteSpaceStringsToNull) {
                    foreach (var entity in entities)
                        StringUtilities.NullOutEmptyStrings(entity);
                }

                List<string> columns = [];

                foreach (Map map in Mappings.AllMaps.Where(x => x != Mappings.AutonumberMap)) {
                    columns.Add(map.ColumnName);
                }

                // Calculate the batch size based on the number of parameters that will be generated in the SQL statements.
                // SQLite has a parameter limit of 999
                int batchSize = 900 / columns.Count;

                // Split entities into batches
                foreach (List<T> batchGroup in entities.Select((x, i) => new { Value = x, Index = i }).GroupBy(x => x.Index / batchSize).Select(x => x.Select(y => y.Value).ToList())) {
                    string query = $"INSERT INTO {TableName} ({string.Join(",", columns)}) VALUES ";

                    List<string> values = [];
                    Dictionary<string, object> parameters = [];
                    int counter = 1;
                    foreach (T entity in batchGroup) {
                        List<string> vals = [];
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

                    WriteLog(GetType().Name, query, parameters);

                    await GetDbConnection().ExecuteAsync(query, parameters, DbAdapter.DbTransaction);
                }
            } else {
                foreach (T entity in entities) {
                    await AddAsync(entity);
                }
            }
        }

        /// <summary>
        /// Updates the matching row in the database
        /// </summary>
        /// <param name="entity">Entity to update in the database</param>
        public virtual void Update(T entity) {
            UpdateAsync(entity).Wait();
        }

        /// <summary>
        /// Updates the matching row in the database
        /// </summary>
        /// <param name="entity">Entity to update in the database</param>
        public virtual async Task UpdateAsync(T entity) {
            if (DbAdapter.AutomaticallyTrimStrings)
                StringUtilities.TrimProperties(entity);
            if (DbAdapter.AutomaticallyConvertWhiteSpaceStringsToNull)
                StringUtilities.NullOutEmptyStrings(entity);

            List<string> updateColumns = [];
            List<string> primaryKeyColumns = [];
            Dictionary<string, object> parameters = [];

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

            WriteLog(GetType().Name, query, parameters);

            await GetDbConnection().ExecuteAsync(query, parameters, DbAdapter.DbTransaction);
        }

        /// <summary>
        /// Updates only the specifically defined properties/columns
        /// </summary>
        /// <param name="entity">Entity to update in the database</param>
        /// <param name="properties">Lambda expression of the properties to update.</param>
        protected virtual void UpdateColumns(T entity, params Expression<Func<T, object>>[] properties) {
            UpdateColumnsAsync(entity, properties).Wait();
        }

        /// <summary>
        /// Updates only the specifically defined properties/columns
        /// </summary>
        /// <param name="entity">Entity to update in the database</param>
        /// <param name="properties">Lambda expression of the properties to update.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual async Task UpdateColumnsAsync(T entity, params Expression<Func<T, object>>[] properties) {
            if (DbAdapter.AutomaticallyTrimStrings)
                StringUtilities.TrimProperties(entity);
            if (DbAdapter.AutomaticallyConvertWhiteSpaceStringsToNull)
                StringUtilities.NullOutEmptyStrings(entity);

            List<IMap> maps = [];
            List<string> missingMaps = [];
            List<string> invalidMaps = [];

            foreach (var property in properties) {
                MemberExpression memberExpression;
                if (property.Body is MemberExpression) {
                    memberExpression = (MemberExpression)property.Body;
                } else if (property.Body is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert) {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                } else {
                    throw new Exception($"Unexpected expression type {property.NodeType}");
                }

                if (!Mappings.AllMapsDictionary.ContainsKey(memberExpression.Member.Name)) {
                    missingMaps.Add(memberExpression.Member.Name);
                    continue;
                }
                IMap map = Mappings.AllMaps.Single(x => x.PropertyName == memberExpression.Member.Name);

                if (map is AutonumberMap || map is IDMap) {
                    invalidMaps.Add(memberExpression.Member.Name);
                    continue;
                }
                maps.Add(map);
            }

            if (missingMaps.Any()) {
                throw new Exception($"{typeof(T).Name}: Maps cannot be found for the following properties: {string.Join(", ", missingMaps)}");
            }
            if (invalidMaps.Any()) {
                throw new Exception($"{typeof(T).Name}: Cannot update the following primary key maps: {string.Join(", ", invalidMaps)}");
            }

            List<string> updateColumns = [];
            List<string> primaryKeyColumns = [];
            Dictionary<string, object> parameters = [];

            int index = 1;
            foreach (Map map in maps) {
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

            WriteLog(GetType().Name, query, parameters);

            await GetDbConnection().ExecuteAsync(query, parameters, DbAdapter.DbTransaction);
        }

        /// <summary>
        /// Removes the matching row from the database
        /// </summary>
        /// <param name="entity">Entity to remove from the database</param>
        public virtual void Remove(T entity) {
            RemoveAsync(entity).Wait();
        }

        /// <summary>
        /// Removes the matching row from the database
        /// </summary>
        /// <param name="entity">Entity to remove from the database</param>
        public virtual async Task RemoveAsync(T entity) {
            if (DbAdapter.AutomaticallyTrimStrings)
                StringUtilities.TrimProperties(entity);
            if (DbAdapter.AutomaticallyConvertWhiteSpaceStringsToNull)
                StringUtilities.NullOutEmptyStrings(entity);

            List<string> primaryKeyColumns = [];
            Dictionary<string, object> parameters = [];

            int index = 1;
            foreach (Map map in Mappings.PrimaryKeyMaps) {
                primaryKeyColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName, index));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName, index), map.GetValue(entity));
                index++;
            }

            string query = $"DELETE FROM {TableName} WHERE {string.Join(" AND ", primaryKeyColumns)}";

            WriteLog(GetType().Name, query, parameters);

            await GetDbConnection().ExecuteAsync(query, parameters, DbAdapter.DbTransaction);
        }
    }
}
