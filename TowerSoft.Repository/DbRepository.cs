using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using TowerSoft.Repository.Attributes;
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
        public DbRepository(IDbAdapter dbAdapter, string overrideTableName = null) {
            DbAdapter = dbAdapter;
            ConnectionString = dbAdapter.ConnectionString;
            if (string.IsNullOrWhiteSpace(overrideTableName))
                TableName = GetTableName();
            else
                TableName = overrideTableName;
            Mappings = new MappingModel<T>();
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
        /// Creates a new Repository with the table name and maps passed directly into the constructor.
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
        /// Can be overridden to run code on entities after GetEntities or GetSingleEntity. This method does nothing unless overridden.
        /// </summary>
        /// <param name="entities">Entities retrieved from the database</param>
        protected virtual void PostProcessEntities(List<T> entities) { }

        private string GetTableName() {
            Type domainType = typeof(T);

            string tableName = domainType.Name;
            if (domainType.IsDefined(typeof(TableNameAttribute))) {
                TableNameAttribute tableAttr = (TableNameAttribute)domainType.GetCustomAttribute(typeof(TableNameAttribute));
                if (!string.IsNullOrWhiteSpace(tableAttr.Name)) {
                    tableName = tableAttr.Name;
                }
            }

            return tableName;
        }

        private void WriteLog(string typeName, string query, Dictionary<string, object> parameters) {
            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{typeName} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");
        }
    }
}
