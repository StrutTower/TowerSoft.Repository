using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository {
    public partial class Repository<T> {
        #region Constructors
        /// <summary>
        /// Create a new Repository using attributes to define the database mapping.
        /// All non-virtual properties with public getters and setters will be mapped
        /// unless overridden with an attribute.
        /// </summary>
        /// <param name="connectionString">Connection string to the database</param>
        /// <param name="dbAdapter">DbAdapter for the database. These are found in separate packages for each database supported.</param>
        public Repository(string connectionString, IDbAdapter dbAdapter) {
            ConnectionString = connectionString;
            DbAdapter = dbAdapter;
            TableName = GetTableName();
            Mappings = new MappingModel<T>();
            IsUnitOfWorkPattern = false;
        }

        /// <summary>
        /// Creates a new Repository with the Unit of Work pattern and uses attributes to define the database mapping.
        /// All non-virtual properties with public getters and setters will be mapped
        /// unless overridden with an attribute.
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork class for the database being used</param>
        public Repository(IUnitOfWork unitOfWork) {
            DbConnection = unitOfWork.DbConnection;
            DbTransaction = unitOfWork.DbTransaction;
            ConnectionString = unitOfWork.ConnectionString;
            DbAdapter = unitOfWork.DbAdapter;
            TableName = GetTableName();
            Mappings = new MappingModel<T>();
            IsUnitOfWorkPattern = true;
        }

        /// <summary>
        /// Creates a new repository using an EntityMap to define the database mapping.
        /// </summary>
        /// <param name="connectionString">Connection string to the database</param>
        /// <param name="dbAdapter">DbAdapter for the database. These are found in separate packages for each database supported.</param>
        /// <param name="entityMap">Entity map class used to define the mapping for this repository</param>
        public Repository(string connectionString, IDbAdapter dbAdapter, EntityMap<T> entityMap) {
            ConnectionString = connectionString;
            DbAdapter = dbAdapter;
            TableName = entityMap.TableName;
            Mappings = new MappingModel<T>(entityMap);
            IsUnitOfWorkPattern = false;
        }

        /// <summary>
        /// Creates a new Repository usongbthe Unit of Work pattern and an EntityMap to define the database mapping
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork class for the database being used</param>
        /// <param name="entityMap">Entity map class used to define the mapping for this repository</param>
        public Repository(IUnitOfWork unitOfWork, EntityMap<T> entityMap) {
            DbConnection = unitOfWork.DbConnection;
            DbTransaction = unitOfWork.DbTransaction;
            ConnectionString = unitOfWork.ConnectionString;
            DbAdapter = unitOfWork.DbAdapter;
            TableName = entityMap.TableName;
            Mappings = new MappingModel<T>(entityMap);
            IsUnitOfWorkPattern = true;
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

        /// <summary>
        /// Stores if the Repository was created using the Unir of Work pattern
        /// </summary>
        protected bool IsUnitOfWorkPattern { get; private set; }

        /// <summary>
        /// Current ADO.NET connection used by this Repository
        /// </summary>
        protected IDbConnection DbConnection { get; private set; }

        /// <summary>
        /// Current ADO.NET transaction used by this Repository
        /// </summary>
        protected IDbTransaction DbTransaction { get; }
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
        public virtual long GetCount(WhereCondition whereCondition) {
            return GetCount(new[] { whereCondition });
        }

        /// <summary>
        /// Return the number of rows that match the supplied WhereConditions
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereCondtions to filter by</param>
        /// <returns></returns>
        public virtual long GetCount(IEnumerable<WhereCondition> whereConditions) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string query = $"SELECT COUNT(*) FROM {Mappings.TableName} ";
            if (whereConditions != null && whereConditions.Any()) {
                List<string> whereStatements = new List<string>();
                foreach (WhereCondition whereCondition in whereConditions) {
                    whereStatements.Add(whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " " + DbAdapter.GetParameterPlaceholder(whereCondition.ColumnName));
                    parameters.Add(DbAdapter.GetParameterName(whereCondition.ColumnName), whereCondition.GetParameterValue());
                }
                query += "WHERE " + string.Join(" AND ", whereStatements);
            }
            long count = GetDbConnection().QuerySingle<long>(query, parameters, DbTransaction);
            if (!IsUnitOfWorkPattern)
                GetDbConnection().Close();

            return count;
        }

        /// <summary>
        /// Adds the supplied entity to the database
        /// </summary>
        /// <param name="entity">Entity to add to the database</param>
        public virtual void Add(T entity) {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            foreach (Map map in Mappings.AllMaps.Where(x => x != Mappings.AutonumberMap)) {
                columns.Add(map.ColumnName);
                values.Add(DbAdapter.GetParameterPlaceholder(map.ColumnName));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName), map.GetValue(entity));
            }



            string query = string.Format(
               "INSERT INTO {0} " +
               "({1})" +
               "VALUES " +
               "({2})",
              Mappings.TableName, string.Join(",", columns), string.Join(",", values));

            if (Mappings.AutonumberMap == null) {
                GetDbConnection().Execute(query, parameters, DbTransaction);
            } else {
                long autonumberValue = 0;
                if (DbAdapter.LastInsertIdInSeparateQuery) {
                    GetDbConnection().Execute(query, parameters, DbTransaction);
                    autonumberValue = GetDbConnection().QuerySingle<long>(DbAdapter.GetLastInsertIdStatement(), null, DbTransaction);
                } else {
                    query += ";" + DbAdapter.GetLastInsertIdStatement();
                    autonumberValue = GetDbConnection().QuerySingle<long>(query, parameters, DbTransaction);
                }
                PropertyInfo prop = entity.GetType().GetProperty(Mappings.AutonumberMap.PropertyName);
                Mappings.AutonumberMap.SetValue(entity, Convert.ChangeType(autonumberValue, prop.PropertyType));
            }

            if (!IsUnitOfWorkPattern)
                GetDbConnection().Close();
        }

        /// <summary>
        /// Updates the matching row in the database
        /// </summary>
        /// <param name="entity">Entity to update in the database</param>
        public virtual void Update(T entity) {
            List<string> updateColumns = new List<string>();
            List<string> primaryKeyColumns = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            foreach (Map map in Mappings.StandardMaps) {
                updateColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName), map.GetValue(entity));
            }

            foreach (Map map in Mappings.PrimaryKeyMaps) {
                primaryKeyColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName), map.GetValue(entity));
            }

            string query = string.Format("UPDATE {0} SET {1} WHERE {2}",
                Mappings.TableName, string.Join(", ", updateColumns), string.Join(" AND ", primaryKeyColumns));

            GetDbConnection().Execute(query, parameters, DbTransaction);

            if (!IsUnitOfWorkPattern)
                GetDbConnection().Close();
        }

        /// <summary>
        /// Removes the matching row from the database
        /// </summary>
        /// <param name="entity">Entity to remove from the database</param>
        public virtual void Remove(T entity) {
            List<string> primaryKeyColumns = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            foreach (Map map in Mappings.PrimaryKeyMaps) {
                primaryKeyColumns.Add(map.ColumnName + "=" + DbAdapter.GetParameterPlaceholder(map.ColumnName));
                parameters.Add(DbAdapter.GetParameterName(map.ColumnName), map.GetValue(entity));
            }

            string query = string.Format("DELETE FROM {0} WHERE {1}",
                Mappings.TableName, string.Join(" AND ", primaryKeyColumns));

            GetDbConnection().Execute(query, parameters, DbTransaction);

            if (!IsUnitOfWorkPattern)
                GetDbConnection().Close();
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
            foreach (WhereCondition whereCondition in whereConditions) {
                if (whereCondition.IsNullEqualsOrNotEquals()) {
                    whereStatements.Add(whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " NULL");
                } else {
                    whereStatements.Add(whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " " + DbAdapter.GetParameterPlaceholder(whereCondition.ColumnName));
                    query.AddParameter(DbAdapter.GetParameterName(whereCondition.ColumnName), whereCondition.GetParameterValue());
                }
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
            List<T> entities = GetDbConnection().Query<T>(queryBuilder.SqlQuery, queryBuilder.Parameters, DbTransaction).ToList();

            if (!IsUnitOfWorkPattern)
                GetDbConnection().Close();

            return entities;
        }

        /// <summary>
        /// Returns a new instance of QueryBuilder for this repository
        /// </summary>
        /// <returns></returns>
        protected QueryBuilder GetQueryBuilder() {
            return new QueryBuilder(Mappings.TableName, Mappings.AllMaps);
        }

        /// <summary>
        /// Creates a new WhereCondition using a strongly-typed property name
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
            return new WhereCondition(Mappings.TableName + "." + columnName, value, Comparison.Equals);
        }

        /// <summary>
        /// Creates a new WhereCondition usinf a strongly-typed property name
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
            return new WhereCondition(Mappings.TableName + "." + columnName, value, comparison);
        }
        #endregion

        /// <summary>
        /// Get the current DbConnection or creates a new one
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetDbConnection() {
            if (DbConnection == null)
                DbConnection = DbAdapter.CreateNewDbConnection(ConnectionString);

            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();

            return DbConnection;
        }

        /// <summary>
        /// Returns the basic SELECT and FROM  parts of a SQL statement for the datebase table
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBasicSelectText() {
            List<string> columns = new List<string>();
            List<string> joins = new List<string>();
            foreach (Map map in Mappings.AllMaps) {
                columns.Add(DbAdapter.GetSelectColumnCast(typeof(T), Mappings.TableName, map));
            }
            if (Mappings.JoinedMaps.Any()) {
                foreach (JoinedMap joinedMap in Mappings.JoinedMaps) {
                    columns.Add(joinedMap.TableAndColumnName + " " + joinedMap.PropertyName);
                    if (!string.IsNullOrWhiteSpace(joinedMap.JoinStatement))
                        joins.Add(joinedMap.JoinStatement);
                }
            }

            string query = $"SELECT {string.Join(",", columns)} FROM {Mappings.TableName} ";
            if (joins.Any())
                query += string.Join(" ", joins.Distinct()) + " ";
            return query;
        }

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
