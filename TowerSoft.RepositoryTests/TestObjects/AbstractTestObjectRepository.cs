using System.Collections.Generic;
using TowerSoft.Repository;
using TowerSoft.RepositoryTests.Interfaces;

namespace TowerSoft.RepositoryTests.TestObjects {
    public class AbstractTestObjectRepository : DbRepository<TestObject> {
        public AbstractTestObjectRepository(IUnitOfWork uow) : base(uow.DbAdapter) { }

        public List<TestObject> GetAllSorted() {
            return GetEntities(QueryBuilder
                .OrderBy(x => x.InputOn)
                .OrderBy(x => x.Title));
        }

        public TestObject GetByID(long id) {
            return GetSingleEntity(WhereEqual(x => x.ID, id));
        }

        public TestObject GetByTitle(string title) {
            return GetSingleEntity(WhereEqual(x => x.Title, title));
        }

        public List<TestObject> GetByDescription(string description) {
            return GetEntities(QueryBuilder
                .WhereEqual(x => x.Description, description)
                .WhereEqual(x => x.StatusID, Status.Active)
                .OrderBy(x => x.Title));
        }


        public List<TestObject> GetByDescriptionWithInputOnOrderAsc(string description) {
            return GetEntities(QueryBuilder
                .WhereEqual(x => x.Description, description)
                .WhereEqual(x => x.StatusID, Status.Active)
                .OrderBy(x => x.InputOn));
        }

        public List<TestObject> GetByDescriptionWithInputOnOrderDesc(string description) {
            return GetEntities(QueryBuilder
                .WhereEqual(x => x.Description, description)
                .WhereEqual(x => x.StatusID, Status.Active)
                .OrderByDescending(x => x.InputOn));
        }

        public List<TestObject> GetByDescriptionWithLimit(string description, int limit) {
            return GetEntities(QueryBuilder
                .WhereEqual(x => x.Description, description)
                .OrderByDescending(x => x.ID)
                .LimitTo(limit));
        }

        public List<TestObject> GetByDescriptionWithLimitAndOffset(string description, int limit, int offset) {
            return GetEntities(QueryBuilder
                .WhereEqual(x => x.Description, description)
                .OrderBy(x => x.ID)
                .LimitTo(limit)
                .OffsetBy(offset));
        }

        public List<TestObject> GetByDescriptionWithLimitOffsetAndSort(string description, int limit, int offset) {
            return GetEntities(QueryBuilder
                .WhereEqual(x => x.Description, description)
                .OrderBy(x => x.InputOn)
                .LimitTo(limit)
                .OffsetBy(offset));
        }
    }
}
