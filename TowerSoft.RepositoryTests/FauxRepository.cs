using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Builders;
using TowerSoft.Repository.Maps;
using TowerSoft.Repository.MySql;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests {
    /// <summary>
    /// Fake repository for unit tests
    /// </summary>
    public class FauxRepository : DbRepository<FauxRepoTestObject> {
        public FauxRepository() : base(new MySqlDbAdapter("")) { }

        public FauxRepository(string overrideTableName) : base(new MySqlDbAdapter(""), overrideTableName) { }

        public FauxRepository(EntityMap<FauxRepoTestObject> entityMap) : base(new MySqlDbAdapter(""), entityMap) { }

        //HACK Using placeholder parameters to call different constructors. There's probably a better way to do this.
        public FauxRepository(bool useManualMaps) : base(new MySqlDbAdapter(""), "", GetMaps()) { }

        public FauxRepository(int useMapBuilder) : base(new MySqlDbAdapter(""), "", GetMapBuilderMaps()) { }

        private static ICollection<IMap> GetMaps() {
            return new[] {
                new AutonumberMap("ID"),
                new Map("Title"),
                new Map("Description"),
                new Map("StatusID", "Status"),
                new Map("InputOn"),
                new Map("InputByID"),
                new Map("IsActive")
            };
        }

        private static List<IMap> GetMapBuilderMaps() {
            return new MapBuilder<FauxRepoTestObject>()
                .AutonumberMap(x => x.ID)
                .Map(x => x.Title)
                .Map(x => x.Description)
                .Map(x => x.StatusID, "Status")
                .Map(x => x.InputOn)
                .Map(x => x.InputByID)
                .Map(x => x.IsActive);
        }

        public string GetTableName() {
            return TableName;
        }

        public IMap GetAutonumberMap() {
            return Mappings.AutonumberMap;
        }

        public List<IMap> GetPrimaryKeyMaps() {
            return Mappings.PrimaryKeyMaps;
        }

        public List<IMap> GetAllMaps() {
            return Mappings.AllMaps;
        }

        public string GetQueryBuilderSqlQuery() {
            return GetQueryBuilder().SqlQuery;
        }

        public string GetQueryBuilderSqlQueryWithCustomColumns() {
            List<string> customColumns = new List<string> {
                "CustomColumn",
                "$INTERNAL(CustomColumn2)"
            };
            return GetQueryBuilder(customColumns).SqlQuery;
        }
    }
}
