using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.TestObjects;

namespace TowerSoft.RepositoryTests.MySql {
    public class TestObjectRepository : Repository<TestObject> {
        public TestObjectRepository(UnitOfWork uow) : base(uow.DbAdapter) { }

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
