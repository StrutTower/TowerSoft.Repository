using System.Collections.Generic;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class AbstractTestObjectRepository : DbRepository<TestObject> {
        public AbstractTestObjectRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

        public TestObject GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public TestObject GetByTitle(string title) {
            return GetSingleEntity(WhereEqual(x => x.Title, title));
        }

        public List<TestObject> GetByDescription(string description) {
            return GetEntities(WhereEqual(x => x.Description, description));
        }
    }
}
