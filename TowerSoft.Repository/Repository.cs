using System;
using System.Collections.Generic;
using System.Text;

namespace TowerSoft.Repository {
    [Obsolete("This classes is being renamed to DbRepository so it's named different than the namespace. Please switch to using DbRepository.")]
    public class Repository<T> : DbRepository<T> {
        /// <summary>
        /// Creates a new Repository using attributes to define the database mapping.
        /// All non-virtual properties with public getters and setters will be mapped
        /// unless overridden with an attribute.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// /// <param name="overrideTableName">Overrides the table name generated from the object name or the table attribute.</param>
        /// <param name="ignoreObjectMaps">Sets if properties that end with _Object or _Objects will be ignored by the automatic mapping</param>
        public Repository(IDbAdapter dbAdapter, string overrideTableName = null, bool ignoreObjectMaps = true) : base(dbAdapter, overrideTableName, ignoreObjectMaps) { }

        /// <summary>
        /// Creates a new Repository using an EntityMap to define the database mapping.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="entityMap">Entity map class used to define the mapping for this repository</param>
        public Repository(IDbAdapter dbAdapter, EntityMap<T> entityMap) : base(dbAdapter, entityMap) { }

        /// <summary>
        /// Creates a new Repository with the table name and maps passed directly into the contructor.
        /// </summary>
        /// <param name="dbAdapter">DbAdapter class for the database being used</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="maps">List of maps for this tabe</param>
        public Repository(IDbAdapter dbAdapter, string tableName, IEnumerable<IMap> maps) : base(dbAdapter, tableName, maps) { }
    }
}
