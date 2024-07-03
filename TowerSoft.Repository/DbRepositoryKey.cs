using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TowerSoft.Repository {
    /// <summary>
    /// Repository wrapper for Dapper. This extends the default DbRepository class and is intended to be used on table with 
    /// a single primary key. Provides GetByID and GetAllDictionary methods
    /// </summary>
    /// <typeparam name="TDomainObject">Domain object type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type</typeparam>
    public class DbRepositoryKey<TDomainObject, TPrimaryKey> : DbRepository<TDomainObject> {
        /// <summary>
        /// Map for the primary key
        /// </summary>
        protected IMap PrimaryKeyMap { get; private set; }

        #region Constructors
        /// <summary>
        /// Creates a new Repository using attributes to define the database mapping.
        /// The name of the primary key will be ID unless changed.
        /// All non-virtual properties with public getters and setters will be mapped
        /// unless overridden with an attribute.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="overrideTableName">Overrides the table name generated from the object name or the table attribute.</param>
        public DbRepositoryKey(IDbAdapter dbAdapter, string overrideTableName = null)
            : base(dbAdapter, overrideTableName) {
            ValidatePrimaryKeyExists();
        }

        /// <summary>
        /// Creates a new Repository using an EntityMap to define the database mapping.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="entityMap">Entity map class used to define the mapping for this repository</param>
        public DbRepositoryKey(IDbAdapter dbAdapter, EntityMap<TDomainObject> entityMap)
            : base(dbAdapter, entityMap) {
            ValidatePrimaryKeyExists();
        }

        /// <summary>
        /// Creates a new Repository with the table name and maps passed directly into the constructor.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="maps">List of maps for this tabe</param>
        public DbRepositoryKey(IDbAdapter dbAdapter, string tableName, IEnumerable<IMap> maps)
            : base(dbAdapter, tableName, maps) {
            ValidatePrimaryKeyExists();
        }
        #endregion

        private void ValidatePrimaryKeyExists() {
            Type domainType = typeof(TDomainObject);
            Type primaryKeyType = typeof(TPrimaryKey);

            if (Mappings.PrimaryKeyMaps.Count > 1)
                throw new Exception($"Multiple primary key maps found on {domainType.Name}. The DbRepositoryKey class can only be used with a single primary key.");

            IMap primaryMap = Mappings.PrimaryKeyMaps.First();
            if (primaryMap == null)
                throw new Exception($"Unable to find a primary key map on {domainType.Name}");

            PrimaryKeyMap = primaryMap;

            PropertyInfo propInfo = domainType.GetProperty(PrimaryKeyMap.PropertyName);
            if (propInfo == null)
                throw new Exception($"{domainType.Name} does not contain a property named '{PrimaryKeyMap.PropertyName}'");

            if (propInfo.PropertyType != primaryKeyType)
                throw new Exception($"The type of property '{PrimaryKeyMap.PropertyName}' on {domainType.Name} does not match the supplied type. {primaryKeyType} was supplied but expected {propInfo.PropertyType}");
        }

        /// <summary>
        /// Get the entity from the database by the supplied primary key.
        /// </summary>
        /// <param name="id">Primay key value</param>
        public TDomainObject GetByID(TPrimaryKey id) => GetByIDAsync(id).Result;

        /// <summary>
        /// Get the entity from the database by the supplied primary key.
        /// </summary>
        /// <param name="id">Primay key value</param>
        public async Task<TDomainObject> GetByIDAsync(TPrimaryKey id) {
            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"WHERE {PrimaryKeyMap.ColumnName} = @ID";
            query.AddParameter("@ID", id);
            return await GetSingleEntityAsync(query);
        }

        /// <summary>
        /// Get the entities from the database by the supplied primary keys.
        /// </summary>
        /// <param name="ids">Primay key values</param>
        public List<TDomainObject> GetByIDs(IEnumerable<TPrimaryKey> ids) =>
            GetByIDsAsync(ids).Result;


        /// <summary>
        /// Get the entities from the database by the supplied primary keys.
        /// </summary>
        /// <param name="ids">Primay key values</param>
        public async Task<List<TDomainObject>> GetByIDsAsync(IEnumerable<TPrimaryKey> ids) {
            if (ids == null || !ids.Any()) return new();

            QueryBuilder query = GetQueryBuilder();
            query.SqlQuery += $"WHERE {PrimaryKeyMap.ColumnName} IN (";

            int counter = 1;
            List<string> inStatements = new();
            foreach (TPrimaryKey id in ids.Distinct()) {
                inStatements.Add("@p" + id);
                query.AddParameter("@p" + id, id);
                counter++;
            }
            query.SqlQuery += string.Join(",", inStatements) + ") ";

            return await GetEntitiesAsync(query);
        }

        /// <summary>
        /// Get a dictionary of all items from the database using the primary key of the dictionary's key.
        /// </summary>
        public Dictionary<TPrimaryKey, TDomainObject> GetAllDictionary() =>
            GetAllDictionaryAsync().Result;

        /// <summary>
        /// Get a dictionary of all items from the database using the primary key of the dictionary's key.
        /// </summary>
        public async Task<Dictionary<TPrimaryKey, TDomainObject>> GetAllDictionaryAsync() {
            List<TDomainObject> entities = await GetAllAsync();
            Type type = typeof(TDomainObject);
            PropertyInfo prop = type.GetProperty(PrimaryKeyMap.PropertyName);

            Dictionary<TPrimaryKey, TDomainObject> dictionary = new();
            foreach (TDomainObject entity in entities) {
                dictionary.Add((TPrimaryKey)prop.GetValue(entity), entity);
            }
            return dictionary;
        }
    }
}
