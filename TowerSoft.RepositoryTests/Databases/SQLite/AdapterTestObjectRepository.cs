using TowerSoft.RepositoryTests.SQLite;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.Databases.SQLite {
    internal class AdapterTestObjectRepository(UnitOfWork uow) : DbRepositoryKey<AdapterTestObject, long>(uow.DbAdapter) {

        public AdapterTestObject GetByTitle(string title) =>
            GetSingleEntity(WhereEqual(x => x.Title, title));

        internal List<AdapterTestObject> GetByDescription(string description) =>
            GetEntities(WhereEqual(x=>x.Description, description));
    }
}
